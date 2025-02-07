using System;
using UnityEngine;

namespace Core.Components.CustomPhysics
{
    public class MyRigidbody : MonoBehaviour
    {
        public float mass;
        public Vector3 boxSize;
        public Vector3 centerOfMass;
        
        private Vector3 _velocity;
        private Vector3 _angularVelocity;

        private Vector3 _inertiaTensor;
        private Matrix4x4 InverseInertiaTensor;
        
        private Vector3 WorldCenterOfMass => transform.TransformPoint(centerOfMass);
        // Matrix4x4 worldInertiaTensor => transform.rotation * InverseInertiaTensor * Quaternion.Inverse(transform.rotation);

        
        public void AddForceAtPosition(Vector3 force, Vector3 point)
        {
            UpdateTensor();
            Vector3 r = point - WorldCenterOfMass;
            _velocity += force / mass;
            var torque = Vector3.Cross(r, force);
            _angularVelocity += InverseInertiaTensor.MultiplyVector(torque);
        }
        
        public Vector3 GetPointVelocity(Vector3 point)
        {
            Vector3 r = point - WorldCenterOfMass;
            return _velocity + Vector3.Cross(_angularVelocity, r);
        }

        
        private void FixedUpdate()
        {
            UpdateTensor();
            UpdatePhysics();
        }

        private void UpdateTensor()
        {
            _inertiaTensor = GetInertiaTensorBox(boxSize, mass);
            // InverseInertiaTensor = GetInertiaTensorMatrix(boxSize, mass).inverse;
            InverseInertiaTensor = Matrix4x4.identity;
            InverseInertiaTensor[0, 0] = 1f / _inertiaTensor.x;
            InverseInertiaTensor[1, 1] = 1f / _inertiaTensor.y;
            InverseInertiaTensor[2, 2] = 1f / _inertiaTensor.z;

        }

        private void UpdatePhysics()
        {
            transform.position += _velocity * Time.fixedDeltaTime;
            
            if (_angularVelocity.sqrMagnitude > 0.0001f)  // Unikaj dzielenia przez zero
            {
                Quaternion deltaRotation = Quaternion.AngleAxis(
                    _angularVelocity.magnitude * Mathf.Rad2Deg * Time.fixedDeltaTime,
                    _angularVelocity.normalized
                );
                transform.rotation = deltaRotation * transform.rotation;
            }
            // Quaternion deltaRotation = Quaternion.AngleAxis(_angularVelocity.magnitude * Mathf.Rad2Deg * Time.fixedDeltaTime, _angularVelocity.normalized);
            // transform.rotation = deltaRotation * transform.rotation;
        }
        
        public static Matrix4x4 GetInertiaTensorMatrix(Vector3 size, float mass)
        {
            float factor = (1f / 12f) * mass;
            return Matrix4x4.Scale(new Vector3(
                factor * (size.y * size.y + size.z * size.z),
                factor * (size.x * size.x + size.z * size.z),
                factor * (size.x * size.x + size.y * size.y)
            ));
        }
        
        public static Vector3 GetInertiaTensorBox(Vector3 size, float mass)
        {
            float factor = (1f / 12f) * mass;
            return new Vector3(
                factor * (size.y * size.y + size.z * size.z),
                factor * (size.x * size.x + size.z * size.z),
                factor * (size.x * size.x + size.y * size.y)
            );
        }
    }
}