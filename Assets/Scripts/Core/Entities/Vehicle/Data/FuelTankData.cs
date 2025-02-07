using System;
using Systems.Info;
using UnityEngine;

namespace Core.Entities.Vehicle.Data
{
    [Serializable]
    public class FuelTankData
    {
        public FuelTypeEnum fuelType;
        public float buffer;
        public float maxCapacity;
    }
}