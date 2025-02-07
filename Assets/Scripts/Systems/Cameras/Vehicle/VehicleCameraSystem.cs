using System;
using Core.Entities.Vehicle;
using Core.Utils.Extensions;
using Systems.Cameras.Vehicle.Views;
using Systems.Gamemodes.VehicleMode;
using Systems.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Cameras.Vehicle
{
    public class VehicleCameraSystem : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private Transform driverPosition;
        [SerializeField] private Transform entryPos;
        [SerializeField] private Animator animator;
        [SerializeField] private VehicleController vehicle;

        // [SerializeField] private CameraView[] views;
        [Header("Views")]
        [SerializeField] private CameraView driverView;
        [SerializeField] private CameraView exteriorView;

        private VehicleInputActions Input => vehicle.VehicleInput;
        public Transform Anchor => anchor;

        private CameraView _currentView;

        private void Start()
        {
            if (animator == null)
                animator = GetComponentInParent<Animator>();
            if (vehicle == null)
                vehicle = GetComponentInParent<VehicleController>();
            
        }

        public void EnterVehicleAsDriver()
        {
            if (_currentView != null) return;
            AssignInputs();
            GameManager.ChangeGamemode(new VehicleGamemode(vehicle));
            SetDriverView();
            
            // anchor.position = Camera.main ? Camera.main.transform.position : anchor.position;
            // anchor.position = entryPos.position;
            // animator.SetTrigger("Driver Vehicle Enter");
        }

        private void AssignInputs()
        {
            Debug.Log("Assigning input to vehicle");
            Input.Functions.Ignition.performed += context =>
            {
                Debug.Log("Ignition performed");
            };
            Input.View.DriverView.performed += OnDriverViewButton;
            Input.View.ExteriorVehicleView.performed += OnExteriorViewButton;
        }

        private void OnDriverViewButton(InputAction.CallbackContext obj)
        {
            Debug.Log("DriverViewButton");
            SetDriverView();
        }

        private void OnExteriorViewButton(InputAction.CallbackContext obj)
        {
            
            Debug.Log("ExteriorViewButton");
            SetExteriorView();
        }

        private void Update()
        {
            _currentView?.UpdateView();
        }

        private void SetDriverView()
            => ChangeView(driverView);

        private void SetExteriorView()
            => ChangeView(exteriorView);

        private void ChangeView(CameraView view)
        {
            Debug.Log($"OldView: {_currentView}, NewView: {view}");
            _currentView?.DetachCamera();
            _currentView = view;
            _currentView.AttachCamera();
        }
    }
}