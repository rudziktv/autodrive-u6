using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Components.Vehicles
{
    public class SimpleInteriorCameraController : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 0.015f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Camera playerCamera;

        [Header("Zoom")]
        [SerializeField] private float fov = 60f;
        [SerializeField] private float zoomFov = 20f;
        [SerializeField] private float zoomSpeed = 50f;
        [SerializeField] private float minFov = 5f;
        [SerializeField] private float maxFov = 90f;

        private Vector2 _mouseDelta;
        private float _rotX = 0f;

        private float _targetFov;
        private bool _isZooming;

        private int _zoomInOut;

        public void OnMouseMove(InputAction.CallbackContext ctx)
        {
            _mouseDelta += ctx.ReadValue<Vector2>();
        }

        public void ToggleCursor()
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            // Cursor.visible = !Cursor.visible;
        }

        public void ZoomIn(InputAction.CallbackContext ctx)
        {
            if (_isZooming) return;
            if (_zoomInOut == 1 || ctx.canceled)
            {
                _zoomInOut = 0;
                return;
            }
            _targetFov = playerCamera.fieldOfView;
            _zoomInOut = -1;
        }

        public void ZoomOut(InputAction.CallbackContext ctx)
        {
            if (_isZooming) return;
            if (_zoomInOut == -1 || ctx.canceled)
            {
                _zoomInOut = 0;
                return;
            }
            _targetFov = playerCamera.fieldOfView;
            _zoomInOut = 1;
        }

        public void ResetZoom()
        {
            _targetFov = fov;
        }

        public void ToggleZoom()
        {
            _isZooming = !_isZooming;
            _targetFov = _isZooming ? zoomFov : fov;
        }

        private void Start()
        {
            _rotX = transform.localEulerAngles.x;
            _targetFov = fov;
        }

        private void Update()
        {
            UpdateRotation();
            UpdateZoom();
        }

        private void UpdateRotation()
        {
            if (_mouseDelta == Vector2.zero || Cursor.lockState != CursorLockMode.Locked)
            {
                _mouseDelta = Vector2.zero;
                return;
            }
            
            var rot = cameraTransform.localEulerAngles;
            // transform.Rotate(Vector3.up, _mouseDelta.x * mouseSensitivity * Time.deltaTime);
            // transform.Rotate(Vector3.left, _mouseDelta.y * mouseSensitivity * Time.deltaTime);
            
            _rotX += _mouseDelta.y * -mouseSensitivity * Time.deltaTime;
            rot.y += _mouseDelta.x * mouseSensitivity * Time.deltaTime;
            
            _rotX = Mathf.Clamp(_rotX, -90f, 90f);
            rot.x = _rotX;
            cameraTransform.eulerAngles = rot;

            // Reset delta (opcjonalnie)
            _mouseDelta = Vector2.zero;
        }

        private void UpdateZoom()
        {
            if (_zoomInOut != 0)
            {
                playerCamera.fieldOfView += _zoomInOut * Time.deltaTime * zoomSpeed;
                _targetFov = playerCamera.fieldOfView;
                return;
            }
            
            var deltaZoom = _targetFov - playerCamera.fieldOfView;
            deltaZoom = Mathf.Clamp(deltaZoom, -zoomSpeed * Time.deltaTime, zoomSpeed * Time.deltaTime);
            playerCamera.fieldOfView += deltaZoom;
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minFov, maxFov);
        }
    }
}