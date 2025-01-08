using System;
using UnityEngine;

namespace Entities.Vehicle.Configs.Electricity
{
    [Serializable]
    public class AlternatorConfig
    {
        [SerializeField] private float minVoltage = 13.8f;
        [SerializeField] private float maxVoltage = 14.4f;
        [SerializeField] private float temperatureCompensation = 0.02f; // per degree
        
        public float MinVoltage => minVoltage;
        public float MaxVoltage => maxVoltage;
        public float TemperatureCompensation => temperatureCompensation;
    }
}