using System;
using Core.Components.CustomPhysics;
using UnityEngine;

namespace Core.Components.Vehicles
{
    public class TireComponent : MonoBehaviour
    {
        [Header("Tire")]
        [SerializeField] private float radius;
        [SerializeField] private float mass;
        [SerializeField] private Transform tire;
        [SerializeField] private float forwardFrictionFactor = 1.2f;
        
        [Header("Suspension")]
        [SerializeField] private float spring;
        [SerializeField] private float damping;
        [SerializeField] private float height;
        [SerializeField] private float restDistance;
        [Range(0, 1)] [SerializeField] private float target = 0.5f;
        
        [Header("Physics")]
        [SerializeField] private LayerMask physicsLayers;

        private Rigidbody _rb;
        // private MyRigidbody _mRb;
        private float _currentTraction;

        private float SuspensionRestDistance => height * target;

        public float MotorTorque { get; set; }
        public float BrakeTorque { get; set; }
        public float SteeringAngle { get; set; }
        public float RadS { get; private set; }
        public float RPM { get; private set; }
        
        public float Circumference => Mathf.PI * 2f * radius;

        private void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            // _mRb = GetComponentInParent<MyRigidbody>();
        }

        private void FixedUpdate()
        {
            // try to stabilize
            // _rb.angularVelocity = Vector3.zero;
            // _rb.linearVelocity = Vector3.zero;

            // _rb.solverIterations = 30;
            // _rb.solverVelocityIterations = 30;
            
            UpdateSuspension();
            // UpdateSteering();
            // UpdateTraction();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * height);
            // Gizmos.Draw
            Gizmos.DrawWireSphere(transform.position - transform.up * SuspensionRestDistance, radius);
        }


        private void UpdateSuspension()
        {
            var ray = new Ray(transform.position, -transform.up);
            var maxDistance = height + radius; // Height of the suspension + radius of the tire.
            if (!Physics.Raycast(ray, out var hit, maxDistance, physicsLayers)) // If tire doesn't have contact with ground, put it on the lowest.
            {
                tire.position = transform.position - transform.up * height;
                _currentTraction = 0;
                return;
            }

            Vector3 springDir = transform.up;
            Vector3 tireWorldVel = _rb.GetPointVelocity(transform.position);

            float offset = restDistance - (hit.distance - radius);
            float vel = Vector3.Dot(springDir, tireWorldVel);
            float force = (offset * spring) - (vel * damping);
            
            // Debug.Log($"Force: {force}, Spring: {offset * spring}, Damping: {vel * damping}");

            _rb.AddForceAtPosition(springDir * force, transform.position);
            
            tire.position = transform.position - transform.up * (hit.distance - radius);
        }

        private void UpdateSteering()
        {
            var tireGripFactor = 1f;
            
            Vector3 steeringDir = transform.right;
            Vector3 tireWorldVel = _rb.GetPointVelocity(transform.position);
            
            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
            float desiredVelChange = -steeringVel * tireGripFactor;
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
            
            _rb.AddForceAtPosition(steeringDir * (mass * desiredAccel), transform.position);
        }

        private void UpdateTraction()
        {
            Vector3 tractionDir = transform.forward;
            Vector3 tireWorldVel = _rb.GetPointVelocity(transform.position);
            
            float tractionVel = Vector3.Dot(tractionDir, tireWorldVel);
            float desiredVelChange = -tractionVel;
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            // var kineticEnergy = _rb.mass * Mathf.Pow(tractionVel, 2) / 2f;

            RadS = tractionVel / radius;
            RPM = tractionVel / Circumference * 60f;

            // if (tractionVel < 0.05f && MotorTorque == 0f)
            //     _rb.AddForceAtPosition(tractionDir * (mass * desiredAccel), transform.position);
            // else
            _rb.AddForceAtPosition(tractionDir * MotorTorque / radius, transform.position);
        }
            //
            // Debug.Log($"Dot Velocity: {tractionVel}, Transform vel: {transform.InverseTransformDirection(tireWorldVel).z}, vehicle vel: {_rb.linearVelocity.z}");
            //
            // var transformVel = transform.InverseTransformDirection(tireWorldVel);

        private void UpdateWheelVisuals()
        {
            
        }
    }
}