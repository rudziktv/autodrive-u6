using Core.Entities.Vehicle.Submodules.Comfort;

namespace Core.Entities.Vehicle.Modules
{
    public class ComfortModule : ElectricalModule
    {
        public LightsModule Lights { get; private set; }
        public BlinkersModule Blinkers { get; private set; }
        public DashboardModule Dashboard { get; private set; }


        public ComfortModule(VehicleController controller) : base(controller)
        {
            Lights = new(controller);
            Blinkers = new(controller);
            Dashboard = new(controller);
        }

        public override void Initialize()
        {
            base.Initialize();
            Lights.Initialize();
            Blinkers.Initialize();
            Dashboard.Initialize();
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            Lights.UpdateModule();
            Blinkers.UpdateModule();
            Dashboard.UpdateModule();
        }
    }
}