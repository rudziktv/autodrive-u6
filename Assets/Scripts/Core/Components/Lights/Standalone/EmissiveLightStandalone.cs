using UnityEngine;

namespace Core.Components.Lights.Standalone
{
    public class EmissiveLightStandalone : MonoBehaviour
    {
        public virtual void ApplyInstantEmission(float percentage = 1f) { }
        public virtual void ApplyAnimatedEmission(float duration, float percentage = 1f) { }
        public virtual void StopTransition() { }
    }
}