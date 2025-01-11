using System;
using System.Collections;
using Core.Entities.Vehicle.Subentities.Lights;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Dashboard
{
    public class IlluminatedIndicatorController : IndicatorController, IIlluminated
    {
        private static readonly int EmissiveColorMap = Shader.PropertyToID("_EmissiveColorMap");
        
        [SerializeField] private Texture2D[] illuminationTextures;

        private int _currentLevel = -1;
        private float _backlightIntensity = 0f;

        private Texture2D _illuminationMap;

        private void Awake()
        {
            if (indicatorLevels.Length == 0)
                throw new Exception("No illuminated indicator levels");
            _illuminationMap = indicatorLevels[0].MeshRenderer.materials[indicatorLevels[0].MaterialIndex].
                GetTexture(EmissiveColorMap) as Texture2D ?? Texture2D.whiteTexture;
        }

        private Texture2D GetIlluminationTexture(int level)
        {
            if (illuminationTextures.Length == 0)
                return _illuminationMap;
            level = Mathf.Clamp(level, 0, illuminationTextures.Length - 1);
            return illuminationTextures[level];
        }

        public override void SetIndicator(bool lightUp, int level = 0, float duration = 0.05f)
        {
            // base.SetIndicator(lightUp, level, duration);
            if (TransitionCoroutine != null)
                StopCoroutine(TransitionCoroutine);
            _currentLevel = lightUp ? level : -1;
            level = Mathf.Clamp(level + 1, 0, indicatorLevels.Length - 1);
            Timer = 0;

            TransitionCoroutine = StartCoroutine(_currentLevel == -1 ? 
                Transition(indicatorLevels[0], _illuminationMap, duration, _backlightIntensity) : 
                Transition(indicatorLevels[level], GetIlluminationTexture(level), duration, lightUp ? 1 : 0));
        }

        public void SetBacklight(float intensity)
        {
            _backlightIntensity = intensity;
            
            if (_currentLevel != -1) return;
            if (TransitionCoroutine != null)
                StopCoroutine(TransitionCoroutine);
            const float duration = 0.05f;
            Timer = 0;
            TransitionCoroutine = StartCoroutine(Transition(indicatorLevels[0], _illuminationMap, duration, _backlightIntensity));
        }

        protected IEnumerator Transition(EmissionMeshPair level, Texture2D targetTex, float duration, float intensity)
        {
            var index = level.MaterialIndex;
            var material = level.MeshRenderer.materials[index];
            var oldColor = material.GetColor(EmissionColor);
            // var oldTex = material.GetTexture(EmissiveColorMap) as Texture2D;
            //
            // if (!oldTex)
            //     throw new Exception("No illuminated indicator texture was found");

            material.SetTexture(EmissiveColorMap, targetTex);
            
            while (Timer < duration)
            {
                Timer += Time.deltaTime;
                
                var progress = Mathf.Clamp01(Timer / duration);
                material.SetColor(EmissionColor,
                    Color.Lerp(oldColor, level.EmissionColor * level.EmissionIntensity * intensity, progress));
                // material.SetTexture(EmissiveColorMap, TextureUtils.BlendTextures(oldTex, targetTex, progress));

                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}