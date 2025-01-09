using System;
using Core.Entities.Vehicle.Helpers;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Lights
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] private IlluminatedItem[] illuminatedLevels;

        public void TurnOnLight(float intensity = 1f, int level = 0)
        {
            if (illuminatedLevels.Length == 0)
                throw new Exception("No illuminated levels have been assigned.");
            level = Mathf.Clamp(level, 0, illuminatedLevels.Length - 1);
            illuminatedLevels[level].ApplyIntensity(this, intensity);
        }

        public void TurnOffLight()
        {
            illuminatedLevels[0].ApplyIntensity(this, 0f);
        }
    }
}