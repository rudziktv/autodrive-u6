using System;
using Core.Entities.Vehicle;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.Components.Vehicles.Mirror
{
    public class VehicleExteriorMirror : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        
        [Header("Folding")]
        [SerializeField] private bool foldByDefault;
        [SerializeField] private float foldAngle;
        [SerializeField] private Vector3 mirrorFoldAxis;
        [SerializeField] private Transform mirrorFoldAnchor;
        [SerializeField] private VehicleMirrorFoldType mirrorFoldType = VehicleMirrorFoldType.Manual;

        [Header("Manual Folding")]
        [SerializeField] private float spring;
        [SerializeField] private float damper;
        
        [Header("Electric Folding")]
        [SerializeField] private float motorVelocity;
        
        [Header("Events")]
        public UnityEvent manuallyFolded;
        public UnityEvent manuallyUnfolded;
        public UnityEvent electricFoldStarted;
        public UnityEvent electricFoldEnded;

        public bool Fold { get; private set; }
        private float TargetAngle => Fold ? foldAngle : 0f;
        
        private float _currentVelocity;
        private float _currentFoldAngle;
        private Vector3 _initialRotation;
        
        private void Start()
        {
            if (!mirrorFoldAnchor)
                mirrorFoldAnchor = transform;
            if (!vehicleController)
                vehicleController = GetComponentInParent<VehicleController>();
            
            _initialRotation = mirrorFoldAnchor.localEulerAngles;
            Fold = foldByDefault;
        }

        public void ToggleManualFold()
            => SetManualFold(!Fold);

        public void SetManualFold(bool fold)
        {
            Fold = fold;
            if (Fold)
                manuallyFolded?.Invoke();
            else
                manuallyUnfolded?.Invoke();
        }

        private void FixedUpdate()
        {
            switch (mirrorFoldType)
            {
                case VehicleMirrorFoldType.Manual:
                    UpdateManualFolding();
                    break;
                case VehicleMirrorFoldType.Electric:
                    UpdateElectricFolding();
                    break;
            }
        }

        private void UpdateManualFolding()
        {
            if (Mathf.Approximately(TargetAngle, _currentFoldAngle)) return;
            var offset = TargetAngle - _currentFoldAngle;
            
            _currentVelocity += offset * spring * Time.fixedDeltaTime;
            _currentVelocity -= _currentVelocity * damper * Time.fixedDeltaTime;
            
            UpdateRotation();
        }

        private void UpdateElectricFolding()
        {
            if (Mathf.Approximately(TargetAngle, _currentFoldAngle)) return;
            if (_currentVelocity == 0) electricFoldStarted?.Invoke();
            
             var dir = Mathf.Sign(TargetAngle - _currentFoldAngle);
             _currentVelocity = motorVelocity * dir;

             var frameAngle = _currentFoldAngle + _currentVelocity * Time.fixedDeltaTime;
             if (frameAngle <= 0 || frameAngle >= foldAngle)
                 electricFoldEnded?.Invoke();
        }

        private void UpdateRotation()
        {
            LimitFoldAngle();
            _currentFoldAngle += _currentVelocity * Time.fixedDeltaTime;
            mirrorFoldAnchor.localEulerAngles = _initialRotation + _currentFoldAngle * mirrorFoldAxis;
        }

        private void LimitFoldAngle()
        {
            if (_currentFoldAngle >= 0 && _currentFoldAngle <= foldAngle) return;
            _currentVelocity = 0;
            _currentFoldAngle = TargetAngle;
        }
    }
}