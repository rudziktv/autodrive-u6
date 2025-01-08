using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Vehicle.Subentities.Dashboard
{
    public class GaugeController : MonoBehaviour
    {
        [SerializeField] private float minScale = 0f;
        [SerializeField] private float maxScale;
        [SerializeField] private Animator animator;
        [SerializeField] private string animatorName;
        [SerializeField] private float smoothing = 0.05f;
        [SerializeField] [Range(0f, 1f)] private float valueControlled = 0f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public float Value { get; set; }
        private float target => valueControlled;
        
        private void Update()
        {
            // var target = Mathf.Clamp01((Value - minScale) / (maxScale - minScale));
            var current = animator.GetFloat(animatorName);
            var animatedValue = Mathf.Lerp(current, target, Time.deltaTime / smoothing);
            animator.SetFloat(animatorName, curve.Evaluate(animatedValue));
        }
    }
}