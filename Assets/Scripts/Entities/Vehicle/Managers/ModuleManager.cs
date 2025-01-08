using System;
using System.Collections.Generic;

namespace Entities.Vehicle.Managers
{
    public class ModuleManager : VehicleManager
    {
        public ModuleManager(VehicleController controller) : base(controller) { }
        
        private readonly Dictionary<Type, VehicleModule> _modules = new();

        public bool RegisterModule(VehicleModule module)
        {
            var type = module.GetType();
            return _modules.TryAdd(type, module);
        }
        
        public T GetModule<T>() where T : VehicleModule
        {
            var type = typeof(T);
            _modules.TryGetValue(type, out var module);
            return (T)module;
        }

        public void InitializeModules()
        {
            foreach (var module in _modules.Values)
            {
                module.Initialize();
            }
        }

        public void UpdateAllModules()
        {
            foreach (var module in _modules.Values)
            {
                module.UpdateModule();
            }
        }

        public void FixedUpdateAllModules()
        {
            foreach (var module in _modules.Values)
            {
                module.FixedUpdateModule();
            }
        }
    }
}