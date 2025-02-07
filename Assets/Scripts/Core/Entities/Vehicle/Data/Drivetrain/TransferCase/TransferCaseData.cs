using System;

namespace Core.Entities.Vehicle.Data.Drivetrain.TransferCase
{
    [Serializable]
    public struct TransferCaseData
    {
        public TransferCaseType type;
        public AxleData[] axles;
    }
}