using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Configs.Electricity;
using Core.Entities.Vehicle.Enums;

namespace Core.Entities.Vehicle.Modules
{
    public class ElectricalModule : VehicleModule
    {
        protected ComfortConfig ComfortConfig => VehicleConfigs.ComfortConfig;
        protected ElectricityConfig ElectricityConfig => VehicleConfigs.ElectricityConfig;
        protected ElectricityState CurrentElectricityState => ElectricityManager.CurrentElectricityState;

        public ElectricalModule(VehicleController ctr) : base(ctr)
        {
            ElectricityManager.OnElectricityStateChanged += OnElectricityStateChanged;
        }

        public virtual void OnElectricityStateChanged(ElectricityState newState)
        {
            switch (newState)
            {
                case ElectricityState.Off:
                    OnOffElectricity();
                    break;
                case ElectricityState.OnlyAccessories:
                    OnAccessoriesOnlyElectricity();
                    break;
                case ElectricityState.LowPowerMode:
                    OnLowPowerModeElectricity();
                    break;
                case ElectricityState.Ignition:
                    OnIgnitionElectricity();
                    break;
                case ElectricityState.Engine:
                    OnEngineElectricity();
                    break;
            }
        }

        public virtual void OnOffElectricity() { }
        public virtual void OnAccessoriesOnlyElectricity() { }
        public virtual void OnIgnitionElectricity() { }
        public virtual void OnLowPowerModeElectricity() { }
        public virtual void OnEngineElectricity() { }
    }
}