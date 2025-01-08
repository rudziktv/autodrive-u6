using System.Collections.Generic;
using JetBrains.Annotations;

namespace Helpers
{
    public class StateController
    {
        private Dictionary<string, string> _states = new();
        
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