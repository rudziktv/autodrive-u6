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
        
        [field: SerializeField] public GaugeController Tachometer { get; private set; }
        [field: SerializeField] public GaugeController CoolantGauge { get; private set; }
        
        // [SerializeField] private LightLevel 
        
        public GaugeController FuelGauge => fuelGauge;
        public DashboardIllumination DashboardIllumination => dashboardIllumination;
        public IndicatorsConfig IndicatorsConfig => indicatorsConfig;
    }
}