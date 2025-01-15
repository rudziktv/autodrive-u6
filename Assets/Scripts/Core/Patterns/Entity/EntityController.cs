using System;
using UnityEngine;

namespace Core.Patterns.Entity
{
    public class EntityController<G, M> : MonoBehaviour where M : EntityModule where G : EntityGroupModule<M>
    {
        public G Modules { get; private set; }

        private void Start()
        {
            Modules?.InitializeModules();
        }

        protected virtual void Update()
        {
            Modules?.UpdateModules();
        }

        protected virtual void FixedUpdate()
        {
            Modules?.FixedUpdateModules();
        }
    }
}