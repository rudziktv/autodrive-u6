using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Modules.Drivetrain;

namespace Core.Entities.Vehicle.Builders
{
    public static class DrivetrainBuilder
    {
        public static DrivetrainModule BuildDrivetrain(this VehicleDrivetrainData drivetrainData, VehicleController ctr)
        {
            var engine = drivetrainData.engine.BuildEngine(ctr);
            var gearbox = drivetrainData.gearbox.BuildGearbox(ctr);
            var transferCase = drivetrainData.transferCase.BuildTransferCase(ctr);
            return new DrivetrainModule(engine, gearbox, transferCase, ctr);
        }
    }
}