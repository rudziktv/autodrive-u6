using System;
using Core.Entities.Vehicle.Builders;
using Core.Entities.Vehicle.Data.Drivetrain.TransferCase;
using Core.Entities.Vehicle.Modules.Differential;

namespace Core.Entities.Vehicle.Modules.TransferCase
{
    public class OneAxleTransferCase : ITransferCase
    {
        public OneAxleTransferCaseData Data { get; }
        public VehicleController Controller { get; }

        public float CurrentRPM => Differential.CurrentRPM;

        public IDifferential Differential { get; }

        public OneAxleTransferCase(OneAxleTransferCaseData data, VehicleController ctr)
        {
            Data = data;
            Controller = ctr;

            if (data.axles.Length != Controller.VehicleConfigs.DrivetrainConfig.Axles.Length)
                throw new Exception("Axles count mismatch.");

            for (var i = 0; i < data.axles.Length; i++)
            {
                if (!data.axles[i].powered) continue;
                Differential = data.axles[i].differential.
                    BuildDifferential(Controller.VehicleConfigs.DrivetrainConfig.Axles[i], ctr);
                break;
            }
        }
        
        public void Initialize()
        {
            Differential.Initialize();
        }

        public void UpdateModule()
        {
            Differential.UpdateModule();
        }

        public void FixedUpdateModule()
        {
            Differential.FixedUpdateModule();
        }

        public void TransferTorque(float torque)
        {
            Differential.TransferTorque(torque);
        }
    }
}