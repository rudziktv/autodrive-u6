using System.Collections;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Interactions;
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
        private HazardsInteractable Hazards => Interactions.Hazards;

        // private bool _hazards;
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
            Hazards.HazardsStateChanged += OnHazardsStateChanged;
        }

        private void OnHazardsStateChanged(bool hazardsActive)
        {
            if (hazardsActive)
                TurnOnHazards();
            else
                TurnOffHazards();
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            
            if (newState < ElectricityState.LowPowerMode &&
                Interactions.BlinkerStick.CurrentBlinkerState == BlinkerStickState.Zero)
                _currentBlinkerSide = 0;
            if (Hazards.CurrentHazardsState &&
                _currentBlinkerSide == 0)
                return;
            if (newState < ElectricityState.LowPowerMode)
                TurnOffAllBlinkers();
            else if (_currentBlinkerSide != 0)
                TurnOnBlinker(_currentBlinkerSide);
        }

        private void OnBlinkerStickStateChanged(BlinkerStickState stick)
        {
            if (Hazards.CurrentHazardsState && stick is not 
                    (BlinkerStickState.Left or BlinkerStickState.Right))
            {
                var side = _currentBlinkerSide;
                _currentBlinkerSide = 0;
                TurnOnHazards(side);
                return;
            }
            
            if (stick is BlinkerStickState.LeftComfort or BlinkerStickState.RightComfort
                && CurrentElectricityState < ElectricityState.LowPowerMode)
            {
                _currentBlinkerSide = 0;
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

        private void TurnOnHazards(int side = 0)
        {
            // _hazards = true;
            if (_currentBlinkerSide != 0) return;
            TurnOnAllBlinkers(side);
        }

        private void TurnOffHazards()
        {
            // _hazards = false;
            if (_currentBlinkerSide == 0 ||
                CurrentElectricityState < ElectricityState.LowPowerMode)
            {
                TurnOffAllBlinkers();
                return;
            }
            if (_currentBlinkerSide != 0)
                TurnOnBlinkerContinuous(_currentBlinkerSide);
        }
        
        private void TurnOnComfortBlinker(int side)
        {
            // _currentBlinkerSide = side;
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
        
        private void TurnOnBlinkerContinuous(int side)
        {
            _currentBlinkerSide = side;
            switch (side)
            {
                case 1:
                    TurnOnBlinkerContinuous(RightBlinker, LeftBlinker);
                    break;
                case -1:
                    TurnOnBlinkerContinuous(LeftBlinker, RightBlinker);
                    break;
            }
        }

        private void TurnOnAllBlinkers(int side = 0)
        {
            // var initialTime = LeftBlinker.Timer.ElapsedTime != 0 ? LeftBlinker.Timer.ElapsedTime : RightBlinker.Timer.ElapsedTime;
            var timer = side == -1 ? LeftBlinker.Timer.ElapsedTime : RightBlinker.Timer.ElapsedTime;
            var isBetween = side == -1 ? LeftBlinker.IsBetweenBlinks : RightBlinker.IsBetweenBlinks;
            TurnOffAllBlinkers(false);
            LeftBlinker.TurnOnBlinkerContinuous(isBetween, timer);
            RightBlinker.TurnOnBlinkerContinuous(isBetween, timer);
        }
        
        private void TurnOnComfortBlinker(SingleBlinkerModule activeBlinker, SingleBlinkerModule inactiveBlinker)
        {
            inactiveBlinker.TurnOffBlinkerAndResetTimer();
            activeBlinker.TurnOnComfortBlinker();
        }

        private void TurnOnBlinker(SingleBlinkerModule activeBlinker, SingleBlinkerModule inactiveBlinker)
        {
            inactiveBlinker.TurnOffBlinkerAndResetTimer();
            activeBlinker.TurnOnBlinker();
        }
        
        private void TurnOnBlinkerContinuous(SingleBlinkerModule activeBlinker, SingleBlinkerModule inactiveBlinker)
        {
            inactiveBlinker.TurnOffBlinkerAndResetTimer();
            activeBlinker.TurnOnBlinkerContinuous(activeBlinker.IsBetweenBlinks);
        }

        private void TurnOffAllBlinkers(bool playSound = true)
        {
            LeftBlinker.TurnOffBlinkerAndResetTimer(playSound);
            RightBlinker.TurnOffBlinkerAndResetTimer(playSound);
        }
    }
}