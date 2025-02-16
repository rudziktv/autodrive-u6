using System;
using System.Collections;
using Core.Utils;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions.Mirror
{
    public class CentralMirrorInteractable : Interactable
    {
        [SerializeField] private Transform mirrorAnchor;
        [SerializeField] private float sensitivity = 0.1f;
        
        private Coroutine _coroutine;

        private Vector3 _rotation;
        private Vector3 _initialRotation;

        private void Start()
        {
            _initialRotation = mirrorAnchor.localEulerAngles;
        }

        public override void SimpleInteract()
        {
            base.SimpleInteract();
            Debug.Log("Central mirror interact");
        }

        public override void InteractionStarted()
        {
            base.InteractionStarted();
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(InteractionUpdate());
            // Debug.Log("Central mirror interaction started");
        }

        public override void InteractionEnded()
        {
            base.InteractionEnded();
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            // Debug.Log("Central mirror interaction ended");
        }

        private IEnumerator InteractionUpdate()
        {
            while (true)
            {
                // Debug.Log($"InteractionUpdate, interaction-cursor: {input.x}, {input.y}");
                var input = InputSystem.actions.FindAction(GlobalInputs.INTERACTION_CURSOR).ReadValue<Vector2>();
                _rotation.x += input.y * sensitivity;
                _rotation.z -= input.x * sensitivity;
                
                mirrorAnchor.localEulerAngles = _initialRotation + _rotation;
                yield return null;
            }
        }
    }
}