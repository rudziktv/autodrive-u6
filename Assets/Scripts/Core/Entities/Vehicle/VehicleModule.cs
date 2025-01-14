using System.Collections;
using Core.Entities.Vehicle.Configs;
using Core.Entities.Vehicle.Configs.Interactions;
using Core.Entities.Vehicle.Managers;
using UnityEngine;

namespace Core.Entities.Vehicle
{
    public class VehicleModule
    {
        protected readonly VehicleController Controller;
        protected VehicleConfigManager VehicleConfigs => Controller.VehicleConfigs;
        protected ElectricityManager ElectricityManager => Controller.ElectricityManager;
        protected InteractionsConfig Interactions => VehicleConfigs.InteractionsConfig;
        protected ComponentsReferences Components => VehicleConfigs.ComponentsReferences;
        protected SoundsConfig Sounds => VehicleConfigs.SoundsConfig;
        
        protected Coroutine StartCoroutine(IEnumerator enumerator) =>
            Controller.StartCoroutine(enumerator);

        protected void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine == null) return;
            Controller.StopCoroutine(coroutine);
        }

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