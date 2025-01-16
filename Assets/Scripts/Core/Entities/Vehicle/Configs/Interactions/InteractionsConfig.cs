using System;
using Core.Entities.Vehicle.Interactions;
using Systems.Interactions;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Interactions
{
    [Serializable]
    public class InteractionsConfig
    {
        [SerializeField] private LightSwitchInteractable lightSwitch;
        [SerializeField] private KeyIgnitionInteractable keyIgnition;
        [field: SerializeField] public HazardsInteractable Hazards { get; private set; }
        [field: SerializeField] public BlinkerStickInteractable BlinkerStick { get; private set; }
        
        
        public LightSwitchInteractable LightSwitch => lightSwitch;
        public KeyIgnitionInteractable KeyIgnition => keyIgnition;
    }
}