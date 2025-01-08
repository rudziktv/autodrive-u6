using System;
using System.Collections;
using Entities.Vehicle.Subentities.Dashboard;
using Entities.Vehicle.Subentities.Lights;
using Helpers;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entities.Vehicle.Helpers
{
    [Serializable]
    public class IlluminatedItem
    {
        private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");
        [SerializeField] private LightIntensityPair[] lightPairs;
        [SerializeField] private EmissionMeshPair[] emissionPairs;
        [SerializeField] private InterfaceWrapper<IIlluminated>[] illuminatedObjects;
        [SerializeField] private float duration = 0.1f;
        
        private Coroutine _transitionCoroutine;
        private float _timer;

        public IlluminatedItem() { }

        public IlluminatedItem(EmissionMeshPair[] emissionPairs)
        {
            this.emissionPairs = emissionPairs;
            this.lightPairs = Array.Empty<LightIntensityPair>();
            illuminatedObjects = Array.Empty<InterfaceWrapper<IIlluminated>>();
        }

        public void ApplyIntensity(MonoBehaviour monoBehaviour, float percentage)
        {
            if (_transitionCoroutine != null)
                monoBehaviour.StopCoroutine(_transitionCoroutine);
            _timer = 0f;
            _transitionCoroutine = monoBehaviour.StartCoroutine(Transition(percentage));
            foreach (var illuminated in illuminatedObjects)
            {
                illuminated.Value.SetBacklight(percentage);
            }
        }

        private IEnumerator Transition(float percentage)
        {
            var intensityArray = GetIntensityArray();
            var rangeArray = GetRangeArray();
            var colorArray = GetColorArray();
            while (_timer < duration)
            {
                _timer += Time.deltaTime;
                var progress = Mathf.Clamp01(_timer / duration);
                
                var i = 0;
                foreach (var pair in lightPairs)
                {
                    if (pair.Light.type == LightType.Tube)
                    {
                        pair.Light.intensity = Mathf.Lerp(intensityArray[i], pair.Intensity * percentage, progress);
                        continue;
                    }
                    
                    var converted = LightUnitUtils.ConvertIntensity(pair.Light, pair.Intensity * percentage,
                        pair.LightUnit, LightUnit.Candela);
                    pair.Light.intensity = Mathf.Lerp(intensityArray[i], converted, progress);
                    pair.Light.range = Mathf.Lerp(rangeArray[i], pair.Range, progress);
                    i++;
                }

                i = 0;
                foreach (var pair in emissionPairs)
                {
                    pair.MeshRenderer.materials[pair.MaterialIndex].SetColor(EmissiveColor,
                        Color.Lerp(colorArray[i], pair.EmissionColor * pair.EmissionIntensity * percentage, progress));
                    i++;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private float[] GetIntensityArray()
        {
            var intensityArray = new float[lightPairs.Length];
            for (int i = 0; i < lightPairs.Length; i++)
            {
                var light = lightPairs[i].Light;
                // if (light.type == LightType.Tube)
                // {
                //     intensityArray[i] = light.
                //     continue;
                // }
                intensityArray[i] = light.intensity;
                // LightUnitUtils.ConvertIntensity(light, light.intensity, LightUnit.Candela, lightPairs[i].LightUnit);
            }
            return intensityArray;
        }

        private float[] GetRangeArray()
        {
            var rangeArray = new float[lightPairs.Length];
            for (int i = 0; i < lightPairs.Length; i++)
            {
                var light = lightPairs[i].Light;
                rangeArray[i] = light.range;
            }
            return rangeArray;
        }

        private Color[] GetColorArray()
        {
            var colorArray = new Color[emissionPairs.Length];
            for (int i = 0; i < emissionPairs.Length; i++)
            {
                var pair = emissionPairs[i];
                var emissive = pair.MeshRenderer.materials[pair.MaterialIndex].GetColor(EmissiveColor);
                colorArray[i] = emissive;
            }
            return colorArray;
        }
    }
}