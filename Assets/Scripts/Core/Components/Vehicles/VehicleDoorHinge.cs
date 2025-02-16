using UnityEngine;
using Core.Entities.Vehicle;

namespace Core.Components.Vehicles
{
    public class VehicleDoorHinge : MonoBehaviour
    {
        [SerializeField] private string doorTag;
        [SerializeField] private Vector3 rotationAxis;
        [SerializeField] private Transform hingeAnchor;
        [SerializeField] private VehicleController vehicleController;

        [Header("Physics")]
        [SerializeField] private float mass;
        [SerializeField] private float damping = 5f;
        [SerializeField] private float forceFactor = 10f;
        
        [Header("Latches")]
        [SerializeField] private bool disableLatches;
        [SerializeField] private float[] latchPositions;
        [SerializeField] private float latchRange;
        [SerializeField] private float latchSpring = 15f;
        [SerializeField] private float latchDamping = 10f;
        [SerializeField] private float hingeOpenLimit;
        
        public bool CentralLockLocked { get; private set; }

        private int _targetLatch = -1;
        private bool _hingeLocked = true;
        
        private float _currentAngle;
        private float _currentVelocity;
        private Vector3 _initialRotation;
        
        private void Start()
        {
            if (!hingeAnchor)
                hingeAnchor = transform;
            if (!vehicleController)
                vehicleController = GetComponentInParent<VehicleController>();

            _initialRotation = hingeAnchor.localEulerAngles;
        }

        public bool SetCentralLock(bool value)
        {
            CentralLockLocked = value && _hingeLocked;
            return CentralLockLocked;
        }

        public void ToggleDoors()
        {
            if (CentralLockLocked) return;
            
            float delta;
            if (_targetLatch == latchPositions.Length - 1)
            {
                _targetLatch = -1;
                delta = -_currentAngle;
            }
            else
            {
                NextLatch();
                UnlockHinge();
                delta = latchPositions[_targetLatch] - _currentAngle;

                if (_targetLatch == 0)
                    delta *= 0.4f;
            }
            
            _currentVelocity += delta * forceFactor;
            Debug.Log($"AddForce: {delta * forceFactor}, delta: {delta}, a: {_currentAngle}, force: {forceFactor}");
        }
        
        private void NextLatch() =>
            _targetLatch = Mathf.Clamp(_targetLatch + 1, 0, latchPositions.Length - 1);
        private void PrevLatch() =>
            _targetLatch = Mathf.Clamp(_targetLatch - 1, 0, latchPositions.Length - 1);

        private void UnlockHinge()
        {
            _hingeLocked = false;
            vehicleController?.Status.SetState($"door_{doorTag}", "opened");
        }

        private void LockHinge()
        {
            _currentAngle = 0;
            _currentVelocity = 0;
            _hingeLocked = true;
            vehicleController?.Status.SetState($"door_{doorTag}", "closed");
        }

        private void LimitHinge()
        {
            if (_currentAngle < 0f)
                LockHinge();
            
            if (_currentAngle < hingeOpenLimit) return;

            _currentVelocity = -Mathf.Abs(_currentVelocity * 0.8f);
        }

        private void FixedUpdate()
        {
            if (_hingeLocked) return;
            UpdateLatch();
            UpdateVelocity();
            LimitHinge();
            UpdateRotation();
        }

        private void UpdateLatch()
        {
            foreach (var latch in latchPositions)
            {
                var delta = latch - _currentAngle;
                if (Mathf.Abs(delta) > latchRange) continue;
                
                _currentVelocity -= _currentVelocity * latchDamping * Time.fixedDeltaTime;
                _currentVelocity += delta * latchSpring * Time.fixedDeltaTime;
            }
        }

        private void UpdateVelocity()
        {
            _currentVelocity -= _currentVelocity * damping * Time.fixedDeltaTime;
        }

        private void UpdateRotation()
        {
            _currentAngle += _currentVelocity * Time.fixedDeltaTime;
            hingeAnchor.localEulerAngles = _initialRotation + _currentAngle * rotationAxis.normalized;
        }
    }
}