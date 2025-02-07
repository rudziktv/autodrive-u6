using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Modules.Engine;
using Systems.Info;

namespace Core.Entities.Vehicle.Builders
{
    public static class EngineBuilder
    {
        public static EngineModule BuildEngine(VehicleEngineData engineData, VehicleController ctr)
        {

            if (engineData is CombustionEngineData combustionEngine)
            {
                if (combustionEngine.fuelType == FuelTypeEnum.Gasoline)
                    return new GasolineEngineModule(combustionEngine, ctr);
            }

            return new EngineModule(ctr);
        }
        
    }
}