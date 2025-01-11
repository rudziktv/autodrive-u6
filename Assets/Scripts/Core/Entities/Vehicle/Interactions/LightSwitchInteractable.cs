using System;
using Core.Entities.Vehicle.Animations;
using Core.Entities.Vehicle.Enums;
using FMOD.Studio;
using FMODUnity;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions
{
    public class LightSwitchInteractable : VehicleInteractable, IStateful<LightSwitchState>
    {
        public LightSwitchState CurrentLightSwitchState => lightSwitchOrder[_index];
        
        [SerializeField] private LightSwitchState[] lightSwitchOrder;

        private int _index;
        
        public event Action<LightSwitchState> StateChanged;

        public void ToggleLights(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            _index++;
            
            if (_index >= lightSwitchOrder.Length)
                _index = 0;

            InvokeLightSwitchChange();
        }

        private void InvokeLightSwitchChange()
        {
            StateChanged?.Invoke(CurrentLightSwitchState);
            Debug.Log(CurrentLightSwitchState);
            // animation
            // sound
            // NO EFFECT ON ACTUAL HEADLIGHTS ETC!!!

            // RuntimeManager.StudioSystem.loadBankFile("Volkswagen Golf Mk6.bank", LOAD_BANK_FLAGS.NORMAL, out var bank);
            // bank.getEventList(out var events);
            

            Animator.SetInteger(InteractionVehicleAnimParams.LightSwitch, _index);
        }

        public void PrevLightSwitchPosition()
        {
            _index = Mathf.Clamp(_index - 1, 0, lightSwitchOrder.Length - 1);
            InvokeLightSwitchChange();
        }

        public void NextLightSwitchPosition()
        {
            _index = Mathf.Clamp(_index + 1, 0, lightSwitchOrder.Length - 1);
            InvokeLightSwitchChange();
        }
    }
}