using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain.Gearbox
{
    [CreateAssetMenu(fileName = "Manual Gearbox", menuName = "Vehicle/Manual Gearbox", order = 0)]
    public class VehicleManualGearboxData : VehicleGearboxData
    {
        [Header("Clutch")]
        public AnimationCurve clutchResponseCurve;

        public float clutchFrictionFactor = 0.5f;
        public float clutchMaxPressure = 5000f;
        public float clutchRadius = 0.11f;
        public float power = 1f;
        public float connectionDifference = 5f;
    }
}