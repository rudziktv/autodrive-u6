using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Patterns.Entity
{
    public class EntityGroupModule<TM> where TM : EntityModule
    {
        private MonoBehaviour _controller;
        
        private readonly Dictionary<Type, TM> _modules = new();

        public EntityGroupModule(MonoBehaviour controller)
        {
            _controller = controller;
        }

        public bool AddModule(TM module)
        {
            var type = module.GetType();
            return _modules.TryAdd(type, module);
        }
        
        public T GetModule<T>() where T : TM
        {
            var type = typeof(T);
            _modules.TryGetValue(type, out var module);
            return (T)module;
        }

        public void RemoveModule(Type moduleType)
        {
            if (_modules.ContainsKey(moduleType))
                _modules.Remove(moduleType);
        }

        public void InitializeModules()
        {
            foreach (var module in _modules.Values)
            {
                module.Initialize();
            }
        }

        public void UpdateModules()
        {
            foreach (var module in _modules.Values)
            {
                module.Update();
            }
        }

        public void FixedUpdateModules()
        {
            foreach (var module in _modules.Values)
            {
                module.FixedUpdate();
            }
        }
    }
}