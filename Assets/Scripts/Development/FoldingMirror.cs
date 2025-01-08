using System;
using UnityEngine;

namespace Development
{
    public class FoldingMirror : MonoBehaviour
    {
        [SerializeField] private HingeJoint joint;
        /// <summary>
        /// The target velocity of the motor. Use signed values to receive folding direction.
        /// </summary>
        [Tooltip("The target velocity of the motor. Use signed values to receive folding direction.")]
        [SerializeField] private float targetVelocity;
        
        [SerializeField] private float targetAngle;
        [SerializeField] private bool foldedByDefault;


        private bool _folded;

        private void Start()
        {
            _folded = foldedByDefault;
            UpdateMirrors();
        }

        public void ToggleFold()
        {
            _folded = !_folded;
            UpdateMirrors();
        }

        public void FoldMirrors()
        {
            _folded = true;
            UpdateMirrors();
        }

        public void UnfoldMirrors()
        {
            _folded = false;
            UpdateMirrors();
        }

        private void UpdateMirrors()
        {
            var spring = joint.spring;
            spring.targetPosition = _folded ? targetAngle : 0;
            joint.spring = spring;

            var motor = joint.motor;
            motor.targetVelocity = _folded ? targetVelocity : -targetVelocity;
            joint.motor = motor;
        }
    }
}