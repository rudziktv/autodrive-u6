using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Engine.Routine
{
    public class RoutineModule
    {
        public Dictionary<string, Routine> Routines { get; }
        private readonly MonoBehaviour _behaviour;

        public RoutineModule(MonoBehaviour behaviour)
        {
            _behaviour = behaviour;
            Routines = new();
        }

        public void StartRoutine(string name, IEnumerator enumerator)
        {
            var routine = new Routine(_behaviour, enumerator);
            if (Routines.ContainsKey(name))
                StopRoutine(name);
            Routines.Add(name, routine);
            routine.RoutineEnded += OnRoutineEnd;
            routine.StartRoutine();
            return;

            void OnRoutineEnd()
            {
                Routines[name].RoutineEnded -= OnRoutineEnd;
                Routines.Remove(name);
            }
        }

        public void StopRoutine(string name)
        {
            if (!Routines.TryGetValue(name, out var routine)) return;
            routine.StopRoutine();
            Routines.Remove(name);
        }

        public void StopAllRoutines()
        {
            foreach (var routines in Routines.Values)
            {
                routines.StopRoutine();
            }
        }
    }
}