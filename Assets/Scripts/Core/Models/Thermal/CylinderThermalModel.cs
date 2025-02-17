using Core.Utils.Physical;
using Systems.Environment;
using UnityEngine;

namespace Core.Models.Thermal
{
    public class CylinderThermalModel
    {
        /// <summary>
        /// Walls of the cylinder
        /// </summary>
        public ThermalModel Walls { get; }
        
        /// <summary>
        /// Air inside the cylinder
        /// </summary>
        public CompressThermalModel Air { get; }
        
        /// <summary>
        /// Cylinder Volume, in m³, thickness not included.
        /// </summary>
        public float CylinderVolume { get; }
        
        public float InsideCylinderArea { get; }
        public float OutsideCylinderArea { get; }
        public float InnerSideWallsCylinderArea { get; }
        public float OuterSideWallsCylinderArea { get; }

        private readonly ThermalTickUpdate _tickMode;

        public float Tick => _tickMode switch
        {
            ThermalTickUpdate.FixedUpdate => Time.fixedDeltaTime,
            ThermalTickUpdate.Update => Time.deltaTime,
            _ => 1
        };

        public CylinderThermalParameters Parameters { get; }


        /// <summary>
        /// Creates metal cylinder, with walls and air inside.
        /// </summary>
        /// <param name="initialTemp">Initial temperature of cylinder walls and air inside, K</param>
        /// <param name="parameters">Cylinder parameters</param>
        /// <param name="tick">Default tick rate, which most of the calculations are multiplied by</param>
        public CylinderThermalModel(float initialTemp, CylinderThermalParameters parameters,
            ThermalTickUpdate tick = ThermalTickUpdate.FixedUpdate)
        {
            _tickMode = tick;
            Parameters = parameters;
            CylinderVolume = VolumeUtils.ConvertMm3ToM3(Mathf.PI * Mathf.Pow(parameters.Bore / 2f, 2) * parameters.Stroke);
            
            var cylinderVolumeThick = VolumeUtils.ConvertMm3ToM3(Mathf.PI *
                Mathf.Pow(parameters.Bore / 2f + parameters.Thickness, 2) * parameters.Stroke);
            var cylinderWallVolume = cylinderVolumeThick - CylinderVolume;
            var cylinderWallMass = DensityUtils.GetMass(cylinderWallVolume, parameters.Density);
            var initialAirMass = DensityUtils.GetMass(CylinderVolume, parameters.Density);
            
            Walls = new ThermalModel(initialTemp, cylinderWallMass, parameters.ThermalCapacity);
            Air = new CompressThermalModel(DensityUtils.AIR_DENSITY, ThermalUtils.AIR_DIABETIC_INDEX,
                initialTemp, initialAirMass, ThermalUtils.AIR_THERMAL_CAPACITY);

            InsideCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderAreaDiameter(parameters.Bore, parameters.Stroke));
            OutsideCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderArea(parameters.Bore / 2f + parameters.Thickness, parameters.Stroke));

            InnerSideWallsCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderSideAreaDiameter(parameters.Bore, parameters.Stroke));
            OuterSideWallsCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderSideArea(parameters.Bore / 2f + parameters.Thickness, parameters.Stroke));
        }

        /// <summary>
        /// Simulates engine thermal exchange in Otto Cycle, between air inside cylinder and cylinder's walls.
        /// </summary>
        /// <param name="qPerCycle">Amount of heat from burnt fuel, J</param>
        /// <param name="intakeAirTemp">Temperature of intake air coming into combustion cycle, K</param>
        /// <param name="cylinderCyclesInFrame">Amount of cycles in cylinder in last frame</param>
        public void OttoCycle(float qPerCycle, float intakeAirTemp, float cylinderCyclesInFrame)
        {
            if (cylinderCyclesInFrame == 0) // if engine shut down, convect between static amount of air
            {
                Air.Mass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY);
                Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                    InnerSideWallsCylinderArea);
                return;
            }
            
            var airMass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY) * cylinderCyclesInFrame;
            
            #region INTAKE
            
            Air.Temperature = (Air.Temperature * 0.07f + intakeAirTemp * 0.93f) * 0.98f;
            Air.Mass = airMass;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);

            #endregion

            #region COMPRESS
            
            Air.CompressionRatio = Parameters.CompressionRatio;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);

            #endregion

            #region WORK
            
            const float initCoeff = 0.9f;
            const float endCoeff = 0.1f;
            // EXTREME WORK TEMPERATURE
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * initCoeff);
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            // DECOMPRESSION WORK PHASE
            Air.CompressionRatio = 1;
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * endCoeff * (1f - Parameters.Efficiency));
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            
            #endregion

            #region EXHAUST
            
            Air.Temperature =
                SimpleEnvironment.instance.AmbientTemperatureKelvin + Air.Temperature * 0.07f;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);

            #endregion
        }
        
        /// <summary>
        /// Simulates engine thermal exchange in Otto Cycle, between air inside cylinder and cylinder's walls.
        /// </summary>
        /// <param name="qPerCycle">Amount of heat from burnt fuel, J</param>
        /// <param name="intakeAirTemp">Temperature of intake air coming into combustion cycle, K</param>
        /// <param name="cylinderCyclesInFrame">Amount of cycles in cylinder in last frame</param>
        /// <param name="debug">Debug output object, helpful to debug combustion thermal cycle</param>
        public void OttoCycle(float qPerCycle, float intakeAirTemp, float cylinderCyclesInFrame, out CylinderThermalOttoCycleDebug debug)
        {
            debug = new CylinderThermalOttoCycleDebug();
            
            if (cylinderCyclesInFrame == 0) // if engine shut down, convect between static amount of air
            {
                Air.Mass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY);
                Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                    InnerSideWallsCylinderArea);
                debug.WorkTemp = Air.Temperature;
                return;
            }
            
            var airMass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY) * cylinderCyclesInFrame;
            
            #region INTAKE
            
            Air.Temperature = (Air.Temperature * 0.07f + intakeAirTemp * 0.93f) * 0.98f;
            Air.Mass = airMass;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.IntakeTemp = Air.Temperature;

            #endregion

            #region COMPRESS
            
            Air.CompressionRatio = Parameters.CompressionRatio;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.CompressTemp = Air.Temperature;

            #endregion

            #region WORK
            
            const float initCoeff = 0.9f;
            const float endCoeff = 0.1f;
            // EXTREME WORK TEMPERATURE
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * initCoeff);
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.ExtremeTemp = Air.Temperature;
            // DECOMPRESSION WORK PHASE
            Air.CompressionRatio = 1;
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * endCoeff * (1f - Parameters.Efficiency));
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.WorkTemp = Air.Temperature;
            
            #endregion

            #region EXHAUST
            
            Air.Temperature =
                SimpleEnvironment.instance.AmbientTemperatureKelvin + Air.Temperature * 0.07f;
            Walls.ConvectTransferHeat(Air, Parameters.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.ExhaustTemp = Air.Temperature;

            #endregion
        }
    }
}