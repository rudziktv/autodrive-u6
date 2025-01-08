using System;
using System.Collections;
using System.Linq;
using Graphics.Lights;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entities.Vehicle.Subentities.Lights
{
    [Serializable]
    public class LightLevel
    {
        private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");
        [SerializeField] private LightIntensityPair[] lightPairs;
        [SerializeField] private AdvancedEmissiveLight[] emissionPairs;
        [SerializeField] private GameObject[] lightBeamPatternShape;
        [SerializeField] private bool usePatternShape = true;
        [SerializeField] private float duration;

        private float _timer;
        private Coroutine _transitionCoroutine;
        
        public LightIntensityPair[] LightPairs => lightPairs;
        public AdvancedEmissiveLight[] EmissionPairs => emissionPairs;

        public void Apply(MonoBehaviour monoBehaviour)
        {
            if (_transitionCoroutine != null)
                monoBehaviour.StopCoroutine(_transitionCoroutine);
            _timer = 0f;

            foreach (var pair in emissionPairs)
            {
                pair.ApplyAnimatedEmission(monoBehaviour, duration);
            }
            
            _transitionCoroutine = monoBehaviour.StartCoroutine(Transition());
        }

        public void ApplyRespectingOtherLevel(MonoBehaviour monoBehaviour, LightLevel other, float percentage = 1f)
        {
            if (_transitionCoroutine != null)
                monoBehaviour.StopCoroutine(_transitionCoroutine);
            _timer = 0f;
            
            var emissionTags = other.EmissionPairs.Select(e => e.Name);
            var respectingEmissions = EmissionPairs.Where(e => !emissionTags.Contains(e.Name)).ToArray();
            
            foreach (var emission in respectingEmissions)
            {
                emission.ApplyAnimatedEmission(monoBehaviour, duration, percentage);
            }
            
            _transitionCoroutine = monoBehaviour.StartCoroutine(RespectingTransition(other, percentage));
        }

        private IEnumerator RespectingTransition(LightLevel other, float percentage)
        {
            var lightTags = other.LightPairs.Select(l => l.Name);
            var respectingLights = LightPairs.Where(l => !lightTags.Contains(l.Name)).ToArray();
            
            var intensityArray = GetIntensityArray(respectingLights);
            var rangeArray = GetRangeArray(respectingLights);
            var scaleArray = GetScaleArray();

            
            while (_timer < duration)
            {
                _timer += Time.deltaTime;
                var progress = Mathf.Clamp01(_timer / duration);
                
                var i = 0;
                foreach (var shaper in lightBeamPatternShape)
                {
                    // shaper.SetActive(usePatternShape);
                    var target = usePatternShape ? Vector3.one : Vector3.zero;
                    shaper.transform.localScale = Vector3.Lerp(scaleArray[i], target * percentage, progress);
                    i++;
                }

                i = 0;
                foreach (var pair in respectingLights)
                {
                    pair.Light.intensity = LightUnitUtils.ConvertIntensity(pair.Light,
                        Mathf.Lerp(intensityArray[i], pair.Intensity * percentage, progress),
                        LightUnit.Lumen, LightUnit.Candela);
                    pair.Light.range = Mathf.Lerp(rangeArray[i], pair.Range, progress);
                    i++;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator Transition()
        {
            var intensityArray = GetIntensityArray(LightPairs);
            var rangeArray = GetRangeArray(LightPairs);
            var scaleArray = GetScaleArray();
            // var colorArray = GetColorArray();
            while (_timer < duration)
            {
                _timer += Time.deltaTime;
                var progress = Mathf.Clamp01(_timer / duration);
                
                var i = 0;
                foreach (var shaper in lightBeamPatternShape)
                {
                    // shaper.SetActive(usePatternShape);
                    var target = usePatternShape ? Vector3.one : Vector3.zero;
                    shaper.transform.localScale = Vector3.Lerp(scaleArray[i], target, progress);
                    i++;
                }

                i = 0;
                foreach (var pair in lightPairs)
                {
                    pair.Light.intensity = LightUnitUtils.ConvertIntensity(pair.Light,
                        Mathf.Lerp(intensityArray[i], pair.Intensity, progress),
                        LightUnit.Lumen, LightUnit.Candela);
                    pair.Light.range = Mathf.Lerp(rangeArray[i], pair.Range, progress);
                    i++;
                }

                // i = 0;
                // foreach (var pair in emissionPairs)
                // {
                //     pair.MeshRenderer.material.SetColor(EmissiveColor,
                //         Color.Lerp(colorArray[i], pair.EmissionColor * pair.EmissionIntensity, progress));
                //     i++;
                // }

                yield return new WaitForEndOfFrame();
            }
        }

        private float[] GetIntensityArray(LightIntensityPair[] pairs)
        {
            var intensityArray = new float[pairs.Length];
            for (int i = 0; i < pairs.Length; i++)
            {
                var light = pairs[i].Light;
                intensityArray[i] =
                    LightUnitUtils.ConvertIntensity(light, light.intensity, LightUnit.Candela, LightUnit.Lumen);
            }
            return intensityArray;
        }

        private float[] GetRangeArray(LightIntensityPair[] pairs)
        {
            var rangeArray = new float[pairs.Length];
            for (int i = 0; i < pairs.Length; i++)
            {
                var light = pairs[i].Light;
                rangeArray[i] = light.range;
            }
            return rangeArray;
        }

        private Vector3[] GetScaleArray()
        {
            var scaleArray = new Vector3[lightBeamPatternShape.Length];
            for (int i = 0; i < lightBeamPatternShape.Length; i++)
            {
                var pattern = lightBeamPatternShape[i];
                scaleArray[i] = pattern.transform.localScale;
            }
            return scaleArray;
        }

        // private Color[] GetColorArray()
        // {
        //     var colorArray = new Color[emissionPairs.Length];
        //     for (int i = 0; i < emissionPairs.Length; i++)
        //     {
        //         var emissive = emissionPairs[i].MeshRenderer.material.GetColor(EmissiveColor);
        //         colorArray[i] = emissive;
        //     }
        //     return colorArray;
        // }
    }
}