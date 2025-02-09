using System;
using Core.Utils;
using UnityEngine;

namespace Systems.Environment
{
    public class SimpleEnvironment : MonoBehaviour
    {
        public static SimpleEnvironment instance;
        
        /// <summary>
        /// Average ambient air temperature, in Kelvins.
        /// </summary>
        [Obsolete("currentTemperature is deprecated, please use AmbientTemperatureKelvin or Celsius instead.")]
        public float currentTemperature = 0f;

        [Tooltip("Ambient air temperature in Celsius.")]
        [SerializeField] private float ambientTemperatureCelsius;

        public float AmbientTemperatureKelvin
        {
            get => TempUnitUtils.CelsiusToKelvin(ambientTemperatureCelsius);
            set => ambientTemperatureCelsius = TempUnitUtils.KelvinToCelsius(value);
        }

        public float AmbientTemperatureCelsius
        {
            get => TempUnitUtils.ValidateCelsius(ambientTemperatureCelsius);
            set => ambientTemperatureCelsius = TempUnitUtils.ValidateCelsius(value);
        }

        private void Awake()
        {
            instance = this;
        }
    }
}