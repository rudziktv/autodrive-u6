using System;
using UnityEngine;

namespace Entities.Vehicle.Configs.Electricity
{
    [Serializable]
    public class ElectricityConfig
    {
        [SerializeField] private BatteryConfig batteryConfig;
        [SerializeField] private AlternatorConfig alternatorConfig;
        
        public BatteryConfig BatteryConfig => batteryConfig;
        public AlternatorConfig AlternatorConfig => alternatorConfig;
    }
}