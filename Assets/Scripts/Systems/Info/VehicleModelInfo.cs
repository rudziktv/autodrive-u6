using System.Collections.Generic;
using UnityEngine;

namespace Systems.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Vehicle Model Info")]
    public class VehicleModelInfo : ScriptableObject
    {
        [field: SerializeField] public string ModelName { get; private set; }
        [field: SerializeField] public List<ModelVersionInfo> Versions { get; private set; }
    }
}