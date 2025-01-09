using System;
using Core.Entities.Vehicle.Interactions;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Interactions
{
    [Serializable]
    public class InteractionsConfig
    {
        [SerializeField] private LightSwitchInteractable lightSwitch;
        [SerializeField] private KeyIgnitionInteractable keyIgnition;
        
        
        public LightSwitchInteractable LightSwitch => lightSwitch;
        public KeyIgnitionInteractable KeyIgnition => keyIgnition;
    }
}