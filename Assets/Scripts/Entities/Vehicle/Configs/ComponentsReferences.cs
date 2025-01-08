using System;
using UnityEngine;

namespace Entities.Vehicle.Configs
{
    [Serializable]
    public class ComponentsReferences
    {
        [SerializeField] private GameObject vehicleRoot;
        
        public GameObject VehicleRoot => vehicleRoot;
    }
}