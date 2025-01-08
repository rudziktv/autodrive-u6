using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Graphics.Lights.Standalone
{
    public class AdvancedPhysicalLightStandalone : MonoBehaviour
    {
        // [SerializeField] private string name;
        [SerializeField] private new Light light;
        [SerializeField] private float range;
        [SerializeField] private float intensity;
        [SerializeField] private LightUnit lightUnit = LightUnit.Lumen;
        
        public float Intensity => intensity;
        public float Range => range;
        public Light Light => light;
        public LightUnit LightUnit => lightUnit;
        // public string Name => name;
        
        private Coroutine _transitionCoroutine;

        public void ApplyInstant(float percentage = 1f)
        {
            StopTransition();
            SetAllValues(Range, Intensity * percentage);
        }

        public void ApplyAnimated(float duration, float percentage = 1f)
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
            var timer = 0f;
            
            var oldRange = Light.range;
            var oldIntensity = Light.intensity;
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;

                Light.range = Mathf.Lerp(oldRange, Range, progress);

                if (Light.type != LightType.Tube)
                {
                    var converted = LightUnitUtils.ConvertIntensity(Light, Intensity * percentage,
                        LightUnit, LightUnit.Candela);
                    Light.intensity = Mathf.Lerp(oldIntensity, converted, progress);
                }
                else
                    Light.intensity = Mathf.Lerp(oldIntensity, Intensity * percentage, progress);

                yield return new WaitForEndOfFrame();
            }
        }

        private void SetAllValues(float newRange, float newIntensity)
        {
            Light.range = newRange;
                
            if (Light.type != LightType.Tube)
            {
                var converted = LightUnitUtils.ConvertIntensity(Light, newIntensity,
                    LightUnit, LightUnit.Candela);
                Light.intensity = converted;
            }
                
            Light.intensity = newIntensity;
        }
    }
}