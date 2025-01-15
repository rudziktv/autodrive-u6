using UnityEngine;

namespace Core.Patterns.Entity
{
    public class EntityModule
    {
        private readonly MonoBehaviour _controller;
        
        public EntityModule(MonoBehaviour controller)
        {
            _controller = controller;
        }
    
        protected virtual T GetController<T>() where T : MonoBehaviour
            => (T)_controller;
        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}