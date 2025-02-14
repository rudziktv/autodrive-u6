using Core.Entities.Vehicle.Data.Drivetrain;

namespace Core.Entities.Vehicle.Modules.Gearbox
{
    public class GearboxModule : DrivetrainSubmodule
    {
        public virtual float OutputRPM { get; protected set; }
        
        public GearboxModule(VehicleController ctr) : base(ctr) { }

        public virtual bool IsNeutral() => false;
    }
}