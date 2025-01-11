using System;
using UnityEngine;

namespace Systems.Info
{
    [Serializable]
    public class VehicleInfo : ScriptableObject
    {
        [field: SerializeField] public BrandInfo Brand { get; set; }
        [field: SerializeField] public VehicleModelInfo Model { get; set; }
        [field: SerializeField] public ModelVersionInfo ModelVersion { get; set; }
        [field: SerializeField] public DrivetrainInfo Drivetrain { get; set; }

        public EquipmentInfo[] StandardEquipment => ModelVersion.StandardEquipment;
        [field: SerializeField] public EquipmentInfo[] AdditionalEquipment { get; set; }
    }
}