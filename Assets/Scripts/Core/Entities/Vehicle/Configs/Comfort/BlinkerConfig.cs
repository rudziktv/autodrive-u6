using System;
using System.ComponentModel;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class BlinkerConfig
    {
        [Tooltip("Timer between blinker light up and light down.")]
        [field: SerializeField] public float BlinkDuration { get; private set; }
        [Tooltip("Timer between blinker light down and light up.")]
        [field: SerializeField] public float BreakerDuration { get; private set; }

        [field: SerializeField] public int ComfortBlinkerBlinks { get; private set; } = 3;
    }
}