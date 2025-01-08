using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Graphics.Lights.Standalone
{
    public class AdvancedEmissiveLightStandalone : MonoBehaviour
    {
        [SerializeField] private string materialName;
        [SerializeField] private int materialIndex;
        [SerializeField] private MeshRenderer meshRenderer;

        [SerializeField] private PropertyValuePair<float>[] floatProperties;
        [SerializeField] private PropertyValuePair<int>[] intProperties;
        [SerializeField] private PropertyValuePair<Color>[] colorProperties;
        // [SerializeField] private PropertyValuePair<Vector2>[] vector2Properties;
        
        public string Name => name;
        private Material _material;
        
        
        public PropertyValuePair<float>[] FloatProperties => floatProperties;
        public PropertyValuePair<int>[] IntProperties => intProperties;
        public PropertyValuePair<Color>[] ColorProperties => colorProperties;
        

        private Coroutine _transitionCoroutine;

        private void Awake()
        {
            if (string.IsNullOrEmpty(materialName))
            {
                _material = meshRenderer.materials[materialIndex];
                return;
            }

            var mat = meshRenderer.materials.FirstOrDefault(m => m.name.StartsWith(materialName));

            if (mat == null)
            {
                Debug.Log($"{name} - Material named {materialName} not found!");
                _material = meshRenderer.materials[materialIndex];
                return;
            }
            _material = mat;
        }

        public void ApplyInstantEmission(float percentage = 1f)
        {
            StopTransition();
            
            foreach (var ints in intProperties)
            {
                _material.SetInt(ints.PropertyID, ints.Value);
            }
            foreach (var prop in floatProperties)
            {
                _material.SetFloat(prop.PropertyID, prop.Value * percentage);
            }
            foreach (var prop in colorProperties)
            {
                _material.SetColor(prop.PropertyID, prop.Value * percentage);
            }
        }

        public void ApplyAnimatedEmission(float duration, float percentage = 1f)
        {
            StopTransition();

            _transitionCoroutine = StartCoroutine(Transition(duration, percentage));
        }

        public void StopTransition()
        {
            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);
        }

        private IEnumerator Transition(float duration, float percentage)
        {
            float timer = 0f;
            
            var floatProps = floatProperties.Select(f => _material.GetFloat(f.PropertyID)).ToArray();
            var colorProps = colorProperties.Select(c => _material.GetColor(c.PropertyID)).ToArray();

            foreach (var ints in intProperties)
            {
                _material.SetInt(ints.PropertyID, ints.Value);
            }
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var progress = timer / duration;

                for (int i = 0; i < floatProperties.Length; i++)
                {
                    var prop = floatProperties[i];
                    _material.SetFloat(prop.PropertyID, Mathf.Lerp(floatProps[i], prop.Value * percentage, progress));
                }

                for (int i = 0; i < colorProperties.Length; i++)
                {
                    var prop = colorProperties[i];
                    _material.SetColor(prop.PropertyID, Color.Lerp(colorProps[i], prop.Value * percentage, progress));
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}