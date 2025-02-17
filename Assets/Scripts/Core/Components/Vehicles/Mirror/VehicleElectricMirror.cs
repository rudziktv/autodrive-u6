using System;
using UnityEngine;

namespace Core.Components.Vehicles.Mirror
{
    public class VehicleElectricMirror : MonoBehaviour
    {
        [SerializeField] private Transform mirrorAnchor;
        [SerializeField] private float motorVelocity = 1f;
        
        private Vector3 _initialRotation;
        private Vector3 _currentRotation;

        private Vector3 _currentMotorVelocity;
        
        public Transform MirrorAnchor => mirrorAnchor;

        private void Start()
        {
            if (!mirrorAnchor)
                mirrorAnchor = transform;
            _initialRotation = mirrorAnchor.localEulerAngles;
        }

        public void SetMotorVelocity(Vector2 input)
        {
            input.Normalize();
            _currentMotorVelocity.x = Mathf.Clamp(input.y, -1, 1) * motorVelocity;
            _currentMotorVelocity.z = -Mathf.Clamp(input.x, -1, 1) * motorVelocity;
        }

        private void FixedUpdate()
        {
            if (_currentMotorVelocity == Vector3.zero)
                return;
            
            _currentRotation += _currentMotorVelocity * Time.fixedDeltaTime;
            mirrorAnchor.localEulerAngles = _initialRotation + _currentRotation;
        }
    }
}