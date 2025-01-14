using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class ComfortConfig
    {
        [SerializeField] private LightsConfig lightsConfig;
        [SerializeField] private DashboardConfig dashboardConfig;
        [field: SerializeField] public BlinkerConfig BlinkerConfig { get; private set; }
        
        public LightsConfig LightsConfig => lightsConfig;
        public DashboardConfig DashboardConfig => dashboardConfig;
    }
}