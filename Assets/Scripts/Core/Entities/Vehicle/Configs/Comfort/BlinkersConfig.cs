using System;
using System.ComponentModel;
using Core.Entities.Vehicle.Subentities.Lights;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class BlinkersConfig
    {
        [Tooltip("Timer between blinker light up and light down.")]
        [field: SerializeField] public float BlinkDuration { get; private set; }
        [Tooltip("Timer between blinker light down and light up.")]
        [field: SerializeField] public float BreakerDuration { get; private set; }

        [field: SerializeField] public int ComfortBlinkerBlinks { get; private set; } = 3;
        
        [Header("Blinker References")]
        [field: SerializeField] public BlinkerController LeftBlinkerController { get; private set; }
        [field: SerializeField] public BlinkerController RightBlinkerController { get; private set; }
    }
}