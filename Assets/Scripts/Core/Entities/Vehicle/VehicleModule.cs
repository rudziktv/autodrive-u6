using Core.Entities.Vehicle.Configs;
using Core.Entities.Vehicle.Configs.Interactions;
using Core.Entities.Vehicle.Managers;

namespace Core.Entities.Vehicle
{
    public class VehicleModule
    {
        protected readonly VehicleController Controller;
        protected VehicleConfigManager VehicleConfigs => Controller.VehicleConfigs;
        protected ElectricityManager ElectricityManager => Controller.ElectricityManager;
        protected InteractionsConfig Interactions => VehicleConfigs.InteractionsConfig;
        protected ComponentsReferences Components => VehicleConfigs.ComponentsReferences;

        protected VehicleModule(VehicleController controller)
        {
            Controller = controller;
        }

        public virtual void Initialize() { }
        public virtual void UpdateModule() { }
        public virtual void FixedUpdateModule() { }

        public T GetModule<T>() where T : VehicleModule
        {
            return Controller.ModuleManager.GetModule<T>();
        }
    }
}