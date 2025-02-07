using Core.Entities.Vehicle.Data.Drivetrain;
using UnityEngine;

namespace Core.Entities.Vehicle.Data
{
    [CreateAssetMenu(fileName = "Vehicle Data", menuName = "Vehicle/Vehicle Instance", order = 0)]
    public class VehicleInstanceData : ScriptableObject
    {
        public VehicleDrivetrainData drivetrain;
    }
}