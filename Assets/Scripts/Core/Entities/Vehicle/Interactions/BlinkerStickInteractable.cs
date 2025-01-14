using System;
using Core.Components.Sounds;
using Core.Entities.Vehicle.Animations;
using Core.Entities.Vehicle.Enums;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions
{
    public class BlinkerStickInteractable : VehicleInteractable
    {
        #region Settings

        [SerializeField] private bool comfortBlinkers;
        
        [SerializeField] private FMODCustomEmitter stickEngageEmitter;
        [SerializeField] private FMODCustomEmitter stickCancelEmitter;

        #endregion
        
        public BlinkerStickState CurrentBlinkerState { get; private set; }
        public event Action<BlinkerStickState> BlinkerStickStateChanged; 
        
        private void Start()
        {
            Input.Functions.LeftBlinker.performed += OnLeftBlinkerButton;
            Input.Functions.RightBlinker.performed += OnRightBlinkerButton;
        }

        private void SetBlinkerState(BlinkerStickState state)
        {
            if (CurrentBlinkerState == state) return;
            CurrentBlinkerState = state;
            InvokeBlinkerEvent(state);
        }

        private void InvokeBlinkerEvent(BlinkerStickState state)
        {
            switch (state)
            {
                case BlinkerStickState.LeftComfort: // TODO
                case BlinkerStickState.RightComfort:
                    break;
                case BlinkerStickState.Zero:
                    stickCancelEmitter.Play();
                    Animator.SetInteger(InteractionVehicleAnimParams.BlinkerStick, (int)state);
                    break;
                default:
                    stickEngageEmitter.Play();
                    Animator.SetInteger(InteractionVehicleAnimParams.BlinkerStick, (int)state);
                    break;
            }
            BlinkerStickStateChanged?.Invoke(state);
        }

        private void OnLeftBlinkerButton(InputAction.CallbackContext ctx)
        {
            if (CancelBlinkerIfNotZero()) return;
            SetBlinkerState(BlinkerStickState.Left);
        }

        private void OnRightBlinkerButton(InputAction.CallbackContext ctx)
        {
            if (CancelBlinkerIfNotZero()) return;
            SetBlinkerState(BlinkerStickState.Right);
        }

        /// <summary>
        /// Set blinker stick to ZERO position
        /// </summary>
        /// <returns>If blinker was zero before - false, otherwise true.</returns>
        private bool CancelBlinkerIfNotZero()
        {
            if (CurrentBlinkerState == BlinkerStickState.Zero) return false;
            SetBlinkerState(BlinkerStickState.Zero);
            return true;
        }
    }
}