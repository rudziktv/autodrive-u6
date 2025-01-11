using Core.Components.Lights.Standalone;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Lights
{
    public class TaillightsController : MonoBehaviour
    {
        // [SerializeField] private LightLevel[] lightLevels;
        // [SerializeField] private LightLevel brakeLevel;

        [SerializeField] private LightLevelStandalone positionLightsLevel;
        [SerializeField] private LightLevelStandalone brakeLightsLevel;

        private bool _brakeLightsActive;
        private bool _positionLightsActive;
        
        public void TurnOffLights()
        {
            _positionLightsActive = false;
            if (_brakeLightsActive)
            {
                positionLightsLevel.ApplyAnimatedWithoutOverride(brakeLightsLevel, 0f);
                brakeLightsLevel.ApplyAnimated();
                return;
            }
            // if (_brakeLightsActive)
            //     lightLevels[0].ApplyRespectingOtherLevel(this);
            positionLightsLevel.ApplyAnimated(0f);
        }

        public void TurnOffAllLights()
        {
            _positionLightsActive = false;
            _brakeLightsActive = false;
            positionLightsLevel.ApplyAnimated(0f);
            brakeLightsLevel.ApplyAnimatedWithoutOverride(positionLightsLevel, 0f);
        }

        public void TurnOnPositionLights()
        {
            _positionLightsActive = true;
            if (_brakeLightsActive)
            {
                positionLightsLevel.ApplyAnimatedWithoutOverride(brakeLightsLevel);
                return;
            }
            positionLightsLevel.ApplyAnimated();
        }

        public void TurnOnBrakeLights()
        {
            _brakeLightsActive = true;
            positionLightsLevel.ApplyAnimatedWithoutOverride(brakeLightsLevel, _positionLightsActive ? 1f : 0f);
            // if (_positionLightsActive)
            //     lightLevels[1].ApplyRespectingOtherLevel(this, brakeLevel);
            // else
            //     lightLevels[0].ApplyRespectingOtherLevel(this, brakeLevel);

            brakeLightsLevel.ApplyAnimated();
        }

        public void TurnOffBrakeLights()
        {
            _brakeLightsActive = false;

            if (!_positionLightsActive)
            {
                TurnOffAllLights();
                return;
            }
            
            brakeLightsLevel.ApplyAnimatedWithoutOverride(positionLightsLevel, 0f);
            positionLightsLevel.ApplyAnimated();
            // lightLevels[1].Apply(this);
        }
    }
}