using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain
{
    [CreateAssetMenu(fileName = "Manual Gearbox", menuName = "Vehicle/Manual Gearbox", order = 0)]
    public class VehicleManualGearboxData : VehicleGearboxData
    {
        [Header("Clutch")]
        public AnimationCurve clutchResponseCurve;
    }
}