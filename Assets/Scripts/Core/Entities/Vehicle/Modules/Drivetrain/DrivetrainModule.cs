using Core.Entities.Vehicle.Modules.Engine;
using Core.Entities.Vehicle.Modules.Gearbox;
using Core.Entities.Vehicle.Modules.TransferCase;

namespace Core.Entities.Vehicle.Modules.Drivetrain
{
    public class DrivetrainModule : VehicleModule
    {
        public EngineModule Engine { get; }
        public GearboxModule Gearbox { get; }
        public ITransferCase TransferCase { get; }

        public DrivetrainModule(EngineModule engine, GearboxModule gearbox, ITransferCase transferCase, VehicleController ctr) : base(ctr)
        {
            Engine = engine;
            Gearbox = gearbox;
            TransferCase = transferCase;
        }

        public override void Initialize()
        {
            base.Initialize();
            Engine.Initialize();
            Gearbox.Initialize();
            TransferCase.Initialize();
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            Engine.UpdateModule();
            Gearbox.UpdateModule();
            TransferCase.UpdateModule();
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            Engine.FixedUpdateModule();
            Gearbox.FixedUpdateModule();
            TransferCase.FixedUpdateModule();
        }
    }
}