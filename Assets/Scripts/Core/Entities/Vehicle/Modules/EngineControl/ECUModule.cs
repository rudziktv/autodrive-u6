using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Modules.Engine;

namespace Core.Entities.Vehicle.Modules.EngineControl
{
    public class ECUModule : DrivetrainSubmodule
    {
        protected EngineModule Engine =>
            GetModule<EngineModule>();
        
        public ECUModule(VehicleController ctr) : base(ctr) { }

        protected T GetEngineModule<T>() where T : EngineModule
            => (T)Drivetrain.Engine;
        
    }
}