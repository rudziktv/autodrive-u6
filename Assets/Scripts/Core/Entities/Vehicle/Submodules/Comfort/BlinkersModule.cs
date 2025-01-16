using System.Collections;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules;
using Core.Entities.Vehicle.Subentities.Dashboard;
using Core.Helpers;
using UnityEngine;

namespace Core.Entities.Vehicle.Submodules.Comfort
{
    public class BlinkersModule : ElectricalModule
    {
        private BlinkersConfig Config => ComfortConfig.BlinkersConfig;
        private IndicatorsConfig Indicators => ComfortConfig.DashboardConfig.IndicatorsConfig;
        private IndicatorController LeftBlinkerIndicator => Indicators.LeftBlinkerIndicator;
        private IndicatorController RightBlinkerIndicator => Indicators.RightBlinkerIndicator;

        private bool _hazards;
        private int _currentBlinkerSide;

        public SingleBlinkerModule LeftBlinker { get; private set; }
        public SingleBlinkerModule RightBlinker { get; private set; }
        
        public BlinkersModule(VehicleController ctr) : base(ctr) { }

        public override void Initialize()
        {
            base.Initialize();
            LeftBlinker = new(Controller, Config.LeftBlinkerController, LeftBlinkerIndicator);
            RightBlinker = new(Controller, Config.RightBlinkerController, RightBlinkerIndicator);
            
            Interactions.BlinkerStick.BlinkerStickStateChanged += OnBlinkerStickStateChanged;
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            
            if (newState < ElectricityState.LowPowerMode &&
                Interactions.BlinkerStick.CurrentBlinkerState == BlinkerStickState.Zero)
                _currentBlinkerSide = 0;
            if (newState < ElectricityState.LowPowerMode)
                TurnOffAllBlinkers();
            else if (_currentBlinkerSide != 0)
                TurnOnBlinker(_currentBlinkerSide);
        }

        private void OnBlinkerStickStateChanged(BlinkerStickState stick)
        {
            if (stick is BlinkerStickState.LeftComfort or BlinkerStickState.RightComfort
                && CurrentElectricityState < ElectricityState.LowPowerMode)
            {
                TurnOffAllBlinkers();
                return;
            }
                
            if (CurrentElectricityState < ElectricityState.LowPowerMode &&
                stick is not (BlinkerStickState.LeftComfort or BlinkerStickState.RightComfort))
            {
                _currentBlinkerSide = Mathf.Clamp((int)stick, -1, 1);
                return;
            }
            
            switch (stick)
            {
                case BlinkerStickState.LeftComfort:
                    TurnOnComfortBlinker(-1);
                    break;
                case BlinkerStickState.Left:
                    TurnOnBlinker(-1);
                    break;
                case BlinkerStickState.Zero:
                    TurnOffAllBlinkers();
                    _currentBlinkerSide = 0;
                    break;
                case BlinkerStickState.Right:
                    TurnOnBlinker(1);
                    break;
                case BlinkerStickState.RightComfort:
                    TurnOnComfortBlinker(1);
                    break;
            }
        }
        
        private void TurnOnComfortBlinker(int side)
        {
            _currentBlinkerSide = side;
            switch (side)
            {
                case 1:
                    TurnOnComfortBlinker(RightBlinker, LeftBlinker);
                    break;
                case -1:
                    TurnOnComfortBlinker(LeftBlinker, RightBlinker);
                    break;
            }
        }

        private void TurnOnComfortBlinker(SingleBlinkerModule activeBlinker, SingleBlinkerModule inactiveBlinker)
        {
            inactiveBlinker.TurnOffBlinkerAndResetTimer();
            activeBlinker.TurnOnComfortBlinker();
        }

        private void TurnOnBlinker(int side)
        {
            _currentBlinkerSide = side;
            switch (side)
            {
                case 1:
                    TurnOnBlinker(RightBlinker, LeftBlinker);
                    break;
                case -1:
                    TurnOnBlinker(LeftBlinker, RightBlinker);
                    break;
            }
        }

        private void TurnOnBlinker(SingleBlinkerModule activeBlinker, SingleBlinkerModule inactiveBlinker)
        {
            inactiveBlinker.TurnOffBlinkerAndResetTimer();
            activeBlinker.TurnOnBlinker();
        }

        private void TurnOffAllBlinkers()
        {
            LeftBlinker.TurnOffBlinkerAndResetTimer();
            RightBlinker.TurnOffBlinkerAndResetTimer();
        }
    }
}