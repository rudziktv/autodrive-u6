using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs
{
    [Serializable]
    public class ComponentsReferences
    {
        [SerializeField] private GameObject vehicleRoot;
        [field: SerializeField] public Animator Animator { get; private set; }
        
        public GameObject VehicleRoot => vehicleRoot;
    }
}