using System.Collections.Generic;
using UnityEngine;

namespace Systems.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Brand Info")]
    public class BrandInfo : ScriptableObject
    {
        [field: SerializeField] public string BrandName { get; private set; }
        [field: SerializeField] public Texture2D BrandLogo { get; private set; }
        [field: SerializeField] public List<VehicleModelInfo> Models { get; private set; }
    }
}