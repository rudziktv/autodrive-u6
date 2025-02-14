using System;
using Core.Entities.Vehicle.Configs.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Differential;
using Core.Entities.Vehicle.Modules.Differential;

namespace Core.Entities.Vehicle.Builders
{
    public static class DifferentialBuilder
    {
        public static IDifferential BuildDifferential(this DifferentialData differentialData, AxleConfig axle, VehicleController ctr)
        {
            if (differentialData is OpenDifferentialData openDiffData)
                return new OpenDifferential(openDiffData, axle, ctr);

            throw new NotImplementedException($"Cannot build differential of type {differentialData.GetType()}. Refer to documentation for supported types.");
        }
    }
}