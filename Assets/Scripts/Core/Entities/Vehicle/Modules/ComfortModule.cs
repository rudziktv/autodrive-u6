using Core.Entities.Vehicle.Submodules.Comfort;

namespace Core.Entities.Vehicle.Modules
{
    public class ComfortModule : ElectricalModule
    {
        public LightsModule Lights { get; private set; }
        public BlinkerModule Blinker { get; private set; }
        public DashboardModule Dashboard { get; private set; }


        public ComfortModule(VehicleController controller) : base(controller)
        {
            Lights = new(controller);
            Blinker = new(controller);
            Dashboard = new(controller);
        }

        public override void Initialize()
        {
            base.Initialize();
            Lights.Initialize();
            Blinker.Initialize();
            Dashboard.Initialize();
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            Lights.UpdateModule();
            Blinker.UpdateModule();
            Dashboard.UpdateModule();
        }
    }
}