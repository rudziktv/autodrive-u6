using System.Collections;
using System.Linq;
using UnityEngine;

namespace Core.Components.Lights.Standalone
{
    public class SimpleEmissiveLightStandalone : EmissiveLightStandalone
    {
        private static readonly int EmissiveColorID = Shader.PropertyToID("_EmissiveColor");
        
        [SerializeField] private string materialName;
        [SerializeField] private int materialIndex;
        [SerializeField] private MeshRenderer meshRenderer;
        
        [SerializeField] private Color emissiveColor;
        [SerializeField] private float emissiveIntensity;
        
        private Material _material;
        private Coroutine _transitionCoroutine;
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(materialName))
            {
                _material = meshRenderer.materials[materialIndex];
                return;
            }

            var mat = meshRenderer.materials.FirstOrDefault(m => m.name.StartsWith(materialName));

            if (!mat)
            {
                Debug.Log($"{name} - Material named {materialName} not found!");
                _material = meshRenderer.materials[materialIndex];
                return;
            }
            _material = mat;
        }
        
        public override void ApplyInstantEmission(float percentage = 1f)
        {
            StopTransition();
            _material.SetColor(EmissiveColorID, emissiveColor * emissiveIntensity * percentage);
        }
        
        public override void ApplyAnimatedEmission(float duration, float percentage = 1f)
        {
            StopTransition();
            _transitionCoroutine = StartCoroutine(Transition(duration, percentage));
        }

        public override void StopTransition()
        {
            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);
        }

        private IEnumerator Transition(float duration, float percentage)
        {
            var timer = 0f;
            // Color before transition.
            var oldColor = _material.GetColor(EmissiveColorID);
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var progress = timer / duration;
                _material.SetColor(EmissiveColorID, Color.Lerp(oldColor, emissiveColor * emissiveIntensity * percentage, progress));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}