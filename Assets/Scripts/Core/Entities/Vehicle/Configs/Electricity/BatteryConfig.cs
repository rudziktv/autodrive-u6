using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Electricity
{
    [Serializable]
    public class BatteryConfig
    {
        [SerializeField] private float maxCapacity = 72f;
        [SerializeField] private float initialChargeLevel = 70f;
        [SerializeField] private float internalResistance = 0.05f;
        
        public float MaxCapacity => maxCapacity;
        public float InitialChargeLevel => initialChargeLevel;
        public float InternalResistance => internalResistance;

    }
}