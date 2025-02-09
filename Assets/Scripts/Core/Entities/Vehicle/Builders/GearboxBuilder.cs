using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Gearbox;
using Core.Entities.Vehicle.Modules.Gearbox;

namespace Core.Entities.Vehicle.Builders
{
    public static class GearboxBuilder
    {
        public static GearboxModule BuildGearbox(this VehicleGearboxData gearboxData, VehicleController ctr)
        {
            if (gearboxData is VehicleManualGearboxData manualGearboxData)
                return new ManualGearboxModule(manualGearboxData, ctr);

            return new GearboxModule(ctr);
        }
    }
}