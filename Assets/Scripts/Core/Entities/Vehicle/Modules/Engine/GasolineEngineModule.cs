using System.Collections;
using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Modules.EngineControl;
using Core.Utils;
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
        
        public GasolineEngineModule(CombustionEngineData data, GasolineECUModule ecu, VehicleController ctr) : base(ctr)
        {
            _data = data;
            Flywheel = new(_data, ctr);
            ECU = ecu;
        }

        public override void Initialize()
        {
            base.Initialize();
            AirTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            BlockTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            OilTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
            CoolantTemperature = SimpleEnvironment.instance.AmbientTemperatureKelvin;
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
            

            if (ComfortConfig.DashboardConfig.CoolantGauge)
                ComfortConfig.DashboardConfig.CoolantGauge.Value = TempUnitUtils.KelvinToCelsius(CoolantTemperature);
            
            // oil
            const float  oilThermalExpansion = 0.0007f;
            var oilArea = cylindersAreaM2 * _data.cylinders * 2f;
            var dynamicOilDensity = (oilDensity * 1000f) * (1 - oilThermalExpansion * (OilTemperature - TempUnitUtils.CelsiusToKelvin(15)));
            var oilVolumetricFlow = _data.oilPressureRpmFactor * FlywheelRPM;
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
            var blockLossArea = (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 4f * 4f
                                + (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 2f;
            // var oConst = 5.67f * Mathf.Pow(10, -8);
            // var lossQ = metalEmissionFactor * oConst * blockLossArea * (BlockTemperature - outsideTemp);
            var blockLossQ = aluminiumHeatGainFactor * blockLossArea * (BlockTemperature - outsideTemp) * Time.fixedDeltaTime;
            
            BlockTemperature -= blockLossQ / aluminiumSpecificHeat / blockMass;
            
            // oil temp loss
            var oilLossArea = (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 4f * 2f;
            var oilLossQ = oilHeatGainFactor * oilLossArea * (BlockTemperature - outsideTemp) * Time.fixedDeltaTime;
            OilTemperature -= oilLossQ / oilSpecificHeat / oilMass;
            
            // coolant temp loss
            var coolantLossArea = (_data.cylinderBore * _data.cylinderStroke / 1000000f) * 4f;
            var coolantLossQ = coolantHeatGainFactor * oilLossArea * (BlockTemperature - outsideTemp) * Time.fixedDeltaTime;
            CoolantTemperature -= coolantLossQ / coolantSpecificHeat / coolantMass;
            
            Debug.Log($"Loss Q: {blockLossQ}, oilQ: {oilQ}, coolantQ: {coolantQ}, oil T: {TempUnitUtils.KelvinToCelsius(OilTemperature)}, coolant T: {TempUnitUtils.KelvinToCelsius(CoolantTemperature)}, block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}");
            // Debug.Log($"Δ Oil T: {deltaOil}, Oil X: {x}, Oil Q: {oilQ}, Oil Density: {dynamicOilDensity}, Oil T {TempUnitUtils.KelvinToCelsius(OilTemperature)}, Coolant T: {TempUnitUtils.KelvinToCelsius(CoolantTemperature)}, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}");
            // Debug.Log($"W: {totalE} kW, Q: {totalQ} kW, {CurrentFuelConsumption} l/h, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, ø Air T: {TempUnitUtils.KelvinToCelsius(avgAirTemperature)}, Air Peak T: {TempUnitUtils.KelvinToCelsius(workAirTempPeak)}, cycles per frame {cycles * Time.fixedDeltaTime}");
            // Debug.Log($"W: {totalE} kW, Q: {totalQ} kW, {CurrentFuelConsumption} l/h, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, cycles per frame {cycles * Time.fixedDeltaTime}");
            // Debug.Log($"ΔT: {airDeltaTemp}, Block T: {TempUnitUtils.KelvinToCelsius(BlockTemperature)}, Block Q: {blockQ}, Air T: {TempUnitUtils.KelvinToCelsius(AirTemperature)}, Δ Air T: {blockQ / airSpecificHeat / airMass}, ø Air T: {TempUnitUtils.KelvinToCelsius(avgAirTemperature)}, Air Mass: {airMass}");
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