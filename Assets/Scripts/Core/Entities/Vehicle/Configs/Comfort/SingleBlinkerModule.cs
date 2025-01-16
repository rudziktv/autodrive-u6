using System.Collections;
using Core.Entities.Vehicle.Modules;
using Core.Entities.Vehicle.Subentities.Dashboard;
using Core.Entities.Vehicle.Subentities.Lights;
using Core.Helpers;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
{
    public class SingleBlinkerModule : ElectricalModule
    {
        public Timer Timer { get; private set; }
        
        private BlinkersConfig Config => ComfortConfig.BlinkersConfig;
        
        private readonly BlinkerController _lights;
        private readonly IndicatorController _indicator;
        
        private bool _isBetweenBlinks;
        private Coroutine _blinkingRoutine;

        public SingleBlinkerModule(VehicleController controller, BlinkerController lights, IndicatorController indicator) : base(controller)
        {
            Timer = new(controller);
            _lights = lights;
            _indicator = indicator;
        }

        public void TurnOnComfortBlinker()
        {
            StopCoroutine(_blinkingRoutine);
            _blinkingRoutine = StartCoroutine(BlinkingCoroutine(Timer.ElapsedTime, true));
        }

        public void TurnOnBlinker(float initialTime = 0f)
        {
            TurnOffBlinker();
            _blinkingRoutine = StartCoroutine(BlinkingCoroutine(
                initialTime != 0 ? initialTime : Timer.ElapsedTime, false));
        }

        private void TurnOffBlinker()
        {
            if (_blinkingRoutine != null)
                StopCoroutine(_blinkingRoutine);
            
            if (_isBetweenBlinks)
                Sounds.BlinkerOffEmitter.Play();
            _isBetweenBlinks = false;
            
            _lights.TurnOffBlinker();
            _indicator.SetIndicator(false);
        }
        
        public void TurnOffBlinkerAndResetTimer(bool playSound = true)
        {
            Timer.Reset();
            if (_blinkingRoutine != null)
                StopCoroutine(_blinkingRoutine);
            
            if (_isBetweenBlinks && playSound)
                Sounds.BlinkerOffEmitter.Play();
            _isBetweenBlinks = false;
            
            _lights.TurnOffBlinker();
            _indicator.SetIndicator(false);
        }
        
        private IEnumerator BlinkingCoroutine(float initialTimer, bool comfort)
        {
            Timer.Restart();
            var i = 0;
            var duration = Mathf.Clamp((_isBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration) - initialTimer, 0, float.MaxValue);
            var blinksCount = Config.ComfortBlinkerBlinks * 2;
            if (initialTimer > 0f)
            {
                yield return new WaitForSeconds(duration);
                _isBetweenBlinks = !_isBetweenBlinks;
                blinksCount += _isBetweenBlinks ? 1 : 0;
            }
            while (!comfort || i < blinksCount)
            {
                Timer.Restart();
                Blink();
                yield return new WaitForSeconds(_isBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration);
                _isBetweenBlinks = !_isBetweenBlinks;
                i++;
            }
            TurnOffBlinker();
            Timer.Reset();
        }

        private void Blink()
        {
            _lights.ApplyBlinker(!_isBetweenBlinks);
            _indicator.SetIndicator(!_isBetweenBlinks);
            if (_isBetweenBlinks)
                Sounds.BlinkerOnEmitter.Play();
            else
                Sounds.BlinkerOffEmitter.Play();
        }
    }
}