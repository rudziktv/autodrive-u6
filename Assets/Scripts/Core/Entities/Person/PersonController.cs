using System;
using Core.Utils;
using Core.Utils.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Person
{
    public class PersonController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform cameraYaw;
        [SerializeField] private float moveForce;
        [SerializeField] private float moveDrag;
        [SerializeField] private float drag;
        [SerializeField] private float dragThreshold = 0.01f;

        private Vector2 _movementInput;

        private void Start()
        {
            if (cameraYaw == null)
                cameraYaw = GlobalUtils.GetCameraYaw().transform;

            InputSystem.actions.FindAction(GlobalInputs.MOVEMENT).performed +=
                OnMovementInput;
            this.GetInputAction(GlobalInputs.MOVEMENT).canceled += OnMovementInput;
        }

        public void OnMovementInput(InputAction.CallbackContext ctx)
        {
            _movementInput = ctx.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var direction = new Vector3(_movementInput.normalized.x, 0, _movementInput.normalized.y);
            var moveVector = cameraYaw.TransformDirection(direction);
            var v = _movementInput.magnitude * moveForce * moveVector;
            var d = Mathf.Lerp(drag, moveDrag, _movementInput.magnitude / dragThreshold);

            rb.linearDamping = d;
            rb.AddForce(v);
        }
    }
}