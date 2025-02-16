using System;

namespace Core.Entities.Vehicle.Data.Status
{
    [Serializable]
    public struct VehicleStatusData
    {
        public VehicleStatusPair[] pairs;
    }
}