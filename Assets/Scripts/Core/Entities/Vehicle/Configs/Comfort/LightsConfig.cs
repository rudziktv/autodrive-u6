using System;
using Core.Entities.Vehicle.Subentities.Lights;
using UnityEngine;

namespace Core.Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class LightsConfig
    {
        [Header("Main Lights")]
        [SerializeField] private HeadlightsController headlightsController;
        [SerializeField] private TaillightsController taillightsController;

        [Header("Exterior Lights")]
        [SerializeField] private LightController placeholder;
        
        [Header("Interior Lights")]
        [SerializeField] private LightController legLightController;
        [SerializeField] private LightController flReadLightController;
        [SerializeField] private LightController frReadLightController;
        [SerializeField] private LightController rlReadLightController;
        [SerializeField] private LightController rrReadLightController;

        [Header("Functionality")] [SerializeField]
        private bool lightSwitchIlluminatedOnIgnitionZeroPosition;

        [Header("Dusk Sensor")]
        [SerializeField] private bool duskSensor;
        [SerializeField] private ComputeShader duskSensorShader;
        [SerializeField] private Transform duskSensorTransform;
        [SerializeField] private float duskSensorThreshold = 1000f;
        [SerializeField] private float duskBacklightMultiplier = 1500f;
        [SerializeField] private Camera duskSensorCamera;

        public HeadlightsController Headlights => headlightsController;
        public TaillightsController Taillights => taillightsController;
        
        public LightController LegLight => legLightController;
        
        public LightController FlReadLight => flReadLightController;
        public LightController FrReadLight => frReadLightController;
        public LightController RlReadLight => rlReadLightController;
        public LightController RrReadLight => rrReadLightController;
        
        public bool LightSwitchIlluminatedOnIgnitionZero => lightSwitchIlluminatedOnIgnitionZeroPosition;
        
        public bool DuskSensor => duskSensor;
        public Transform DuskSensorTransform => duskSensorTransform;
        public float DuskSensorThreshold => duskSensorThreshold;
        public float DuskBacklightMultiplier => duskBacklightMultiplier;
        public Camera DuskSensorCamera => duskSensorCamera;
        public ComputeShader DuskSensorShader => duskSensorShader;
    }
}