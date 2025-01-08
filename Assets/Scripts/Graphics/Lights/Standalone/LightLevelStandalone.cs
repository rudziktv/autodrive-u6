using System.Collections;
using System.Linq;
using UnityEngine;

namespace Graphics.Lights.Standalone
{
    public class LightLevelStandalone : MonoBehaviour, ILightLevel
    {
        [SerializeField] private AdvancedPhysicalLightStandalone[] lights;
        [SerializeField] private AdvancedEmissiveLightStandalone[] emissives;
        [SerializeField] private GameObject[] cutOffLineObjects;

        [SerializeField] private bool useCutOffLine;
        [SerializeField] private float duration;


        private Coroutine _transitionCoroutine;

        public void ApplyInstant(float percentage = 1f)
        {
            StopTransition();
            
            foreach (var light in lights)
            {
                light.ApplyInstant(percentage);
            }

            foreach (var emissive in emissives)
            {
                emissive.ApplyInstantEmission(percentage);
            }

            foreach (var cutOffLine in cutOffLineObjects)
            {
                var target = useCutOffLine ? Vector3.one : Vector3.zero;
                cutOffLine.transform.localScale = target;
            }
        }

        public void ApplyInstantWithoutOverride(ILightLevel other, float percentage = 1f)
        {
            var respectingLights = GetLightsExcluding(other.GetLightNames());
            var respectingEmissives = GetEmissivesExcluding(other.GetEmissiveNames());
            // var respectingCutOffLines = GetCutOffLinesExcluding(other.GetCutOffLines());
            StopTransition(respectingLights, respectingEmissives);

            foreach (var l in respectingLights)
            {
                l.ApplyInstant(percentage);
            }

            foreach (var e in respectingEmissives)
            {
                e.ApplyInstantEmission(percentage);
            }
            
            foreach (var cutOffLine in cutOffLineObjects)
            {
                var target = useCutOffLine ? Vector3.one : Vector3.zero;
                cutOffLine.transform.localScale = target;
            }
        }
        
        public void ApplyAnimated(float percentage = 1f)
        {
            StopTransition();

            foreach (var light in lights)
            {
                light.ApplyAnimated(duration, percentage);
            }

            foreach (var emissive in emissives)
            {
                emissive.ApplyAnimatedEmission(duration, percentage);
            }

            _transitionCoroutine = StartCoroutine(Transition());
        }

        public void ApplyAnimatedWithoutOverride(ILightLevel other, float percentage = 1f)
        {
            var respectingLights = GetLightsExcluding(other.GetLightNames());
            var respectingEmissives = GetEmissivesExcluding(other.GetEmissiveNames());
            StopTransition(respectingLights, respectingEmissives);

            foreach (var l in respectingLights)
            {
                l.ApplyAnimated(duration, percentage);
            }

            foreach (var e in respectingEmissives)
            {
                e.ApplyAnimatedEmission(duration, percentage);
            }
            
            _transitionCoroutine = StartCoroutine(Transition());
        }

        private void StopTransition()
        {
            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

            foreach (var l in lights)
            {
                l.StopTransition();
            }

            foreach (var e in emissives)
            {
                e.StopTransition();
            }
        }
        
        private void StopTransition(AdvancedPhysicalLightStandalone[] customLights, AdvancedEmissiveLightStandalone[] customEmissives)
        {
            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

            foreach (var l in customLights)
            {
                l.StopTransition();
            }

            foreach (var e in customEmissives)
            {
                e.StopTransition();
            }
        }
        
        private IEnumerator Transition()
        {
            var timer = 0f;
            var scaleArray = GetScaleArray();

            while (timer < duration)
            {
                timer += Time.deltaTime;
                var progress = timer / duration;

                for (int i = 0; i < cutOffLineObjects.Length; i++)
                {
                    var cutOffLine = cutOffLineObjects[i];
                    var target = useCutOffLine ? Vector3.one : Vector3.zero;
                    cutOffLine.transform.localScale = Vector3.Lerp(scaleArray[i], target, progress);
                }

                yield return new WaitForEndOfFrame();
            }
        }
        
        private Vector3[] GetScaleArray()
        {
            var scaleArray = new Vector3[cutOffLineObjects.Length];
            for (int i = 0; i < cutOffLineObjects.Length; i++)
            {
                var pattern = cutOffLineObjects[i];
                scaleArray[i] = pattern.transform.localScale;
            }
            return scaleArray;
        }

        public string[] GetLightNames()
        {
            return lights.Select(l => l.name).ToArray();
        }

        public string[] GetEmissiveNames()
        {
            return emissives.Select(e => e.name).ToArray();
        }

        public GameObject[] GetCutOffLines()
        {
            return cutOffLineObjects;
        }

        public AdvancedPhysicalLightStandalone[] GetLightsExcluding(string[] excludedNames)
        {
            return lights.Where(l => !excludedNames.Contains(l.name)).ToArray();
        }

        public AdvancedEmissiveLightStandalone[] GetEmissivesExcluding(string[] excludedNames)
        {
            return emissives.Where(e => !excludedNames.Contains(e.name)).ToArray();
        }

        public GameObject[] GetCutOffLinesExcluding(GameObject[] excludedObjects)
        {
            return cutOffLineObjects.Where(o => !excludedObjects.Contains(o)).ToArray();
        }
    }
}