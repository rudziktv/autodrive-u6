using System;
using Core.Entities.Person;
using Core.Utils;
using Core.Utils.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Systems.Cameras.Vehicle.Views
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Transform anchor;

        [Header("Secondary Anchor")]
        [SerializeField] private bool lockViewRotation;
        [SerializeField] private bool rotateAroundSecondAnchor;
        [SerializeField] private Transform secondAnchor;
        
        private float mouseSensitivity = 5f;

        private Vector2 _mouseDelta;

        private float _rotX;
        
        private bool _isActive;

        public virtual void AttachCamera()
        {
            _isActive = true;
            anchor.AttachCameraRigSetLocalPos(Vector3.zero);
            // cameraRig.SetParent(anchor, false);
            if (lockViewRotation)
                CameraController.Instance.ResetAndLockView();
            else
                CameraController.Instance.UnlockView();

            if (rotateAroundSecondAnchor)
            {
                AssignInputs();
                _rotX = secondAnchor.localEulerAngles.x;
            }
        }

        private void AssignInputs()
        {
            this.GetInputAction(GlobalInputs.CAMERA).performed += OnViewRotate;
        }

        private void UnassignInputs()
        {
            this.GetInputAction(GlobalInputs.CAMERA).performed -= OnViewRotate;
        }

        public virtual void DetachCamera()
        {
            _isActive = false;
            if (rotateAroundSecondAnchor)
                UnassignInputs();
        }
        
        private void OnViewRotate(InputAction.CallbackContext ctx)
        {
            Debug.Log("OnViewRotate called");
            _mouseDelta += ctx.ReadValue<Vector2>();
        }

        public virtual void UpdateView()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (_mouseDelta == Vector2.zero  || Cursor.lockState != CursorLockMode.Locked ||
                !_isActive || !rotateAroundSecondAnchor)
            {
                _mouseDelta = Vector2.zero;
                return;
            }

            var rotAnchor = secondAnchor.localEulerAngles;
            
            _rotX -= _mouseDelta.y * mouseSensitivity * Time.deltaTime;
            rotAnchor.y += _mouseDelta.x * mouseSensitivity * Time.deltaTime;
            
            _rotX = Mathf.Clamp(_rotX, -90f, 90f);
            rotAnchor.x = _rotX;
            
            secondAnchor.localEulerAngles = rotAnchor;
            
            _mouseDelta = Vector2.zero;
        }
    }
}