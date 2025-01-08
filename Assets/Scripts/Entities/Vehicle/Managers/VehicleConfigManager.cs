using System;
using Entities.Vehicle.Configs;
using Entities.Vehicle.Configs.Comfort;
using Entities.Vehicle.Configs.Electricity;
using Entities.Vehicle.Configs.Interactions;
using UnityEngine;

namespace Entities.Vehicle.Managers
{
    [Serializable]
    public class VehicleConfigManager
    {
        [SerializeField] private ComponentsReferences componentsReferences;
        [SerializeField] private ComfortConfig comfortConfig;
        [SerializeField] private ElectricityConfig electricityConfig;
        [SerializeField] private InteractionsConfig interactionsConfig;
        
        
        
        public ComponentsReferences ComponentsReferences => componentsReferences;
        public ComfortConfig ComfortConfig => comfortConfig;
        public ElectricityConfig ElectricityConfig => electricityConfig;
        public InteractionsConfig InteractionsConfig => interactionsConfig;
    }
}