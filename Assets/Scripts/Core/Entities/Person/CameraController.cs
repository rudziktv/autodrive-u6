using System;
using Core.Utils;
using Core.Utils.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Person
{
    public class CameraController : MonoBehaviour
    {
        public static Camera Main { get; private set; }
        public static CameraController Instance { get; private set; }
        
        [SerializeField] private float mouseSensitivity = 0.015f;
        [SerializeField] private Camera playerCamera;

        [SerializeField] private Transform cameraYaw;
        [SerializeField] private Transform cameraPitch;

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

        public bool PanningLocked { get; private set; }

        private Transform _focusTarget;
        private Quaternion _beforeFocusRotation;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            Instance = this;
        }

        private void Start()
        {
            if (playerCamera == null)
                playerCamera = Camera.main;
            if (cameraYaw == null)
                cameraYaw = GlobalUtils.GetCameraYaw().transform;
            if (cameraPitch == null)
                cameraPitch = GlobalUtils.GetCameraPitch().transform;
            
            Main = playerCamera;
            _rotX = transform.localEulerAngles.x;
            _targetFov = fov;

            InitializeInput();
        }

        private void InitializeInput()
        {
            InputSystem.actions.FindAction(GlobalInputs.TOGGLE_CURSOR)
                .performed += ToggleCursor;
            InputSystem.actions.FindAction(GlobalInputs.CAMERA).performed += OnMouseMove;

            this.GetInputAction(GlobalInputs.ZOOM_IN).performed += ZoomIn;
            this.GetInputAction(GlobalInputs.ZOOM_OUT).performed += ZoomOut;
            this.GetInputAction(GlobalInputs.TOGGLE_ZOOM).performed += ToggleZoom;
            this.GetInputAction(GlobalInputs.RESET_ZOOM).performed += ResetZoom;
        }

        public void FocusOn(Transform target)
        {
            _focusTarget = target;
        }

        public void Unfocus()
        {
            _focusTarget = null;
            Main.transform.localRotation = Quaternion.identity;
        }

        public void ResetAndLockView()
        {
            PanningLocked = true;
            cameraYaw.localEulerAngles = Vector3.zero;
            cameraPitch.localEulerAngles = Vector3.zero;
        }
        
        public void UnlockView() =>
            PanningLocked = false;
        
        public void OnMouseMove(InputAction.CallbackContext ctx)
        {
            _mouseDelta += ctx.ReadValue<Vector2>();
        }

        public void ToggleCursor(InputAction.CallbackContext ctx)
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

        public void ResetZoom(InputAction.CallbackContext ctx)
        {
            _targetFov = fov;
        }

        public void ToggleZoom(InputAction.CallbackContext ctx)
        {
            _isZooming = !_isZooming;
            _targetFov = _isZooming ? zoomFov : fov;
        }

        private void Update()
        {
            UpdateRotation();
            UpdateFocus();
            UpdateZoom();
        }

        private void UpdateRotation()
        {
            if (_mouseDelta == Vector2.zero || Cursor.lockState != CursorLockMode.Locked ||
                PanningLocked)
            {
                _mouseDelta = Vector2.zero;
                return;
            }
            
            // var rot = cameraTransform.localEulerAngles;
            var rotCamYaw = cameraYaw.localEulerAngles;
            var rotCamPitch = cameraPitch.localEulerAngles;
            // transform.Rotate(Vector3.up, _mouseDelta.x * mouseSensitivity * Time.deltaTime);
            // transform.Rotate(Vector3.left, _mouseDelta.y * mouseSensitivity * Time.deltaTime);
            
            _rotX -= _mouseDelta.y * mouseSensitivity * Time.deltaTime;
            rotCamYaw.y += _mouseDelta.x * mouseSensitivity * Time.deltaTime;
            
            _rotX = Mathf.Clamp(_rotX, -90f, 90f);
            rotCamPitch.x = _rotX;
            // cameraTransform.eulerAngles = rot;
            cameraYaw.localEulerAngles = rotCamYaw;
            cameraPitch.localEulerAngles = rotCamPitch;

            // Reset delta (opcjonalnie)
            _mouseDelta = Vector2.zero;
        }

        private void UpdateFocus()
        {
            if (!_focusTarget) return;

            // var currentRot = Main.transform.localRotation;
            var dir = _focusTarget.position - Main.transform.position;
            var rot = Quaternion.LookRotation(dir);
            Main.transform.rotation = Quaternion.Lerp(Main.transform.rotation, rot, 0.2f);
            // Main.transform.LookAt(_focusTarget);

        }

        private void UpdateZoom()
        {
            if (_zoomInOut != 0)
            {
                playerCamera.fieldOfView += _zoomInOut * Time.deltaTime * zoomSpeed;
                playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minFov, maxFov);
                _targetFov = playerCamera.fieldOfView;
                return;
            }
            
            _targetFov = Mathf.Clamp(_targetFov, minFov, maxFov);
            var deltaZoom = _targetFov - playerCamera.fieldOfView;
            deltaZoom = Mathf.Clamp(deltaZoom, -zoomSpeed * Time.deltaTime, zoomSpeed * Time.deltaTime);
            playerCamera.fieldOfView += deltaZoom;
            // playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minFov, maxFov);
        }
    }
}