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
        public bool IsBetweenBlinks { get; private set; }
        
        private BlinkersConfig Config => ComfortConfig.BlinkersConfig;
        
        private readonly BlinkerController _lights;
        private readonly IndicatorController _indicator;

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

        public void TurnOnBlinkerContinuous(bool isBetweenBlinks, float initialTime = 0)
        {
            IsBetweenBlinks = isBetweenBlinks;
            StopCoroutine(_blinkingRoutine);
            _blinkingRoutine = StartCoroutine(BlinkingCoroutine(
                initialTime != 0 ? initialTime : Timer.ElapsedTime, false));
        }

        public void TurnOnBlinker(float initialTime = 0f)
        {
            StopCoroutine(_blinkingRoutine);
            TurnOffBlinker();
            _blinkingRoutine = StartCoroutine(BlinkingCoroutine(
                initialTime != 0 ? initialTime : Timer.ElapsedTime, false));
        }

        private void TurnOffBlinker()
        {
            if (_blinkingRoutine != null)
                StopCoroutine(_blinkingRoutine);
            
            if (IsBetweenBlinks)
                Sounds.BlinkerOffEmitter.Play();
            IsBetweenBlinks = false;
            
            _lights.TurnOffBlinker();
            _indicator.SetIndicator(false);
        }
        
        public void TurnOffBlinkerAndResetTimer(bool playSound = true)
        {
            Timer.Reset();
            if (_blinkingRoutine != null)
                StopCoroutine(_blinkingRoutine);
            
            if (IsBetweenBlinks && playSound)
                Sounds.BlinkerOffEmitter.Play();
            IsBetweenBlinks = false;
            
            _lights.TurnOffBlinker();
            _indicator.SetIndicator(false);
        }
        
        private IEnumerator BlinkingCoroutine(float initialTimer, bool comfort)
        {
            Timer.Restart();
            var i = 0;
            var duration = Mathf.Clamp((IsBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration) - initialTimer, 0, float.MaxValue);
            var blinksCount = Config.ComfortBlinkerBlinks * 2;
            if (initialTimer > 0f)
            {
                yield return new WaitForSeconds(duration);
                IsBetweenBlinks = !IsBetweenBlinks;
                blinksCount += IsBetweenBlinks ? 1 : 0;
            }
            while (!comfort || i < blinksCount)
            {
                Timer.Restart();
                Blink();
                yield return new WaitForSeconds(IsBetweenBlinks ? Config.BreakerDuration : Config.BlinkDuration);
                IsBetweenBlinks = !IsBetweenBlinks;
                i++;
            }
            TurnOffBlinker();
            Timer.Reset();
        }

        private void Blink()
        {
            _lights.ApplyBlinker(!IsBetweenBlinks);
            _indicator.SetIndicator(!IsBetweenBlinks);
            if (IsBetweenBlinks)
                Sounds.BlinkerOnEmitter.Play();
            else
                Sounds.BlinkerOffEmitter.Play();
        }
    }
}