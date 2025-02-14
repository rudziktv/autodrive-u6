using System;
using Core.Entities.Vehicle.Data.Drivetrain.TransferCase;
using Core.Entities.Vehicle.Modules.TransferCase;

namespace Core.Entities.Vehicle.Builders
{
    public static class TransferCaseBuilder
    {
        public static ITransferCase BuildTransferCase(this TransferCaseData transferCaseData, VehicleController ctr)
        {
            // if (transferCaseData.type == TransferCaseType.Fwd)
            //     return new OneAxleTransferCase(nonObjectTransferCase, ctr);
            if (transferCaseData is OneAxleTransferCaseData oneAxleData)
                return new OneAxleTransferCase(oneAxleData, ctr);
            
            throw new NotImplementedException($"Transfer case type {transferCaseData.GetType()} is not implemented.");
        }
    }
}