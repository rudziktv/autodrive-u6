using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Graphics.Lights
{
    [Serializable]
    public class AdvancedEmissiveLight
    {
        [SerializeField] private string name;
        [SerializeField] private MeshRenderer meshRenderer;
        // [SerializeField] private float emissionIntensity;
        // [SerializeField] private Color emissionColor;
        [SerializeField] private int materialIndex;

        [SerializeField] private PropertyValuePair<float>[] floatProperties;
        [SerializeField] private PropertyValuePair<int>[] intProperties;
        [SerializeField] private PropertyValuePair<Color>[] colorProperties;
        // [SerializeField] private PropertyValuePair<Vector2>[] vector2Properties;
        
        public string Name => name;
        private Material LightMaterial => meshRenderer.materials[materialIndex];

        public AdvancedEmissiveLight() { }

        public AdvancedEmissiveLight(MeshRenderer meshRenderer, float emissionIntensity, Color color, int materialIndex)
        {
            // emissionColor = color;
            this.meshRenderer = meshRenderer;
            // this.emissionIntensity = emissionIntensity;
            this.materialIndex = materialIndex;
        }
        
        public PropertyValuePair<float>[] FloatProperties => floatProperties;
        public PropertyValuePair<int>[] IntProperties => intProperties;
        public PropertyValuePair<Color>[] ColorProperties => colorProperties;
        //
        // public MeshRenderer MeshRenderer => meshRenderer;
        // public float EmissionIntensity => emissionIntensity;
        // public Color EmissionColor => emissionColor;
        // public int MaterialIndex => materialIndex;

        private Coroutine _transitionCoroutine;

        public void ApplyInstantEmission(float percentage = 1f)
        {
            foreach (var ints in intProperties)
            {
                LightMaterial.SetInt(ints.PropertyID, ints.Value);
            }
            foreach (var prop in floatProperties)
            {
                LightMaterial.SetFloat(prop.PropertyID, prop.Value * percentage);
            }
            foreach (var prop in colorProperties)
            {
                LightMaterial.SetColor(prop.PropertyID, prop.Value * percentage);
            }
        }

        public void ApplyAnimatedEmission(MonoBehaviour monoBehaviour, float duration, float percentage = 1f)
        {
            if (_transitionCoroutine != null)
                monoBehaviour.StopCoroutine(_transitionCoroutine);

            _transitionCoroutine = monoBehaviour.StartCoroutine(Transition(duration, percentage));
        }

        private IEnumerator Transition(float duration, float percentage)
        {
            float timer = 0f;
            
            var floatProps = floatProperties.Select(f => LightMaterial.GetFloat(f.PropertyID)).ToArray();
            var colorProps = colorProperties.Select(c => LightMaterial.GetColor(c.PropertyID)).ToArray();

            foreach (var ints in intProperties)
            {
                LightMaterial.SetInt(ints.PropertyID, ints.Value);
            }
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;

                for (int i = 0; i < floatProperties.Length; i++)
                {
                    var prop = floatProperties[i];
                    LightMaterial.SetFloat(prop.PropertyID, Mathf.Lerp(floatProps[i], prop.Value * percentage, progress));
                }

                for (int i = 0; i < colorProperties.Length; i++)
                {
                    var prop = colorProperties[i];
                    LightMaterial.SetColor(prop.PropertyID, Color.Lerp(colorProps[i], prop.Value * percentage, progress));
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public float? GetFloatProperty(string name)
        {
            return floatProperties.FirstOrDefault(f => f.PropertyName == name)?.Value;
        }
    }
}