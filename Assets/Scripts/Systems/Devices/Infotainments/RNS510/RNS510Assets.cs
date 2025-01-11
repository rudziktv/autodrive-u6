using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510
{
    [Serializable]
    public class RNS510Assets
    {
        [SerializeField] private EventReference eventReference;
        
        public EventReference EventReference => eventReference;
        
        public VisualTreeAsset GetMediaScreenByLoad()
        {
            return Resources.Load<VisualTreeAsset>(
                "Devices/Infotainments/RNS510/Screens/Media Screen");
        }

        public VisualTreeAsset GetRadioScreenByLoad()
        {
            return Resources.Load<VisualTreeAsset>(
                "Devices/Infotainments/RNS510/Screens/Radio Screen");
        }
    }
}