using System.Collections;
using Core.Entities.Vehicle.Subentities.Lights;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Dashboard
{
    public class IndicatorController : MonoBehaviour
    {
        protected static readonly int EmissionColor = Shader.PropertyToID("_EmissiveColor");
        [SerializeField] protected EmissionMeshPair[] indicatorLevels;

        protected Coroutine TransitionCoroutine;
        protected float Timer;

        public virtual void SetIndicator(bool lightUp, int level = 0, float duration = 0.05f)
        {
            if (TransitionCoroutine != null)
                StopCoroutine(TransitionCoroutine);
            level = Mathf.Clamp(level, 0, indicatorLevels.Length - 1);
            Timer = 0;
            TransitionCoroutine = StartCoroutine(Transition(indicatorLevels[level], duration, lightUp ? 1 : 0));
        }
        
        protected virtual IEnumerator Transition(EmissionMeshPair level, float duration, float intensity)
        {
            var index = level.MaterialIndex;
            var material = level.MeshRenderer.materials[index];
            var oldColor = material.GetColor(EmissionColor);

            while (Timer < duration)
            {
                Timer += Time.deltaTime;
                
                var progress = Mathf.Clamp01(Timer / duration);
                material.SetColor(EmissionColor,
                    Color.Lerp(oldColor, level.EmissionColor * level.EmissionIntensity * intensity, progress));
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}