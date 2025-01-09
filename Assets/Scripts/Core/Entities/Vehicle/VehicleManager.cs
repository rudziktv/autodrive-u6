using Core.Entities.Vehicle.Configs.Interactions;
using Core.Entities.Vehicle.Managers;

namespace Core.Entities.Vehicle
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