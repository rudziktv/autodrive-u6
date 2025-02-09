using Core.Entities.Vehicle.Data.Drivetrain.EngineControl;
using Systems.Info;
using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain.Engine
{
    [CreateAssetMenu(fileName = "Combustion Engine", menuName = "Vehicle/Engine/Combustion Engine", order = 0)]
    public class CombustionEngineData : VehicleEngineData
    {
        [Header("Combustion Info")]
        public ECUData ecu;
        public int capacity;
        public string cylindersInfo;
        public string valvesInfo;
        public string intakeInfo;
        public string powerInfo;
        public string torqueInfo;

        public float compressionRatio;
        public int cylinders;
        public float cylinderBore;
        public float cylinderStroke;
        
        [Header("Fuel")]
        public FuelTypeEnum fuelType;
        public float targetOctane;
        public float octaneTolerance;
        public float targetCetane;
        public float cetaneTolerance;
        
        [Header("Revs")]
        public float stallRev;

        public float redLineRev;
        public bool redLineCutOff;
        public float redLineCutOffTime;

        public float timingGearRevLimit;

        [Header("Other")]
        public float innerDrag;
        public float innerRevDrag;
        public AnimationCurve innerRevDragCurve;

        public float torqueBasedDrag;
        public float flywheelWeight;
        public float flywheelRadius;
        public float flywheelFreeMotionFactor;

        public float intakeResponseTime;
        public AnimationCurve intakeResponseCurve;
        
        [Header("Arcade Fuel Consumption")]
        public float fuelConsumptionBase = 120f;
        public float fuelConsumptionRpmFactor = 500f;
        public float fuelConsumptionFactor = 1f;
        public float fuelConsumptionCapacityFactor = 1;

        [Header("Cooling")]
        public float coolantFlow = 1f;
        public float coolantFlowRpmFactor = 1f;

        public float oilPressure = 5f;
        public float oilPressureRpmFactor = 1f;
    }
}