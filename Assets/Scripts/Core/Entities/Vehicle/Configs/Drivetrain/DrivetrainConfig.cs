using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Drivetrain
{
    [Serializable]
    public class DrivetrainConfig
    {
        [field: SerializeField] public AxleConfig[] Axles { get; private set; }
    }
}