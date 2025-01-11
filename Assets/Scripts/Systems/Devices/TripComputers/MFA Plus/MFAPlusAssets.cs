using System;
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
        // [field: SerializeField] public VisualTreeAsset Type { get; set; }

        [SerializeField] private StyleSheet settingsStyle;
        
        public VisualTreeAsset MainScreen => mainScreen;
        public VisualTreeAsset DataSubpage => dataSubpage;
        public VisualTreeAsset SplashScreen => splashScreen;
        
        public StyleSheet SettingsStyle => settingsStyle;

        public VisualTreeAsset GetCarStatusComponentByLoad()
        {
            return UnityEngine.Resources.Load<VisualTreeAsset>(
                "Bundles/Cars/Volkswagen Golf Mk6/MFA Plus/Components/MFA Plus Car Indicator");
        }
    }
}