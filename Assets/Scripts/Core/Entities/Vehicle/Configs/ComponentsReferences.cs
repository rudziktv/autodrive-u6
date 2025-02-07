using System;
using Systems.Cameras.Vehicle;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs
{
    [Serializable]
    public class ComponentsReferences
    {
        [SerializeField] private GameObject vehicleRoot;
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public VehicleCameraSystem CameraSystem { get; private set; }
        
        public GameObject VehicleRoot => vehicleRoot;
    }
}