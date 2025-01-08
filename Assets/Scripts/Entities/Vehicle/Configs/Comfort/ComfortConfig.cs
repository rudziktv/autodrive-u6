using System;
using UnityEngine;

namespace Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class ComfortConfig
    {
        [SerializeField] private LightsConfig lightsConfig;
        [SerializeField] private DashboardConfig dashboardConfig;
        
        public LightsConfig LightsConfig => lightsConfig;
        public DashboardConfig DashboardConfig => dashboardConfig;
    }
}