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

        // public float ThermalConductivity { get; }
        // public float Type { get; set; }
        
        public float InsideCylinderArea { get; }
        public float OutsideCylinderArea { get; }
        public float InnerSideWallsCylinderArea { get; }
        public float OuterSideWallsCylinderArea { get; }

        private ThermalTickUpdate _tickMode;

        public float Tick => _tickMode switch
        {
            ThermalTickUpdate.FixedUpdate => Time.fixedDeltaTime,
            ThermalTickUpdate.Update => Time.deltaTime,
            _ => 1
        };

        public CylinderThermalInfo Info { get; }


        /// <summary>
        /// Creates metal cylinder, with walls and air inside.
        /// </summary>
        /// <param name="initialTemp">Initial temperature of cylinder walls and air inside, K</param>
        /// <param name="thermalCapacity">Thermal capacity of cylinder material, kJ/kg*K</param>
        /// <param name="thermalConductivity">Thermal conductivity of cylinder material, kW/m²*K</param>
        /// <param name="density">Density of cylinder material, kg/m³</param>
        /// <param name="bore">Bore of cylinder, mm</param>
        /// <param name="stroke">Stroke of cylinder, mm</param>
        /// <param name="thickness">Thickness of cylinder walls, mm</param>
        public CylinderThermalModel(float initialTemp, CylinderThermalInfo info,
            ThermalTickUpdate tick = ThermalTickUpdate.FixedUpdate)
        {
            _tickMode = tick;
            Info = info;
            
            // ThermalConductivity = info.ThermalConductivity;
            
            CylinderVolume = VolumeUtils.ConvertMm3ToM3(Mathf.PI * Mathf.Pow(info.Bore / 2f, 2) * info.Stroke);
            var cylinderVolumeThick = VolumeUtils.ConvertMm3ToM3(Mathf.PI *
                Mathf.Pow(info.Bore / 2f + info.Thickness, 2) * info.Stroke);
            var cylinderWallVolume = cylinderVolumeThick - CylinderVolume;
            
            
            var cylinderWallMass = DensityUtils.GetMass(cylinderWallVolume, info.Density);
            var initialAirMass = DensityUtils.GetMass(CylinderVolume, info.Density);
            
            Walls = new ThermalModel(initialTemp, cylinderWallMass, info.ThermalCapacity);
            Air = new CompressThermalModel(DensityUtils.AIR_DENSITY, ThermalUtils.AIR_DIABETIC_INDEX,
                initialTemp, initialAirMass, ThermalUtils.AIR_THERMAL_CAPACITY);

            InsideCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderAreaDiameter(info.Bore, info.Stroke));
            OutsideCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderArea(info.Bore / 2f + info.Thickness, info.Stroke));

            InnerSideWallsCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderSideAreaDiameter(info.Bore, info.Stroke));
            OuterSideWallsCylinderArea = AreaUtils.ConvertMm2ToM2(AreaUtils.
                CalculateCylinderSideArea(info.Bore / 2f + info.Thickness, info.Stroke));
        }

        public void OttoCycle(float qPerCycle, float intakeAirTemp, float cylinderCyclesInFrame)
        {
            if (cylinderCyclesInFrame == 0)
            {
                Air.Mass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY);
                Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                    InnerSideWallsCylinderArea);
                return;
            }
            
            var airMass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY) * cylinderCyclesInFrame;
            
            // SUCK
            Air.Temperature = (Air.Temperature * 0.07f + intakeAirTemp * 0.93f) * 0.98f;
            Air.Mass = airMass;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            
            // COMPRESS
            Air.CompressionRatio = Info.CompressionRatio;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);

            // WORK
            var q = qPerCycle * cylinderCyclesInFrame * 0.5f;
            Air.TransferHeat(q);
            Air.CompressionRatio = 1;
            Air.TransferHeat(q);
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);

            // EXHAUST
            Air.Temperature =
                SimpleEnvironment.instance.AmbientTemperatureKelvin + Air.Temperature * 0.07f;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
        }
        
        public void OttoCycle(float qPerCycle, float intakeAirTemp, float cylinderCyclesInFrame, out CylinderThermalOttoCycleDebug debug)
        {
            debug = new CylinderThermalOttoCycleDebug();
            
            if (cylinderCyclesInFrame == 0)
            {
                Air.Mass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY);
                Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                    InnerSideWallsCylinderArea);
                debug.WorkTemp = Air.Temperature;
                return;
            }
            
            var airMass = DensityUtils.GetMass(CylinderVolume, DensityUtils.AIR_DENSITY) * cylinderCyclesInFrame;
            
            // SUCK
            Air.Temperature = (Air.Temperature * 0.07f + intakeAirTemp * 0.93f) * 0.98f;
            Air.Mass = airMass;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            
            debug.IntakeTemp = Air.Temperature;

            
            // COMPRESS
            Air.CompressionRatio = Info.CompressionRatio;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            
            debug.CompressTemp = Air.Temperature;


            // WORK
            // // var q = qPerCycle * cylinderCyclesInFrame * 0.5f;
            // // Air.TransferHeat(q);
            // Air.CompressionRatio = 1;
            // // Air.TransferHeat(q);
            // Air.TransferHeat(qPerCycle * cylinderCyclesInFrame);
            // Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
            //     InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            //
            // debug.WorkTemp = Air.Temperature;
            
            // Air.TransferHeat(qPerCycle * cylinderCyclesInFrame);
            // Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
            //     InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            // debug.ExtremeTemp = Air.Temperature;
            //
            // Air.CompressionRatio = 1;
            // Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
            //     InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            // debug.WorkTemp = Air.Temperature;

            const float initCoeff = 0.9f;
            const float endCoeff = 0.1f;
            
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * initCoeff);
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.ExtremeTemp = Air.Temperature;
            
            Air.CompressionRatio = 1;
            // Air.Temperature *= ;
            Air.TransferHeat(qPerCycle * cylinderCyclesInFrame * endCoeff * (1f - Info.Efficiency));
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            debug.WorkTemp = Air.Temperature;


            // EXHAUST
            Air.Temperature =
                SimpleEnvironment.instance.AmbientTemperatureKelvin + Air.Temperature * 0.07f;
            Walls.ConvectTransferHeat(Air, Info.ThermalConductivity,
                InnerSideWallsCylinderArea, cylinderCyclesInFrame);
            
            debug.ExhaustTemp = Air.Temperature;

        }
    }
}