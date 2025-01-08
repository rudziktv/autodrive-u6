using UnityEngine;

namespace Game.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Model Version Info")]
    public class ModelVersionInfo : ScriptableObject
    {
        [field: SerializeField] public string VersionName { get; private set; }
        [field: SerializeField] public EquipmentInfo[] StandardEquipment { get; private set; }
        [field: SerializeField] public EquipmentInfo[] AdditionalEquipment { get; private set; }
        [field: SerializeField] public PricePair<EquipmentInfo>[] AvailableEquipment { get; private set; }
        [field: SerializeField] public PricePair<DrivetrainInfo>[] AvailableDrivetrains { get; private set; }
    }
}