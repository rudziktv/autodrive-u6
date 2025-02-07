using System;
using Core.Entities.Vehicle.Data.Drivetrain.Differential;

namespace Core.Entities.Vehicle.Data.Drivetrain.TransferCase
{
    [Serializable]
    public class AxleData
    {
        public bool powered;
        public DifferentialData differential;
    }
}