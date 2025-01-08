using System;
using Entities.Vehicle.Configs.Comfort;
using Entities.Vehicle.Enums;
using Entities.Vehicle.Modules;
using UnityEngine.InputSystem;

namespace Entities.Vehicle.Submodules.Comfort
{
    public class ExteriorLightsModule : ElectricalModule
    {
        public LightsConfig Config => ComfortConfig.LightsConfig;
        private LightsModule LightsModule => GetModule<ComfortModule>().Lights;
        
        public ExteriorLightsModule(VehicleController ctr) : base(ctr) { }

        public override void Initialize()
        {
            base.Initialize();
            LightsModule.OnLightsStateChanged += OnLightsStateChanged;
            Controller.InputActions.Development.ForceBrakeLights.performed += TurnOnBrakeLights;
            OnLightsStateOff();
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            
            if (CurrentElectricityState >= ElectricityState.LowPowerMode && _brakeLights)
                Config.Taillights.TurnOnBrakeLights();
        }

        private bool _brakeLights;

        private void TurnOnBrakeLights(InputAction.CallbackContext ctx)
        {
            _brakeLights = !_brakeLights;
            if (CurrentElectricityState >= ElectricityState.LowPowerMode && _brakeLights)
            {
                Config.Taillights.TurnOnBrakeLights();
                return;
            }
            Config.Taillights.TurnOffBrakeLights();
        }

        private void OnLightsStateChanged(RuntimeLightsState newLightsState)
        {
            switch (newLightsState)
            {
                case RuntimeLightsState.Off:
                    OnLightsStateOff();
                    break;
                case RuntimeLightsState.Daylights:
                    OnLightsStateDaylights();
                    break;
                case RuntimeLightsState.Position:
                    OnLightsStatePositions();
                    break;
                case RuntimeLightsState.LowBeams:
                    OnLightsStateLowBeamLights();
                    break;
            }
        }

        private void OnLightsStateOff()
        {
            Config.Headlights.TurnOffAllLights();
            Config.Taillights.TurnOffAllLights();
        }

        private void OnLightsStateDaylights()
        {
            Config.Headlights.TurnOnDaylights();
            Config.Taillights.TurnOffLights();
        }

        private void OnLightsStatePositions()
        {
            Config.Headlights.TurnOnPositionLights();
            Config.Taillights.TurnOnPositionLights();
        }

        private void OnLightsStateLowBeamLights()
        {
            Config.Headlights.TurnOnLowBeamLights();
            Config.Taillights.TurnOnPositionLights();
        }
    }
}