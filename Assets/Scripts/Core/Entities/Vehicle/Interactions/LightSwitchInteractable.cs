using System;
using Core.Entities.Vehicle.Enums;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions
{
    public class LightSwitchInteractable : InteractableItem, IStateful<LightSwitchState>
    {
        public LightSwitchState CurrentLightSwitchState { get; private set; }
        
        
        public event Action<LightSwitchState> OnStateChanged;

        public void ToggleLights(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            
            if (CurrentLightSwitchState == LightSwitchState.LowBeams)
                CurrentLightSwitchState = LightSwitchState.Off;
            else
                CurrentLightSwitchState++;

            LightsChanged();
        }

        private void LightsChanged()
        {
            OnStateChanged?.Invoke(CurrentLightSwitchState);
            UnityEngine.Debug.Log(CurrentLightSwitchState);
            // animation
            // sound
            // NO EFFECT ON ACTUAL HEADLIGHTS ETC!!!
        }
    }

    [Serializable]
    public class LightSwitchOrder
    {
        [SerializeField] private LightSwitchState lightState;
        
    }
}