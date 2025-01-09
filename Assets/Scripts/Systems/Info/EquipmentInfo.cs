using System.Collections.Generic;
using Core.Utils;
using Systems.Info.Coding;
using UnityEngine;

namespace Systems.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Equipment Info")]
    public class EquipmentInfo : ScriptableObject
    {
        [field: SerializeField] public string EquipmentName { get; private set; }
        [field: SerializeField] public float EquipmentPrice { get; private set; }
        
        [field: SerializeField] public EquipmentInfo[] RequiredEquipment { get; private set; }
        [field: SerializeField] public EquipmentInfo[] NotCompatibleEquipment { get; private set; }
        
        [field: SerializeField] public PackageInfo[] RequiredPackages { get; private set; }
        [field: SerializeField] public bool HideIfPackagesNotSelected { get; private set; }
        
        [field: SerializeField] public CodingVariables[] EquipmentCoding { get; private set; }
        
        [field: SerializeField] public string[] EnabledObjectsNames { get; private set; }
        [field: SerializeField] public string[] DisabledObjectsNames { get; private set; }

        public void ApplyEquipment(GameObject vehicleRoot)
        {
            var enableObjects = new List<GameObject>();
            var disableObjects = new List<GameObject>();
            
            foreach (var e in EnabledObjectsNames)
            {
                enableObjects.AddRange(RecursiveChildFinder.FindChildrenWithNameRecursive(vehicleRoot, e));
            }

            foreach (var d in DisabledObjectsNames)
            {
                disableObjects.AddRange(RecursiveChildFinder.FindChildrenWithNameRecursive(vehicleRoot, d));
            }

            foreach (var e in enableObjects)
            {
                e.SetActive(true);
            }

            foreach (var d in disableObjects)
            {
                d.SetActive(false);
            }
        }
    }
}