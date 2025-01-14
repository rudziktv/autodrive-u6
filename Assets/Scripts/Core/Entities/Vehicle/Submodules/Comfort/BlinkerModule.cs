using System;
using System.Collections;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules;
using Core.Entities.Vehicle.Subentities.Dashboard;
using UnityEngine;

namespace Core.Entities.Vehicle.Submodules.Comfort
{
    public class BlinkerModule : ElectricalModule
    {
        private BlinkerConfig Config => ComfortConfig.BlinkerConfig;
        private IndicatorsConfig Indicators => ComfortConfig.DashboardConfig.IndicatorsConfig;
        private IndicatorController LeftBlinkerIndicator => Indicators.LeftBlinkerIndicator;
        private IndicatorController RightBlinkerIndicator => Indicators.RightBlinkerIndicator;

        private int _currentBlinkerSide;
        private bool _isBetweenBlinks;
        private bool _hazards;
        
        private Coroutine _blinkerCoroutine;
        
        public BlinkerModule(VehicleController ctr) : base(ctr) { }

        public override void Initialize()
        {
            base.Initialize();
            
            Interactions.BlinkerStick.BlinkerStickStateChanged += OnBlinkerStickStateChanged;
        }

        public override void OnElectricityStateChanged(ElectricityState newState)
        {
            base.OnElectricityStateChanged(newState);
            
            if (_hazards) return;

            if (newState < ElectricityState.LowPowerMode &&
                Interactions.BlinkerStick.CurrentBlinkerState == BlinkerStickState.Zero)
                _currentBlinkerSide = 0;
            if (newState < ElectricityState.LowPowerMode)
                TurnOffBlinker();
            else if (_currentBlinkerSide != 0)
                TurnOnBlinker(_currentBlinkerSide);

            // switch (expression)
            // {
            //     
            // }
        }

        private void OnBlinkerStickStateChanged(BlinkerStickState stick)
        {
            if (stick is BlinkerStickState.LeftComfort or BlinkerStickState.RightComfort
                && CurrentElectricityState < ElectricityState.LowPowerMode)
            {
                TurnOffBlinker();
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
                    TurnOffBlinker();
                    _currentBlinkerSide = 0;
                    break;
                case BlinkerStickState.Right:
                    TurnOnBlinker(1);
                    break;
                case BlinkerStickState.RightComfort:
                    TurnOnComfortBlinker(1);
                    break;
            }
            // throw new System.NotImplementedException();
        }

        private void TurnOnComfortBlinker(int side)
        {
            if (_currentBlinkerSide != side)
                TurnOffBlinker();
            _currentBlinkerSide = side;
            if (_hazards) return;
            _blinkerCoroutine = StartCoroutine(ComfortBlinkerCoroutine());
        }

        private void TurnOnBlinker(int side)
        {
            if (_currentBlinkerSide != side)
                TurnOffBlinker();
            _currentBlinkerSide = side;
            if (_hazards) return;
            _blinkerCoroutine = StartCoroutine(BlinkerCoroutine());
        }

        private void TurnOffBlinker()
        {
            if (_hazards) return;
            if (_blinkerCoroutine != null)
                StopCoroutine(_blinkerCoroutine);
            
            LeftBlinkerIndicator.SetIndicator(false);
            RightBlinkerIndicator.SetIndicator(false);
            
            if (_isBetweenBlinks)
                Sounds.BlinkerOffEmitter.Play();
            _isBetweenBlinks = false;
        }

        private IEnumerator BlinkerCoroutine()
        {
            while (_currentBlinkerSide != 0)
            {
                Blink();
                _isBetweenBlinks = !_isBetweenBlinks;
                yield return new WaitForSeconds(_isBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration);
            }
        }
        
        private IEnumerator ComfortBlinkerCoroutine()
        {
            var i = 0;
            while (i < Config.ComfortBlinkerBlinks)
            {
                Blink();
                _isBetweenBlinks = !_isBetweenBlinks;
                yield return new WaitForSeconds(_isBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration);
                Blink();
                _isBetweenBlinks = !_isBetweenBlinks;
                yield return new WaitForSeconds(_isBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration);
                i++;
            }
            TurnOffBlinker();
        }

        private void Blink()
        {
            var blinkIndicator = _currentBlinkerSide == 1 ? RightBlinkerIndicator : LeftBlinkerIndicator;
            blinkIndicator.SetIndicator(!_isBetweenBlinks);
            if (_isBetweenBlinks)
                Sounds.BlinkerOnEmitter.Play();
            else
                Sounds.BlinkerOffEmitter.Play();
        }
    }
}