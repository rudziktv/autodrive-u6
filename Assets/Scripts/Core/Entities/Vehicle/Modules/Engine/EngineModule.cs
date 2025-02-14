using Core.Entities.Vehicle.Data.Drivetrain;

namespace Core.Entities.Vehicle.Modules.Engine
{
    public class EngineModule : DrivetrainSubmodule
    {
        public EngineModule(VehicleController ctr) : base(ctr) { }
        
        public virtual float CurrentRPM => 0f;
        public virtual float CurrentTorque { get; protected set; }
    }
}