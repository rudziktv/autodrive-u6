using Entities.Vehicle.Configs.Interactions;
using Entities.Vehicle.Managers;

namespace Entities.Vehicle
{
    public class VehicleManager
    {
        protected VehicleController Controller;
        protected VehicleConfigManager Config => Controller.VehicleConfigManager;
        protected InteractionsConfig Interactions => Config.InteractionsConfig; 

        public VehicleManager(VehicleController controller)
        {
            Controller = controller;
        }
    }
}