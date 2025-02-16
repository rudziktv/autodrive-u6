using System;

namespace Core.Entities.Vehicle.Data.Status
{
    [Serializable]
    public struct VehicleStatusPair
    {
        public string key;
        public string defaultValue;
    }
}