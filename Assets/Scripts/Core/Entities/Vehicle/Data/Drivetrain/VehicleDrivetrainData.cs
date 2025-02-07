using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Data.Drivetrain.TransferCase;
using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain
{
    [CreateAssetMenu(fileName = "Drivetrain Data", menuName = "Vehicle/Drivetrain", order = 0)]
    public class VehicleDrivetrainData : ScriptableObject
    {
        public VehicleEngineData engine;
        public VehicleGearboxData gearbox;
        public TransferCaseData transferCase;
    }
}