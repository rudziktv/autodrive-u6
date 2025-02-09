using UnityEngine;

namespace Core.Utils
{
    public static class TempUnitUtils
    {
        public static float KelvinToCelsius(float kelvin)
            => ValidateCelsius(kelvin - 273.15f);
        
        public static float CelsiusToKelvin(float celsius)
            => ValidateKelvin(celsius + 273.15f);
        
        public static float ValidateKelvin(float kelvin)
            => Mathf.Clamp(kelvin, 0, float.MaxValue);
        
        public static float ValidateCelsius(float celsius)
            => Mathf.Clamp(celsius, -273.15f, float.MaxValue);
    }
}