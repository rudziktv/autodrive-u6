using System;
using Core.Entities.Vehicle.Helpers;
using Core.Entities.Vehicle.Subentities.Dashboard;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
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