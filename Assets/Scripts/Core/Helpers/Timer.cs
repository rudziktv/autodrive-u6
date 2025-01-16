using System.Collections;
using UnityEngine;

namespace Core.Helpers
{
    public class Timer
    {
        private MonoBehaviour _mono;

        public float ElapsedTime { get; private set; }
        private Coroutine _routine;

        public Timer(MonoBehaviour monoBehaviour)
        {
            _mono = monoBehaviour;
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            Stop();
            _routine = _mono.StartCoroutine(TimerRoutine());
        }

        /// <summary>
        /// Stops timer and resets ElapsedTime
        /// </summary>
        public void Reset()
        {
            Stop();
            ElapsedTime = 0;
        }

        /// <summary>
        /// Stops and reset timer,
        /// then starts it again
        /// </summary>
        public void Restart()
        {
            Reset();
            Start();
        }

        public void Stop()
        {
            if (_routine != null)
                _mono.StopCoroutine(_routine);
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                yield return null;
                ElapsedTime += Time.deltaTime;
            }
        }
    }
}