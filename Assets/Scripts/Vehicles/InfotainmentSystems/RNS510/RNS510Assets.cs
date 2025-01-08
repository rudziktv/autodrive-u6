using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vehicles.InfotainmentSystems.RNS510
{
    [Serializable]
    public class RNS510Assets
    {
        [SerializeField] private EventReference eventReference;
        
        public EventReference EventReference => eventReference;
        
        public VisualTreeAsset GetMediaScreenByLoad()
        {
            return UnityEngine.Resources.Load<VisualTreeAsset>(
                "Bundles/Cars/Volkswagen Golf Mk6/RNS510/Screens/Media Screen");
        }

        public VisualTreeAsset GetRadioScreenByLoad()
        {
            return UnityEngine.Resources.Load<VisualTreeAsset>(
                "Bundles/Cars/Volkswagen Golf Mk6/RNS510/Screens/Radio Screen");
        }
    }
}