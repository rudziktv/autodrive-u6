using System;
using System.Collections;
using Core.Components.Vehicles.Mirror;
using Core.Entities.Vehicle.Animations;
using Core.Utils;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions.Mirror
{
    public class MirrorSwitchInteractable : VehicleInteractable
    {
        [Header("Mirror Switch")]
        [SerializeField] private int defaultSwitchPosition;
        [SerializeField] private MirrorSwitchPositions[] switchPositions;
        
        [Header("Interactables")]
        [SerializeField] private Interactable controlInteractable;
        [SerializeField] private Interactable leftInteractable;
        [SerializeField] private Interactable rightInteractable;
        
        [Header("Animation")]
        [SerializeField] private float animationDuration = 0.2f;
        
        [Header("Mirrors Test")]
        [SerializeField] private VehicleElectricMirror leftMirror;
        
        private int _currentSwitchPosition;
        private Coroutine _animationRoutine;
        private Coroutine _controlRoutine;
        
        public MirrorSwitchPositions CurrentSwitchPosition => switchPositions[_currentSwitchPosition];

        private void Start()
        {
            SetPosition(defaultSwitchPosition);
            leftInteractable.interact.AddListener(PreviousPosition);
            rightInteractable.interact.AddListener(NextPosition);
            controlInteractable.onInteractionStarted.AddListener(StartMirrorControl);
        }

        private void StartMirrorControl()
        {
            if (_controlRoutine != null)
                StopCoroutine(_controlRoutine);
            if (CurrentSwitchPosition != MirrorSwitchPositions.Left) return;
            
            Debug.Log("Starting mirror control");

            _controlRoutine = StartCoroutine(MirrorControlRoutine(leftMirror));
            InputSystem.actions.FindAction(GlobalInputs.INTERACT).canceled += StopMirrorControl;
        }

        private void StopMirrorControl(InputAction.CallbackContext ctx)
        {
            Debug.Log("Stop mirror control");
            leftMirror.SetMotorVelocity(Vector2.zero);
            if (_controlRoutine != null)
                StopCoroutine(_controlRoutine);
        }

        private IEnumerator MirrorControlRoutine(VehicleElectricMirror mirror)
        {
            while (true)
            {
                var input = InputSystem.actions.FindAction(GlobalInputs.INTERACTION_CURSOR).ReadValue<Vector2>();
                mirror.SetMotorVelocity(input);
                yield return null;
            }
        }

        private void NextPosition()
            => SetPosition(_currentSwitchPosition + 1);
        private void PreviousPosition()
            => SetPosition(_currentSwitchPosition - 1);
        

        private void SetPosition(int position)
        {
            if (_animationRoutine != null)
                StopCoroutine(_animationRoutine);
            _currentSwitchPosition = Mathf.Clamp(position, 0, switchPositions.Length - 1);
            StartCoroutine(AnimateSwitch(animationDuration));
            
        }

        private IEnumerator AnimateSwitch(float duration)
        {
            var timer = 0f;
            var init = Animator.GetFloat(InteractionVehicleAnimParams.MirrorSwitch);
            var target = _currentSwitchPosition / ((float)switchPositions.Length - 1);
            while (timer <= duration)
            {
                var progress = Mathf.Lerp(init, target, timer / duration);
                Animator.SetFloat(InteractionVehicleAnimParams.MirrorSwitch, progress);
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}