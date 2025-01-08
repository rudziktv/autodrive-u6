using System;
using Entities.Vehicle.Enums;
using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Vehicle.Interactions
{
    public class KeyIgnitionInteractable : MonoBehaviour
    {
        public delegate void KeyIgnitionEventHandler(KeyPositionState newState, KeyPositionState oldState);
        public KeyPositionState CurrentKeyPositionState { get; private set; } = KeyPositionState.Off;
        
        public event KeyIgnitionEventHandler OnStateChanged;

        public void ToggleKeyIgnition(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            
            var old = CurrentKeyPositionState;
            
            switch (CurrentKeyPositionState)
            {
                case KeyPositionState.Off:
                    CurrentKeyPositionState = KeyPositionState.Ignition;
                    break;
                case KeyPositionState.Ignition:
                    CurrentKeyPositionState = KeyPositionState.Off;
                    break;
            }
            
            Debug.Log(CurrentKeyPositionState);
            
            Handle(old);
        }

        private void Handle(KeyPositionState oldState)
        {
            OnStateChanged?.Invoke(CurrentKeyPositionState, oldState);
        }

        public void HoldKeyStarter()
        {
            // TO DO
        }
    }
}