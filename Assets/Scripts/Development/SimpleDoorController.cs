using System;
using Entities.Vehicle;
using UnityEngine;

namespace Development
{
    public class SimpleDoorController : MonoBehaviour
    {
        [SerializeField] private HingeJoint doorJoint;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float doorOpenTarget;
        [SerializeField] private float doorCloseTarget;
        [SerializeField] private float hingeLockThreshold;
        [SerializeField] private float hingeLockAngle;
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private string doorTag;
        
        [SerializeField] private float[] doorStepAngles;

        private bool _opened;
        private int _step;
        private bool _lockHinge;
        private Transform _transform;
        private JointLimits _limits;

        private void FixedUpdate()
        {
            if (_lockHinge || _step > 0 ||
                (doorJoint.angle > hingeLockThreshold && !float.IsNaN(doorJoint.angle))) return;
            LockHinge();
        }

        public void ToggleDoor()
        {
            _step++;
            if (_step >= doorStepAngles.Length)
                _step = 0;

            UpdateDoor();
        }

        private void UpdateDoor()
        {
            var spring = doorJoint.spring;
            // _lockHinge = _opened ? false : _lockHinge;
            if (_lockHinge && _step > 0)
                UnlockHinge();
            spring.targetPosition = doorStepAngles[_step];
            doorJoint.spring = spring;
        }

        private void LockHinge()
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            var lim = _limits;
            lim.max = hingeLockAngle;
            doorJoint.limits = lim;
            _lockHinge = true;
            vehicleController.CodingVariables.SetState($"door_{doorTag}", "closed");
        }

        private void UnlockHinge()
        {
            rb.constraints = RigidbodyConstraints.None;
            doorJoint.limits = _limits;
            _lockHinge = false;
            vehicleController.CodingVariables.SetState($"door_{doorTag}", "opened");
        }

        private void Start()
        {
            _limits = doorJoint.limits;
            _transform = transform;
            UpdateDoor();
        }
    }
}