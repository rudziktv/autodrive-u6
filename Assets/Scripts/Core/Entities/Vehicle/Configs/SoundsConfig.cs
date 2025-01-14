using System;
using Core.Components.Sounds;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs
{
    [Serializable]
    public class SoundsConfig
    {
        [field: SerializeField] public FMODCustomEmitter BlinkerOnEmitter { get; private set; }
        [field: SerializeField] public FMODCustomEmitter BlinkerOffEmitter { get; private set; }
    }
}