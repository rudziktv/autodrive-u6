using System;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules;
using JetBrains.Annotations;

namespace Core.Entities.Vehicle.Submodules.Comfort
{
    public class LightsModule : ElectricalModule
    {
        public RuntimeLightsState CurrentLightsState { get; private set; }
        public bool DoesAutomaticLightsUsed { get; private set; }
        
        public InteriorLightsModule InteriorLights { get; private set; }
        public ExteriorLightsModule ExteriorLights { get; private set; }
        [CanBeNull] public DuskSensorModule DuskSensor { get; private set; }
        
        public event LightStateChangedArgs OnLightsStateChanged;
        public event Action<bool> OnAutomaticLightsStateChanged;
        public delegate void LightStateChangedArgs(RuntimeLightsState newLightsState);
        
        public LightsConfig Config => VehicleConfigs.ComfortConfig.LightsConfig;

        public LightsModule(VehicleController controller) : base(controller)
        {
            InteriorLights = new(Controller);
            ExteriorLights = new(Controller);
            
            if (Config.DuskSensor)
                DuskSensor = new(Controller);
        }

        public override void Initialize()
        {
            base.Initialize();
            ComfortConfig.DashboardConfig.DashboardIllumination.FindIlluminationItemsByTag(Components.VehicleRoot);
            Interactions.LightSwitch.OnStateChanged += OnLightSwitchStateChanged;
            OnLightSwitchStateChanged(Interactions.LightSwitch.CurrentLightSwitchState);
            
            // initialize internal modules
            InteriorLights.Initialize();
            ExteriorLights.Initialize();
            DuskSensor?.Initialize();
        }
        public override void UpdateModule()
        {
            base.UpdateModule();
            InteriorLights.UpdateModule();
            ExteriorLights.UpdateModule();
            if (CurrentElectricityState is ElectricityState.Engine or ElectricityState.Ignition &&
                Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Auto)
                DuskSensor?.UpdateModule();
        }
        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            InteriorLights.FixedUpdateModule();
            ExteriorLights.FixedUpdateModule();
            if (CurrentElectricityState is ElectricityState.Engine or ElectricityState.Ignition &&
                Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Auto)
                DuskSensor?.FixedUpdateModule();
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);

            if (Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Positions)
            {
                OnLightSwitchPositionLights();
                return;
            }
            
            if (Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Auto && CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine)
                DuskSensor?.StartDuskSensor();
            else
                DuskSensor?.StopDuskSensor();
            
            switch (newState)
            {
                case ElectricityState.Off:
                    OnLightSwitchOff();
                // case ElectricityState.OnlyAccessories:
                    // Config.Headlights.TurnOffAllLights();
                    break;
                case ElectricityState.OnlyAccessories:
                    break;
                default:
                    OnLightSwitchStateChanged(Interactions.LightSwitch.CurrentLightSwitchState);
                    break;
            }
        }

        public override void OnAccessoriesOnlyElectricity()
        {
            base.OnAccessoriesOnlyElectricity();
            if (Interactions.LightSwitch.CurrentLightSwitchState is LightSwitchState.Auto or LightSwitchState.LowBeams)
            {
                InvokeEventIfStateChanged(RuntimeLightsState.Position);
                return;
            }
            OnLightSwitchOff();
        }

        public void OnDuskDetected(RuntimeLightsState newState)
        {
            InvokeEventIfStateChanged(newState);
        }

        private void OnLightSwitchStateChanged(LightSwitchState lightState)
        {
            if (lightState == LightSwitchState.Auto && CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine)
                DuskSensor?.StartDuskSensor();
            else
                DuskSensor?.StopDuskSensor();
            
            switch (lightState)
            {
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
            // Config.Headlights.TurnOffAllLights();
            if (CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine)
            {
                InvokeEventIfStateChanged(RuntimeLightsState.Daylights);
                return;
            }
            InvokeEventIfStateChanged(RuntimeLightsState.Off);
        }

        private void OnLightSwitchAuto()
        {
            if (DuskSensor == null) return;
            if (CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine)
            {
                InvokeEventIfStateChanged(DuskSensor.DuskSense());
                DoesAutomaticLightsUsed = true;
                OnAutomaticLightsStateChanged?.Invoke(true);
                return;
            }
            OnLightSwitchOff();
        }
        
        private void OnLightSwitchPositionLights()
        {
            InvokeEventIfStateChanged(RuntimeLightsState.Position);
        }

        private void OnLightSwitchLowBeamLights()
        {
            if (CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine)
                InvokeEventIfStateChanged(RuntimeLightsState.LowBeams);
            else
                OnLightSwitchOff();
        }

        private void InvokeEventIfStateChanged(RuntimeLightsState newState)
        {
            if (DoesAutomaticLightsUsed !=
                (CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine &&
                 Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Auto))
            {
                DoesAutomaticLightsUsed =
                    CurrentElectricityState is ElectricityState.Ignition or ElectricityState.Engine &&
                    Interactions.LightSwitch.CurrentLightSwitchState == LightSwitchState.Auto;
                OnAutomaticLightsStateChanged?.Invoke(DoesAutomaticLightsUsed);
            }
            
            if (newState == CurrentLightsState)
                return;
            CurrentLightsState = newState;
            OnLightsStateChanged?.Invoke(newState);
        }
    }
}