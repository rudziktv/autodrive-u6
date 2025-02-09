using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Gearbox;

namespace Core.Entities.Vehicle.Modules.Gearbox
{
    public class ManualGearboxModule : GearboxModule
    {
        private VehicleManualGearboxData _data;

        public ManualGearboxModule(VehicleManualGearboxData data, VehicleController ctr) : base(ctr)
        {
            _data = data;
        }
    }
}