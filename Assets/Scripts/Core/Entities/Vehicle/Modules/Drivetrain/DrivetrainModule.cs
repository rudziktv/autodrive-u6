using Core.Entities.Vehicle.Builders;
using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Modules.Engine;
using Core.Entities.Vehicle.Modules.Gearbox;

namespace Core.Entities.Vehicle.Modules.Drivetrain
{
    public class DrivetrainModule : VehicleModule
    {
        public EngineModule Engine { get; }
        public GearboxModule Gearbox { get; }

        public DrivetrainModule(EngineModule engine, GearboxModule gearbox, VehicleController ctr) : base(ctr)
        {
            Engine = engine;
            Gearbox = gearbox;
        }

        public override void Initialize()
        {
            base.Initialize();
            Engine.Initialize();
            Gearbox.Initialize();
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            Engine.UpdateModule();
            Gearbox.UpdateModule();
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            Engine.FixedUpdateModule();
            Gearbox.FixedUpdateModule();
        }
    }
}