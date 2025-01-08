using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Entities.Vehicle.Subentities.Lights
{
    [Serializable]
    public class LightIntensityPair
    {
        [SerializeField] private string name;
        [FormerlySerializedAs("lumen")] [SerializeField] private float intensity;
        [SerializeField] private LightUnit lightUnit = LightUnit.Lumen;
        [SerializeField] private float range;
        [SerializeField] private Light light;
        
        public float Intensity => intensity;
        public float Range => range;
        public Light Light => light;
        public LightUnit LightUnit => lightUnit;
        public string Name => name;
    }
}