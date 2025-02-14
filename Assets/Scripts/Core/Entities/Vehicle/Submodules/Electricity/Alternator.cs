using Core.Entities.Vehicle.Configs.Electricity;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Managers;
using Core.Entities.Vehicle.Modules;
using Systems.Environment;
using UnityEngine;

namespace Core.Entities.Vehicle.Submodules.Electricity
{
    public class Alternator : ComfortModule, IModuleConfig<AlternatorConfig>
    {
        public AlternatorConfig Config => VehicleConfigs.ElectricityConfig.AlternatorConfig;
        private Battery Battery => Controller.ElectricityManager.Battery;
        private ElectricityManager Electricity => Controller.ElectricityManager;
        public float CurrentTemperature => SimpleEnvironment.instance.AmbientTemperatureCelsius;

        public float OutputVoltage { get; private set; } = 0f;

        public Alternator(VehicleController controller) : base (controller) { }

        public override void UpdateModule()
        {
            base.UpdateModule();
            var compensatedMaxVoltage = Config.MaxVoltage -
                                        (25f - CurrentTemperature) * Config.TemperatureCompensation;
            if (Electricity.CurrentElectricityState != ElectricityState.Engine)
            {
                OutputVoltage = 0f;
                return;
            }
            OutputVoltage = Mathf.Clamp(Battery.CurrentVoltage + 0.1f, Config.MinVoltage,
                compensatedMaxVoltage);
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);

            Dashboard.Indicators.BatteryIndicator.SetIndicator(newState is ElectricityState.Ignition
                or ElectricityState.LowPowerMode);
        }
    }
}