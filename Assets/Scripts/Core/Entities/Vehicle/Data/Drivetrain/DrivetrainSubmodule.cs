using Core.Entities.Vehicle.Modules;
using Core.Entities.Vehicle.Modules.Drivetrain;

namespace Core.Entities.Vehicle.Data.Drivetrain
{
    public class DrivetrainSubmodule : ElectricalModule
    {
        public DrivetrainSubmodule(VehicleController ctr) : base(ctr) { }
        
        protected DrivetrainModule Drivetrain => GetDrivetrainModule<DrivetrainModule>();
        protected DrivetrainModule GetDrivetrainModule<T>() where T : DrivetrainModule
            => (T)GetModule<DrivetrainModule>();
        
    }
}