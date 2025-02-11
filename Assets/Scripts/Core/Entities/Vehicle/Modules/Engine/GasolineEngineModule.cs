using System.Collections;
using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Modules.EngineControl;
using Core.Models.Thermal;
using Core.Utils;
using Core.Utils.Physical;
using Systems.Environment;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Core.Entities.Vehicle.Modules.Engine
{
    public class GasolineEngineModule : CombustionEngineModule
    {
        private readonly CombustionEngineData _data;
        private float _throttle;
        
        public float Throttle
        {
            get => _throttle;
            set => _throttle = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Air temperature inside cylinders after exhaust, in Kelvins.
        /// </summary>
        public float AirTemperature { get; private set; }
        
        /// <summary>
        /// Block temperature, in Kelvins.
        /// </summary>
        public float BlockTemperature { get; private set; }
        
        /// <summary>
        /// Oil temperature, in Kelvins.
        /// </summary>
        public float OilTemperature { get; private set; }
        
        /// <summary>
        /// Coolant temperature, in Kelvins.
        /// </summary>
        public float CoolantTemperature { get; private set; }
        
        
        
        public FlywheelModule Flywheel { get; }
        public GasolineECUModule ECU { get; set; }
        public float FlywheelRPM => Flywheel.RPM;
        public float CurrentTorque { get; private set; }

        private float _lastDeltaTorque;

        private bool _cutOff;
        private Coroutine _cutOffRoutine;

        /// <summary>
        /// Mass of the fuel, burnt in the last frame, in grams.
        /// </summary>
        public float LastFrameFuelConsumption { get; private set; }

        /// <summary>
        /// Volume of burnt fuel in the last frame, in l/h.
        /// </summary>
        public float CurrentFuelConsumption { get; private set; }

        private float _oilTempLossTimer;

        public CompressThermalModel AirThermal { get; set; }
        public ThermalModel BlockThermal { get; set; }
        public ThermalModel OilThermal { get; set; }
        public ThermalModel CoolantThermal { get; set; }
        
        public GasolineEngineModule(CombustionEngineData data, GasolineECUModule ecu, VehicleController ctr) : base(ctr)
        {
            _data = data;
            Flywheel = new(_data, ctr);
            ECU = ecu;

            InitializeThermalModel();
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeThermalModel();
            
            AirTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            BlockTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            // OilTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            // CoolantTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;

            OilTemperature = TempUnitUtils.CelsiusToKelvin(90);
            CoolantTemperature = TempUnitUtils.CelsiusToKelvin(90);
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            ECU.UpdateModule();
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            ECU.FixedUpdateModule();
            Flywheel.FixedUpdateModule();

            FuelConsumption();
            Cycle();
            TemperatureCycle();
            NewTemperatureCycle();

            // Debug.Log($"RPM: {FlywheelRPM}, Thr: {Throttle}, Torque: {CurrentTorque}, Idle: {MyThrottleIdleControl()}");
            ComfortConfig.DashboardConfig.Tachometer.Value = Mathf.Clamp(FlywheelRPM, 0f, 10000f);
            // Debug.Log($"Oil: {OilTemperature}, Coolant: {CoolantTemperature}");
        }

        private void TemperatureCycle()
        {
            var outsideTemp = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            
            // kJ*kg/K
            const float oilSpecificHeat = 2.2f;
            const float coolantSpecificHeat = 3.8f;
            const float aluminiumSpecificHeat = 0.9f;
            const float steelSpecificHeat = 0.5f;
            const float airSpecificHeat = 1.005f;

            // kW/m2K
            const float aluminiumHeatGainFactor = 0.2f;
            const float coolantHeatGainFactor = 0.5f / 1000f;
            const float oilHeatGainFactor = 0.15f / 1000f;
            
            const float fuelEnergy = 45; // MJ
            const float efficiency = 0.35f;
            
            // const float blockFraction = 0.1f;
            const float coolantFraction = 0.35f;
            const float oilFraction = 0.08f;

            const float coolantDensity = 1.05f; // avg in 15 celsius degrees
            const float coolantVolume = 7f; // in liters
            
            const float oilDensity = 0.85f; // in 15 celsius degrees
            const float oilVolume = 3.6f; // in liters
            
            const float thermalConductivity = 0.01f;
            
            const float airDensity = 1.29f;
            const float airDiabeticIndex = 1.4f;

            const float blockMass = 35f;

            var oilMass = oilVolume * oilDensity;
            var coolantMass = coolantVolume * coolantDensity;
            // var coolantMass

            var cycles = (FlywheelRPM / 60f) * (_data.cylinders / 2f);
            var cyclesPerFrame = cycles * Time.fixedDeltaTime;
            
            // Debug.Log($"idiotic. Oil Mass: {oilMass} XD units, Coolant Mass: {coolantMass} XD units");
            
            
            // var totalE = LastFrameFuelConsumption * fuelEnergy; // MJ to kJ
            var totalE = CurrentFuelConsumption * 0.75f / 3600f * fuelEnergy * 1000f; // MJ to kJ
            var work = totalE * efficiency;
            var totalQ = totalE - work;
            // totalQ /= cycles;
            // totalQ /= (FlywheelRPM / 2f);

            // var airVolume = _data.capacity / 1000000f / 2f;
            var airVolume = cycles == 0f ? _data.capacity / 1000000f : cycles * _data.capacity / _data.cylinders / 1000000f;
            // var airVolume = (float)_data.capacity / _data.cylinders / 1000000f;
            var compressedAirVolume = (airVolume / _data.compressionRatio);
            var airMass = airVolume * airDensity;
            var airDeltaTemp = totalQ / airSpecificHeat / airMass;

            var cylindersAreaMm2 = 2 * Mathf.PI * (_data.cylinderBore / 2f) +
                _data.cylinderStroke * 2 * Mathf.PI * (_data.cylinderBore / 2f);
            var cylindersAreaM2 = cylindersAreaMm2 / 1000000f;
            
            // AirTemperature += Mathf.Pow(airDeltaTemp * (airVolume / compressedAirVolume), airDiabeticIndex); // peak temperature

            var intakeTemp = outsideTemp + 20;
            
            
            // precise model
            if (cycles != 0f)
            {
                var baseAirTemp = intakeTemp * 0.93f + AirTemperature * 0.07f;
                var intakeAirTemp = baseAirTemp * 0.95f;
                var blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (intakeAirTemp - BlockTemperature) * Time.fixedDeltaTime * cyclesPerFrame;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
                
                var compressAirTemp = intakeAirTemp * Mathf.Pow((airVolume / compressedAirVolume),
                    airDiabeticIndex - 1);
                blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (compressAirTemp - BlockTemperature) * Time.fixedDeltaTime * cyclesPerFrame;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
                
                var workAirTempPeak = compressAirTemp + airDeltaTemp;
                blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (workAirTempPeak - BlockTemperature) * Time.fixedDeltaTime * cyclesPerFrame;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
                
                var workAirTemp = workAirTempPeak * Mathf.Pow((compressedAirVolume / airVolume), airDiabeticIndex - 1);
                blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (workAirTemp - BlockTemperature) * Time.fixedDeltaTime * cyclesPerFrame;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
                
                var exhaustAirTemp = workAirTemp * 0.07f;
                blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (exhaustAirTemp - BlockTemperature) * Time.fixedDeltaTime * cyclesPerFrame;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
                AirTemperature = workAirTemp;
                
                Debug.Log($"Intake: {intakeAirTemp}, compress: {compressAirTemp} Peak: {workAirTempPeak}, Decomp: {workAirTemp}");
            }
            else
            {
                var blockQ = aluminiumHeatGainFactor * cylindersAreaM2 * (AirTemperature - BlockTemperature) *
                             Time.fixedDeltaTime;
                BlockTemperature += blockQ / aluminiumSpecificHeat / blockMass;
                AirTemperature -= blockQ / airSpecificHeat / airMass;
            }
            
            
            
            // var avgAirTemperature = (intakeAirTemp + compressAirTemp + workAirTempPeak + workAirTemp + exhaustAirTemp) / 5f;
            
            // coolant
            const float coolantCableDiameter = 0.02f;
            // var nu = 
            var coolantThermalFlow = CoolantTemperature / TempUnitUtils.CelsiusToKelvin(90);
            var coolantHeatGainFactorWithFlow = coolantHeatGainFactor / coolantCableDiameter * 3.66f
                * (FlywheelRPM / _data.redLineRev * _data.coolantFlowRpmFactor)
                * _data.coolantFlow;
            
            var coolantThermalArea = cylindersAreaM2 * _data.cylinders * 16f;
            var coolantQ = coolantHeatGainFactor * coolantThermalArea * (BlockTemperature - CoolantTemperature) * Time.fixedDeltaTime;
            if (cycles != 0f)
            {
                coolantQ = coolantHeatGainFactorWithFlow * coolantThermalFlow * coolantThermalArea * (BlockTemperature - CoolantTemperature)
                                      * Time.fixedDeltaTime;
            }
            
            CoolantTemperature += coolantQ / coolantSpecificHeat / coolantMass;
            BlockTemperature -= coolantQ / aluminiumSpecificHeat / blockMass;
            
            
            // var coolantQ = aluminiumHeatGainFactor * coolantThermalArea * (BlockTemperature - CoolantTemperature)
            //                * Time.fixedDeltaTime;

            const float testOilPressureFactor = 0.001f;

            // if (ComfortConfig.DashboardConfig.CoolantGauge)
            //     ComfortConfig.DashboardConfig.CoolantGauge.Value = TempUnitUtils.KelvinToCelsius(CoolantTemperature);
            
            // oil
            const float oilThermalExpansion = 0.0007f;
            var oilArea = cylindersAreaM2 * _data.cylinders * 2f;
            var dynamicOilDensity = (oilDensity * 1000f) * (1 - oilThermalExpansion * (OilTemperature - TempUnitUtils.CelsiusToKelvin(15)));
            var oilVolumetricFlow = testOilPressureFactor * FlywheelRPM;
            var dynamicOilMass = dynamicOilDensity * oilVolumetricFlow;

            var oilQ = oilHeatGainFactor * oilArea * (BlockTemperature - OilTemperature) * Time.fixedDeltaTime;
            var deltaOil = oilQ / oilSpecificHeat / oilMass;
            var x = 0f;
            
            if (cycles != 0f)
            {
                x = dynamicOilMass / oilMass * Time.fixedDeltaTime;
                deltaOil = oilHeatGainFactor * oilArea * (BlockTemperature - OilTemperature) / (oilMass * oilSpecificHeat) * x;
                oilQ = oilMass * oilSpecificHeat * deltaOil;
            }
            
            
            OilTemperature += deltaOil;
            BlockTemperature -= oilQ / aluminiumSpecificHeat / blockMass;
            
            // heat loss

            // block temp loss
            const float metalEmissionFactor = 0.2f;
            var blockLossArea = (((_data.cylinderBore * 4f / 1000f) * (_data.cylinderStroke / 1000f)) * 4f
                                 + (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 2f) * 1.5f;
            // var oConst = 5.67f * Mathf.Pow(10, -8);
            // var lossQ = metalEmissionFactor * oConst * blockLossArea * (BlockTemperature - outsideTemp);
            var blockLossQ = aluminiumHeatGainFactor * blockLossArea * (BlockTemperature - outsideTemp) * Time.fixedDeltaTime;
            
            BlockTemperature -= blockLossQ / aluminiumSpecificHeat / blockMass;

            const float oilHeatLossFactor = 5f / 1000f;
            
            _oilTempLossTimer += Time.fixedDeltaTime;
            var oilLossArea = (_data.cylinderBore * 4f / 1000f) * (_data.cylinderBore / 1000f) * 2f;
            // var oilLossArea = 0.15f;
            var oilLossQ = oilHeatLossFactor * oilLossArea * (OilTemperature - outsideTemp) * _oilTempLossTimer;

            if (_oilTempLossTimer > 2f)
            {
                OilTemperature -= oilLossQ / oilSpecificHeat / oilMass;
                _oilTempLossTimer = 0;
            }

            const float coolantHeatLossFactor = 35f / 1000f;
            
            // coolant temp loss
            var coolantLossArea = ((_data.cylinderBore * 4f / 1000f) * (_data.cylinderStroke / 1000f)) * 4f;
            var coolantLossQ = coolantHeatLossFactor * coolantLossArea * (CoolantTemperature - outsideTemp) * Time.fixedDeltaTime;
            CoolantTemperature -= coolantLossQ / coolantSpecificHeat / coolantMass;
            
            // Debug.Log($"Δ oil loss: {oilLossQ / oilSpecificHeat / oilMass}, oil area: {oilLossArea}, coolant area: {coolantLossArea}, block area: {blockLossArea}");
            // Debug.Log($"Loss Q: {blockLossQ}, oil loss Q: {oilLossQ}, Δ oil loss: {oilLossQ / oilSpecificHeat / oilMass}, Δ oil: {deltaOil}, coolant loss Q: {coolantLossQ}, oil T: {TempUnitUtils.KelvinToCelsius(OilTemperature)}, coolant T: {TempUnitUtils.KelvinToCelsius(CoolantTemperature)}, block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}");
            // Debug.Log($"Δ Oil T: {deltaOil}, Oil X: {x}, Oil Q: {oilQ}, Oil Density: {dynamicOilDensity}, Oil T {TempUnitUtils.KelvinToCelsius(OilTemperature)}, Coolant T: {TempUnitUtils.KelvinToCelsius(CoolantTemperature)}, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}");
            // Debug.Log($"W: {totalE} kW, Q: {totalQ} kW, {CurrentFuelConsumption} l/h, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, ø Air T: {TempUnitUtils.KelvinToCelsius(avgAirTemperature)}, Air Peak T: {TempUnitUtils.KelvinToCelsius(workAirTempPeak)}, cycles per frame {cycles * Time.fixedDeltaTime}");
            Debug.Log($"W: {totalE} kW, Q: {totalQ} kW, {CurrentFuelConsumption} l/h, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, Oil T: {TempUnitUtils.KelvinToCelsius(OilTemperature)}, Coolant T: {TempUnitUtils.KelvinToCelsius(CoolantTemperature)}, cycles per frame {cycles * Time.fixedDeltaTime}");
            // Debug.Log($"Air Mass: {airMass}, Oil D: {dynamicOilDensity}, x: {x}, oilArea: {oilArea} m², oil convect factor: {oilHeatGainFactor}");
            // Debug.Log($"ΔT: {airDeltaTemp}, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Block Q: {blockQ}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, Δ Air T: {blockQ / airSpecificHeat / airMass}, ø Air T: {TempUnitUtils.KelvinToCelsius(avgAirTemperature)}, Air Mass: {airMass}");
        }

        private void InitializeThermalModel()
        {
            // const float fuelEnergy = 45; // MJ
            // const float efficiency = 0.35f;

            // const float coolantDensity = 1.05f; // avg in 15 celsius degrees
            const float coolantVolume = 7f; // in liters
            
            // const float oilDensity = 0.85f; // in 15 celsius degrees
            const float oilVolume = 3.6f; // in liters
            
            const float blockMass = 35f;
            var outsideTemp = SimpleEnvironment.instance.AmbientTemperatureKelvin;

            var oilMass = DensityUtils.GetMass(VolumeUtils.ConvertDm3ToM3(oilVolume), DensityUtils.OIL_DENSITY);
            var coolantMass = DensityUtils.GetMass(VolumeUtils.ConvertDm3ToM3(coolantVolume), DensityUtils.COOLANT_DENSITY);
            
            AirThermal = new CompressThermalModel(
                DensityUtils.AIR_DENSITY, ThermalUtils.AIR_DIABETIC_INDEX, outsideTemp,
                1, ThermalUtils.AIR_THERMAL_CAPACITY, ThermalTickUpdate.FixedUpdate);
            BlockThermal = new ThermalModel(outsideTemp, blockMass, ThermalUtils.ALUMINIUM_THERMAL_CAPACITY);
            OilThermal = new ThermalModel(outsideTemp + 90f, oilMass, ThermalUtils.OIL_THERMAL_CAPACITY);
            CoolantThermal = new ThermalModel(outsideTemp + 90f, coolantMass, ThermalUtils.COOLANT_THERMAL_CAPACITY);

            _engineCapacity = VolumeUtils.ConvertCm3ToM3(_data.capacity);
            _cylindersArea = AreaUtils.ConvertMm2ToM2(2 * Mathf.PI * (_data.cylinderBore / 2f) +
                                                           _data.cylinderStroke * 2 * Mathf.PI * (_data.cylinderBore / 2f));
            
            // Debug.Log($"Cylinders Area: {_cylindersArea} m³, Engine Capacity: {_engineCapacity} m³");
            // Debug.Log($"V: {VolumeUtils.ConvertDm3ToM3(oilVolume)} m³, Oil Mass: {oilMass} kg, Coolant Mass: {coolantMass} kg, V: {VolumeUtils.ConvertDm3ToM3(coolantVolume)} m³");
        }

        private float _engineCapacity;
        private float _cylindersArea;
        private float _newOilTimer;

        private void NewTemperatureCycle()
        {
            const float efficiency = 0.35f;
            const float exhaustRestFactor = 0.07f;
            
            var outsideTemp = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            
            float fuelEnergy = EnergyUtils.PETROL_E5_ENERGY;
            
            var cycles = (FlywheelRPM / 60f) * (_data.cylinders / 2f);
            var cyclesPerFrame = cycles * Time.fixedDeltaTime;
            
            var totalE = CurrentFuelConsumption * 0.75f / 3600f * fuelEnergy * 1000f; // MJ to kJ
            var work = totalE * efficiency;
            var totalQ = totalE - work;
            
            var airVolume = cycles == 0 ? _engineCapacity :
                    cycles * _engineCapacity / _data.cylinders;
            var airMass = DensityUtils.GetMass(airVolume, DensityUtils.AIR_DENSITY);
            
            
            AirThermal.Mass = airMass;

            float intake = 0;
            float compressed = 0;
            float peakTemp = 0;
            float decompressed = 0;
            var intakeTemp = outsideTemp + 20;
            
            // air thermal
            if (cycles != 0)
            {
                // SUCK CYCLE
                AirThermal.Temperature = (intakeTemp * (1f - exhaustRestFactor) + AirThermal.Temperature * exhaustRestFactor) * 0.95f;
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea, cyclesPerFrame);

                intake = AirThermal.Temperature;
                
                // COMPRESSION CYCLE
                AirThermal.CompressionRatio = _data.compressionRatio;
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea, cyclesPerFrame);
                
                compressed = AirThermal.Temperature;
                
                // WORK CYCLE
                
                // compressed work cycle - peak temperature
                AirThermal.TransferHeat(totalQ);
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea, cyclesPerFrame);
                
                peakTemp = AirThermal.Temperature;
                
                // decompressed work cycle
                AirThermal.CompressionRatio = 1;
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea, cyclesPerFrame);

                decompressed = AirThermal.Temperature;
                
                // EXHAUST CYCLE
                AirThermal.Temperature *= 0.92f;
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea, cyclesPerFrame);
            }
            else
            {
                BlockThermal.ConvectTransferHeat(AirThermal,
                    ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                    _cylindersArea);
            }
            
            // small circuit
            
            // COOLANT
            const float coolantCableDiameter = 0.02f;
            var coolantThermalFlow = CoolantTemperature / TempUnitUtils.CelsiusToKelvin(90);
            var coolantHeatGainFactorWithFlow = ThermalUtils.COOLANT_THERMAL_CONDUCTIVITY / coolantCableDiameter * 3.66f
                * (FlywheelRPM / _data.redLineRev * _data.coolantFlowRpmFactor)
                * _data.coolantFlow;
            
            var coolantThermalArea = _cylindersArea * _data.cylinders * 16f;
            
            if (cycles != 0f)
                CoolantThermal.ConvectTransferHeat(BlockThermal, coolantHeatGainFactorWithFlow,
                    coolantThermalArea, coolantThermalFlow);
            else
                CoolantThermal.ConvectTransferHeat(BlockThermal,
                    ThermalUtils.COOLANT_THERMAL_CONDUCTIVITY, coolantThermalArea);
            
            if (ComfortConfig.DashboardConfig.CoolantGauge)
                ComfortConfig.DashboardConfig.CoolantGauge.Value = TempUnitUtils.KelvinToCelsius(CoolantThermal.Temperature);
            
            
            // OIL
            const float oilThermalExpansion = 0.0007f;
            var oilArea = _cylindersArea * _data.cylinders * 2f;
            var dynamicOilDensity = DensityUtils.OIL_DENSITY * (1 - oilThermalExpansion * (OilThermal.Temperature - TempUnitUtils.CelsiusToKelvin(15)));
            var oilVolumetricFlow = _data.oilPressureRpmFactor * FlywheelRPM;
            var dynamicOilMass = dynamicOilDensity * oilVolumetricFlow;
            var x = 1f;

            if (cycles != 0f)
                x = dynamicOilMass / OilThermal.Mass * Time.fixedDeltaTime;
            
            OilThermal.ConvectTransferHeat(BlockThermal, ThermalUtils.OIL_THERMAL_CONDUCTIVITY,
                oilArea, x);
            
            
            // AMBIENT HEAT CONVECTION
            // create private class variable instead of calculating same thing over again
            // use AreaUtils
            var blockLossArea = (((_data.cylinderBore * 4f / 1000f) * (_data.cylinderStroke / 1000f)) * 4f
                                 + (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 2f) * 1.5f;
            BlockThermal.ConvectHeatToAmbient(ThermalUtils.ALUMINIUM_THERMAL_CONDUCTIVITY,
                blockLossArea);
            
            const float oilHeatLossFactor = 5f / 1000f;
            var oilLossArea = (_data.cylinderBore * 4f / 1000f) * (_data.cylinderBore / 1000f) * 2f;
            _newOilTimer += Time.fixedDeltaTime;
            if (_newOilTimer > 2f)
            {
                OilThermal.InstantConvectHeatToAmbient(oilHeatLossFactor,
                    oilLossArea, _newOilTimer);
                _newOilTimer = 0;
            }

            const float coolantHeatLossFactor = 35f / 1000f;
            var coolantLossArea = ((_data.cylinderBore * 4f / 1000f) * (_data.cylinderStroke / 1000f)) * 4f;
            CoolantThermal.ConvectHeatToAmbient(coolantHeatLossFactor, coolantLossArea);
            
            
            Debug.Log($"NEW! Block T {TempUnitUtils.KelvinToCelsius(BlockThermal.Temperature)}, Air T {TempUnitUtils.KelvinToCelsius(AirThermal.Temperature)}, Oil T {TempUnitUtils.KelvinToCelsius(OilThermal.Temperature)}, Coolant T: {TempUnitUtils.KelvinToCelsius(CoolantThermal.Temperature)}");
            Debug.Log($"NEW! Intake: {intake}, Compressed: {compressed}, Peak T: {peakTemp}, Decomp: {decompressed}");
        }

        private void FuelConsumption()
        {
            var fuelDensity = 0.75f;
            
            var rpmFactor = FlywheelRPM * _data.fuelConsumptionRpmFactor;
            var capacityFactor = _data.fuelConsumptionCapacityFactor * _data.capacity;
            var fuelConsumptionCm3PerMinute = _data.fuelConsumptionFactor * (rpmFactor + capacityFactor)
                / _data.compressionRatio * Throttle; // cm3 per minute
            
            LastFrameFuelConsumption = fuelConsumptionCm3PerMinute / 60f * fuelDensity * Time.fixedDeltaTime;
            var fuelLitersPerHour = fuelConsumptionCm3PerMinute / 1000f * 60f;
            CurrentFuelConsumption = fuelLitersPerHour;
        }

        private void Cycle()
        {
            _lastDeltaTorque = (TargetTorque - CurrentTorque) / CurrentIntakeResponse * Time.fixedDeltaTime;
            
            var minTorque = Flywheel.DeltaRPMToEngineTorque(0f - FlywheelRPM);
            var newTorque = Mathf.Clamp(_lastDeltaTorque + CurrentTorque, minTorque, _data.maxTorque);

            CurrentTorque = newTorque;
            Flywheel.TransferEngineTorque(CurrentTorque);
            
            // Debug.Log($"min torque: {minTorque}, new torque: {newTorque}, rpm: {FlywheelRPM}");
        }

        public float GetIntakeResponse(float rpm) =>
            _data.intakeResponseCurve.Evaluate(rpm / _data.torqueRevScale);

        public float CurrentIntakeResponse =>
            GetIntakeResponse(FlywheelRPM);

        public float TotalInnerDrag =>
            _data.innerDrag + CurrentInnerRevDrag + CurrentTorqueBasedDrag;
        
        public float CurrentMaxTorque =>
            _data.torqueCurve.Evaluate(FlywheelRPM / _data.torqueRevScale) * (_data.maxTorque + _data.innerDrag);
            
        public float TargetTorque =>
            CurrentMaxTorque * Throttle - TotalInnerDrag;

        public float CurrentInnerRevDrag =>
            _data.innerRevDrag * _data.innerRevDragCurve.Evaluate(FlywheelRPM / _data.torqueRevScale);
        
        public float CurrentTorqueBasedDrag =>
            _data.torqueBasedDrag * _data.torqueCurve.Evaluate(FlywheelRPM / _data.torqueRevScale);
        
        public float GetMaxTorque(float rpm) =>
            _data.torqueCurve.Evaluate(rpm / _data.torqueRevScale) * (_data.maxTorque + _data.innerDrag);
        
        public float GetTotalInnerDrag(float rpm) =>
            _data.innerDrag + GetInnerRevDrag(rpm) + GetTorqueBasedDrag(rpm);
        
        public float GetInnerRevDrag(float rpm) =>
            _data.innerRevDrag * _data.innerRevDragCurve.Evaluate(rpm / _data.torqueRevScale);

        public float GetTorqueBasedDrag(float rpm) =>
            _data.torqueBasedDrag * _data.torqueCurve.Evaluate(rpm / _data.torqueRevScale);
    }
}