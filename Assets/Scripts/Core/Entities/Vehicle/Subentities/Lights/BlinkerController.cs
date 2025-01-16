using Core.Components.Lights.Standalone;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Lights
{
    public class BlinkerController : MonoBehaviour
    {
        [SerializeField] private LightLevelStandalone lights;

        public void TurnOnBlinker()
        {
            lights.ApplyAnimated();
        }

        public void TurnOffBlinker()
        {
            lights.ApplyAnimated(0f);
        }

        public void ApplyBlinker(bool active)
        {
            lights.ApplyAnimated(active ? 1f : 0f);
        }
    }
}