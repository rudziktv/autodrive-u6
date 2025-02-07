using Core.Entities.Vehicle.Builders;
using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Modules.Engine;

namespace Core.Entities.Vehicle.Modules.Drivetrain
{
    public class DrivetrainModule : VehicleModule
    {
        public EngineModule Engine { get; private set; }

        public DrivetrainModule(VehicleController ctr) : base(ctr)
        {
            Engine = EngineBuilder.BuildEngine(VehicleData.drivetrain.engine, ctr);
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            Engine.UpdateModule();
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            Engine.FixedUpdateModule();
        }
    }
}