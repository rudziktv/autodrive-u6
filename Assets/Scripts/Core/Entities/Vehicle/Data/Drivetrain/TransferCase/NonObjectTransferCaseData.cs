using System;

namespace Core.Entities.Vehicle.Data.Drivetrain.TransferCase
{
    [Serializable]
    public struct NonObjectTransferCaseData
    {
        public TransferCaseType type;
        public AxleData[] axles;
    }
}