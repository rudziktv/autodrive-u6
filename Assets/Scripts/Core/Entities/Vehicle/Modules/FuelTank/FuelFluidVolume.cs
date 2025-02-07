using System;

namespace Core.Entities.Vehicle.Modules.FuelTank
{
    [Serializable]
    public class FuelFluidVolume
    {
        public float volume;
        public float avgOctane;
        public float avgCetane;
        public float avgCarbonChainLength;
    }
}