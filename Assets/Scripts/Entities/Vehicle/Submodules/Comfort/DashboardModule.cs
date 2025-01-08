using System;
using System.Collections;
using Entities.Vehicle.Configs.Comfort;
using Entities.Vehicle.Enums;
using Entities.Vehicle.Modules;
using UnityEngine;

namespace Entities.Vehicle.Submodules.Comfort
{
    public class DashboardModule : ElectricalModule
    {
        public DashboardConfig Config => VehicleConfigs.ComfortConfig.DashboardConfig;
        public IndicatorsConfig Indicators => Config.IndicatorsConfig;
        public LightsModule Lights => GetModule<ComfortModule>().Lights;

        public DashboardModule(VehicleController controller) : base(controller) { }
        
        private Coroutine _safetyCheckCoroutine;
        private Coroutine _airbagsCheckCoroutine;
        private Coroutine _selfCheckCoroutine;

        public override void Initialize()
        {
            base.Initialize();
            Lights.OnLightsStateChanged += OnLightsStateChanged;
            Lights.OnAutomaticLightsStateChanged += OnAutomaticLightsStateChanged;
        }

        private void OnAutomaticLightsStateChanged(bool isUsingAutomaticLights)
        {
            Indicators.AutomaticLightsIndicator?.SetIndicator(isUsingAutomaticLights);
        }

        private void OnLightsStateChanged(RuntimeLightsState newLightsState)
        {
            switch (newLightsState)
            {
                case RuntimeLightsState.Off:
                    Indicators.AutomaticLightsIndicator?.SetIndicator(false);
                    Indicators.PositionLightsIndicator?.SetIndicator(false);
                    break;
                case RuntimeLightsState.Daylights:
                    Indicators.PositionLightsIndicator?.SetIndicator(false);
                    break;
                case RuntimeLightsState.Position:
                    Indicators.AutomaticLightsIndicator?.SetIndicator(false);
                    Indicators.PositionLightsIndicator?.SetIndicator(true);
                    break;
                case RuntimeLightsState.LowBeams:
                    Indicators.PositionLightsIndicator?.SetIndicator(true);
                    break;
            }
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            Handle();
        }

        public override void OnAccessoriesOnlyElectricity()
        {
            base.OnAccessoriesOnlyElectricity();
            OnOffElectricity();
        }

        public override void OnOffElectricity()
        {
            base.OnOffElectricity();
            AllIndicatorsOff();
        }

        private void AllIndicatorsOff()
        {
            // stop coroutines
            if (_airbagsCheckCoroutine != null)
                Controller.StopCoroutine(_airbagsCheckCoroutine);
            if (_safetyCheckCoroutine != null)
                Controller.StopCoroutine(_safetyCheckCoroutine);
            if (_selfCheckCoroutine != null)
                Controller.StopCoroutine(_selfCheckCoroutine);
            
            // ignition indicators
            Indicators.BatteryIndicator.SetIndicator(false);
            Indicators.EcuIndicator.SetIndicator(false);
            Indicators.CheckEngineIndicator.SetIndicator(false);
            
            // self-check indicators
            SelfCheckIndicatorsOff();
            
            // safety system self-check indicators
            SafetySystemsIndicatorsOff();
            Indicators.AirbagIndicator.SetIndicator(false);
            
            // others
            Indicators.BlinkerLeftIndicator.SetIndicator(false);
            Indicators.BlinkerRightIndicator.SetIndicator(false);
            
            Indicators.ParkingBrakeIndicator.SetIndicator(false);
            Indicators.SeatbeltIndicator.SetIndicator(false);
            
            Indicators.AutomaticLightsIndicator?.SetIndicator(false);
            // Indicators.PositionLightsIndicator.SetIndicator(false);
        }

        public override void OnIgnitionElectricity()
        {
            base.OnIgnitionElectricity();
            Indicators.BatteryIndicator.SetIndicator(true);
            Indicators.EcuIndicator.SetIndicator(true);
            Indicators.CheckEngineIndicator.SetIndicator(true);
            
            
            SafetySystemsIndicatorsOn();
            _safetyCheckCoroutine = Controller.StartCoroutine(DelayedCoroutineWithEffect(2f, SafetySystemsIndicatorsOff));
            Indicators.AirbagIndicator.SetIndicator(true);
            _airbagsCheckCoroutine = Controller.StartCoroutine(DelayedCoroutineWithEffect(5f, () =>
            {
                Indicators.AirbagIndicator.SetIndicator(false);
            }));
            SelfCheckIndicatorsOn();
            _selfCheckCoroutine = Controller.StartCoroutine(DelayedCoroutineWithEffect(3f, SelfCheckIndicatorsOff));
        }

        private void SelfCheckIndicatorsOn()
        {
            Indicators.LowFuelIndicator.SetIndicator(true);
            Indicators.HotCoolantIndicator.SetIndicator(true);
            Indicators.BrakeIndicator.SetIndicator(true);
            Indicators.SteeringIndicator.SetIndicator(true);
        }

        private void SelfCheckIndicatorsOff()
        {
            Indicators.LowFuelIndicator.SetIndicator(false);
            Indicators.HotCoolantIndicator.SetIndicator(false);
            Indicators.BrakeIndicator.SetIndicator(false);
            Indicators.SteeringIndicator.SetIndicator(false);
        }

        private void SafetySystemsIndicatorsOn()
        {
            Indicators.AbsIndicator.SetIndicator(true);
            Indicators.AsrIndicator.SetIndicator(true);
            Indicators.AsrOffIndicator.SetIndicator(true);
            Indicators.TpmsIndicator.SetIndicator(true);
        }

        private void SafetySystemsIndicatorsOff()
        {
            Indicators.AbsIndicator.SetIndicator(false);
            Indicators.AsrIndicator.SetIndicator(false);
            Indicators.AsrOffIndicator.SetIndicator(false);
            Indicators.TpmsIndicator.SetIndicator(false);
        }

        private IEnumerator DelayedCoroutineWithEffect(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        private void Handle()
        {
            // switch (CurrentElectricityState)
            // {
            //     case ElectricityState.Off:
            //     case ElectricityState.OnlyAccessories:
            //         Config.DashboardIllumination.IlluminateGauges(Controller, 0f);
            //         Config.DashboardIllumination.IlluminateNeedles(Controller, 0f);
            //         break;
            //     case ElectricityState.LowPowerMode:
            //     case ElectricityState.Ignition:
            //     case ElectricityState.Engine:
            //         Config.DashboardIllumination.IlluminateGauges(Controller, 0f);
            //         Config.DashboardIllumination.IlluminateNeedles(Controller, 1f);
            //         break;
            //     // default:
            //     //     break;
            // }
        }
    }
}