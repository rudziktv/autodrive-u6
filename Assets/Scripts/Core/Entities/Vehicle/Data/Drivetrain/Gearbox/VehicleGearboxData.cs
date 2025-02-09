using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain.Gearbox
{
    public class VehicleGearboxData : ScriptableObject
    {
        [Header("Info")]
        public string gearboxName;
        public string gearboxCode;

        [Header("Ratios")]
        public float finalRatio;
        public float[] gearRatios;
        public float reverseRatio;

        [Header("Limits")]
        public float maxInputTorque;
    }
}