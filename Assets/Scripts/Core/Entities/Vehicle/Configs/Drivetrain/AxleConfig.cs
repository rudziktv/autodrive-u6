using System;
using Core.Components.Vehicles;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Drivetrain
{
    [Serializable]
    public class AxleConfig
    {
        [field: SerializeField] public TireComponent[] Tires { get; private set; }
    }
}