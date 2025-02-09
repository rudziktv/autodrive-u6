using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Modules.Engine;
using Core.Entities.Vehicle.Modules.EngineControl;
using Systems.Info;

namespace Core.Entities.Vehicle.Builders
{
    public static class EngineBuilder
    {
        public static EngineModule BuildEngine(this VehicleEngineData engineData, VehicleController ctr)
        {

            if (engineData is CombustionEngineData combustionEngine)
            {
                if (combustionEngine.fuelType == FuelTypeEnum.Gasoline)
                    return new GasolineEngineModule(combustionEngine, new GasolineECUModule(combustionEngine, ctr), ctr);
            }

            return new EngineModule(ctr);
        }
        
    }
}