using System;
using System.Collections;
using UnityEngine;

namespace Core.Engine.Routine
{
    public class Routine
    {
        public event Action RoutineEnded;
        private readonly IEnumerator _wrappedEnumerator;
        private readonly MonoBehaviour _behaviour;
        private Coroutine _coroutine;

        public Routine(MonoBehaviour behaviour, IEnumerator enumerator)
        {
            _behaviour = behaviour;
            _wrappedEnumerator = WrapIEnumerator(enumerator);
        }
        
        private IEnumerator WrapIEnumerator(IEnumerator enumerator)
        {
            yield return enumerator;
            RoutineEnded?.Invoke();
        }

        public Coroutine StartRoutine()
            => _coroutine = _behaviour.StartCoroutine(_wrappedEnumerator);
        

        public void StopRoutine()
        {
            if (_coroutine != null)
                _behaviour.StopCoroutine(_coroutine);
            RoutineEnded?.Invoke();
        }
    }
}