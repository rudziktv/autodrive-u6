using System;
using Core.Components.Sounds;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus
{
    [Serializable]
    public class MFAPlusAssets
    {
        [SerializeField] private VisualTreeAsset mainScreen;
        [SerializeField] private VisualTreeAsset dataSubpage;
        [SerializeField] private VisualTreeAsset splashScreen;
        
        [field: SerializeField] public FMODCustomEmitter WarningEmitter { get; private set; }
        // [field: SerializeField] public VisualTreeAsset Type { get; set; }

        [SerializeField] private StyleSheet settingsStyle;
        
        public VisualTreeAsset MainScreen => mainScreen;
        public VisualTreeAsset DataSubpage => dataSubpage;
        public VisualTreeAsset SplashScreen => splashScreen;
        
        public StyleSheet SettingsStyle => settingsStyle;

        public VisualTreeAsset GetCarStatusComponentByLoad()
        {
            return Resources.Load<VisualTreeAsset>(
                "Devices/Trip Computers/MFA Plus/Components/MFA Plus Car Indicator");
        }
    }
}