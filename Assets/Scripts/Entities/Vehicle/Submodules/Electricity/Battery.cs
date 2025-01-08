using Entities.Vehicle.Configs.Electricity;
using Environment;
using UnityEngine;

namespace Entities.Vehicle.Submodules.Electricity
{
    public class Battery : VehicleModule, IModuleConfig<BatteryConfig>
    {
        public BatteryConfig Config => VehicleConfigs.ElectricityConfig.BatteryConfig;
        public float CurrentTemperature => SimpleEnvironment.instance.currentTemperature;

        public Battery(VehicleController controller) : base(controller)
        {
            ChargeLevel = Config.InitialChargeLevel;
        }

        public float ChargeLevel { get; private set; }
        public float CurrentVoltage { get; private set; }
            

        public override void UpdateModule()
        {
            base.UpdateModule();
            var temperatureCompensation = (25f - CurrentTemperature) * 0.002f * 6;
            CurrentVoltage = 12.6f + 0.2f * (ChargeLevel / Config.MaxCapacity) - temperatureCompensation;
        }

        public void LoadOnBattery(float load)
        {
            ChargeLevel += load;
            ChargeLevel = Mathf.Clamp(ChargeLevel, 0f, Config.MaxCapacity);
        }
    }
}