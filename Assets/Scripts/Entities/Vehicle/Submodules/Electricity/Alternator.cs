using Entities.Vehicle.Configs.Electricity;
using Entities.Vehicle.Enums;
using Entities.Vehicle.Managers;
using Environment;
using UnityEngine;

namespace Entities.Vehicle.Submodules.Electricity
{
    public class Alternator : VehicleModule, IModuleConfig<AlternatorConfig>
    {
        public AlternatorConfig Config => VehicleConfigs.ElectricityConfig.AlternatorConfig;
        private Battery Battery => Controller.ElectricityManager.Battery;
        private ElectricityManager Electricity => Controller.ElectricityManager;
        public float CurrentTemperature => SimpleEnvironment.instance.currentTemperature;

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
    }
}