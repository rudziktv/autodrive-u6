using System.Collections.Generic;
using Core.Entities.Vehicle.Data.Status;

namespace Core.Helpers
{
    public class StateController
    {
        private Dictionary<string, string> _states = new();

        public StateController(VehicleStatusPair[] pairs = null)
        {
            if (pairs == null) return;
            foreach (var status in pairs)
            {
                _states.TryAdd(status.key, status.defaultValue);
            }
        }
        
        public delegate void StateChangedArgs(string key, string value);
        public event StateChangedArgs StateChanged;
        
        public void SetState(string key, string value)
        {
            _states[key] = value;
            StateChanged?.Invoke(key, value);
        }
        
        public string GetState(string key, string defaultValue = "")
        {
            return _states.GetValueOrDefault(key, defaultValue);
        }
    }
}