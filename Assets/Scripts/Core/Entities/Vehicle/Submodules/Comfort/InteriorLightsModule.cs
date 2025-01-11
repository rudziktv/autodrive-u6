using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules;
using UnityEngine;

namespace Core.Entities.Vehicle.Submodules.Comfort
{
    public class InteriorLightsModule : ElectricalModule
    {
        public LightsConfig Config => ComfortConfig.LightsConfig;
        
        public InteriorLightsModule(VehicleController controller) : base(controller) { }

        public override void Initialize()
        {
            base.Initialize();
            Interactions.LightSwitch.StateChanged += OnLightSwitchStateChanged;
            OnLightSwitchStateChanged(Interactions.LightSwitch.CurrentLightSwitchState);
        }

        public void DuskSensorUpdate(float intensity)
        {
            intensity = Mathf.Clamp(intensity, 0.01f, float.MaxValue);
            var backlight = Mathf.Clamp01(1f / intensity * Config.DuskBacklightMultiplier);
            var gauges = Mathf.Clamp(backlight, 0.2f, 1f);
            DashboardIllumination(gauges, backlight, 1, backlight);
        }

        public override void OnOffElectricity()
        {
            base.OnOffElectricity();
            DashboardIlluminationTurnOff();
        }

        public override void OnAccessoriesOnlyElectricity()
        {
            base.OnAccessoriesOnlyElectricity();
            if (Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Off)
            {
                DashboardIllumination(0f, 0f, 1f, 0f);
            }
            
            // TEMPORARY
            TurnOnAllReadLights();
        }

        public override void OnIgnitionElectricity()
        {
            base.OnIgnitionElectricity();
            TurnOffAllReadLights();
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            switch (newState)
            {
                case ElectricityState.Off:
                case ElectricityState.OnlyAccessories:
                    ComfortConfig.DashboardConfig.IndicatorsConfig.AutomaticLightsIndicator?.SetIndicator(false);
                    break;
                case ElectricityState.LowPowerMode:
                case ElectricityState.Ignition:
                case ElectricityState.Engine:
                    OnLightSwitchStateChanged(Interactions.LightSwitch.CurrentLightSwitchState);
                    break;
            }
        }

        private void OnLightSwitchStateChanged(LightSwitchState lightSwitchState)
        {
            switch (lightSwitchState)
            {
                // case LightSwitchState.Off:
                //     break;
                case LightSwitchState.Auto:
                    OnLightSwitchAuto();
                    break;
                case LightSwitchState.Positions:
                    OnLightSwitchPositionLights();
                    break;
                case LightSwitchState.LowBeams:
                    OnLightSwitchLowBeamLights();
                    break;
                default:
                    OnLightSwitchOff();
                    break;
            }
        }

        private void OnLightSwitchOff()
        {
            if (CurrentElectricityState < ElectricityState.Ignition)
            {
                DashboardIlluminationTurnOff();
                return;
            }
            DashboardIllumination(0f, 0f, 1f, Config.LightSwitchIlluminatedOnIgnitionZero ? 1f : 0f);
        }
        
        private void OnLightSwitchAuto()
        {
            if (CurrentElectricityState < ElectricityState.Ignition)
            {
                DashboardIlluminationTurnOff();
                return;
            }
            // DuskSensorUpdate(1f);
            DashboardIllumination(0.2f, 0f, 1f, 0f);
        }

        private void OnLightSwitchPositionLights()
        {
            DashboardIllumination(1f, 1f, 1f, 1f);
        }

        private void OnLightSwitchLowBeamLights()
        {
            if (CurrentElectricityState < ElectricityState.Ignition)
            {
                DashboardIlluminationTurnOff();
                return;
            }
            DashboardIllumination(1f, 1f, 1f, 1f);
        }
        
        // private void 

        public void TurnOnAllReadLights()
        {
            Config.FlReadLight?.TurnOnLight();
            Config.FrReadLight?.TurnOnLight();
            Config.RlReadLight?.TurnOnLight();
            Config.RrReadLight?.TurnOnLight();
        }

        public void TurnOffAllReadLights()
        {
            Config.FlReadLight?.TurnOffLight();
            Config.FrReadLight?.TurnOffLight();
            Config.RrReadLight?.TurnOffLight();
            Config.RlReadLight?.TurnOffLight();
        }
        
        public void DashboardIlluminationTurnOff()
        {
            DashboardIllumination(0f, 0f, 0f, 0f);
        }
        
        public void DashboardIllumination(float gauges, float buttons, float needles, float lightSwitch)
        {
            ComfortConfig.DashboardConfig.DashboardIllumination.IlluminateGauges(Controller, gauges);
            ComfortConfig.DashboardConfig.DashboardIllumination.IlluminateButtons(Controller, buttons);
            ComfortConfig.DashboardConfig.DashboardIllumination.IlluminateNeedles(Controller, needles);
            ComfortConfig.DashboardConfig.DashboardIllumination.IlluminateLightSwitch(Controller, lightSwitch);
        }
    }
}