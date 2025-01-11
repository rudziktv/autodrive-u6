using Core.Components.Sounds;
using Core.Entities.Vehicle.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions
{
    public class KeyIgnitionInteractable : MonoBehaviour
    {
        [SerializeField] private FMODCustomEmitter keyfobOffEmitter;
        [SerializeField] private FMODCustomEmitter keyfobIgnitionEmitter;
        
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
            
            UnityEngine.Debug.Log(CurrentKeyPositionState);
            
            Handle(old);
        }

        private void Handle(KeyPositionState oldState)
        {
            OnStateChanged?.Invoke(CurrentKeyPositionState, oldState);
            
            if (CurrentKeyPositionState == KeyPositionState.Ignition)
                keyfobIgnitionEmitter.Play();
            else if (CurrentKeyPositionState is KeyPositionState.Off)
                keyfobOffEmitter.Play();
        }

        public void HoldKeyStarter()
        {
            // TO DO
        }
    }
}