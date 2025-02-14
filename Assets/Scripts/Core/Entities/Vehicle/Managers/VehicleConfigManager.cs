using System;
using Core.Entities.Vehicle.Configs;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Configs.Drivetrain;
using Core.Entities.Vehicle.Configs.Electricity;
using Core.Entities.Vehicle.Configs.Interactions;
using Core.Entities.Vehicle.Data;
using UnityEngine;

namespace Core.Entities.Vehicle.Managers
{
    [Serializable]
    public class VehicleConfigManager
    {
        [SerializeField] private ComponentsReferences componentsReferences;
        [SerializeField] private ComfortConfig comfortConfig;
        [SerializeField] private ElectricityConfig electricityConfig;
        [SerializeField] private InteractionsConfig interactionsConfig;
        [field: SerializeField] public SoundsConfig SoundsConfig { get; private set; }
        [field: SerializeField] public DrivetrainConfig DrivetrainConfig { get; private set; }
        [field: SerializeField] public VehicleInstanceData VehicleData { get; private set; }
        
        
        
        public ComponentsReferences ComponentsReferences => componentsReferences;
        public ComfortConfig ComfortConfig => comfortConfig;
        public ElectricityConfig ElectricityConfig => electricityConfig;
        public InteractionsConfig InteractionsConfig => interactionsConfig;
    }
}