using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs
{
    [Serializable]
    public class ComponentsReferences
    {
        [SerializeField] private GameObject vehicleRoot;
        
        public GameObject VehicleRoot => vehicleRoot;
    }
}