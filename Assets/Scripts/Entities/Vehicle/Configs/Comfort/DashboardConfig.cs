using System;
using Entities.Vehicle.Helpers;
using Entities.Vehicle.Subentities.Dashboard;
using Entities.Vehicle.Subentities.Lights;
using UnityEngine;

namespace Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class DashboardConfig
    {
        [SerializeField] private GaugeController fuelGauge;
        
        [SerializeField] private DashboardIllumination dashboardIllumination;
        [SerializeField] private IndicatorsConfig indicatorsConfig;
        
        // [SerializeField] private LightLevel 
        
        public GaugeController FuelGauge => fuelGauge;
        public DashboardIllumination DashboardIllumination => dashboardIllumination;
        public IndicatorsConfig IndicatorsConfig => indicatorsConfig;
    }
}