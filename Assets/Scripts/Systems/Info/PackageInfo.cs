using UnityEngine;

namespace Systems.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Package Info")]
    public class PackageInfo : ScriptableObject
    {
        [field: SerializeField] public string PackageName { get; private set; }
        [field: SerializeField] public PackageInfo[] IncludedPackages { get; private set; }
        [field: SerializeField] public EquipmentInfo[] IncludedEquipment { get; private set; }
        
        /// <summary>
        /// If you chose incompatible additional equipment, equipment included inside this package, will be replaced.
        /// </summary>
        [field: SerializeField] public bool IncompatibleEquipmentReplace { get; private set; }
    }
}