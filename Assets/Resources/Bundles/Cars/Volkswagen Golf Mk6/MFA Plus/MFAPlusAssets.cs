using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    [Serializable]
    public class MFAPlusAssets
    {
        [SerializeField] private VisualTreeAsset mainScreen;
        [SerializeField] private VisualTreeAsset dataSubpage;
        
        public VisualTreeAsset MainScreen => mainScreen;
        public VisualTreeAsset DataSubpage => dataSubpage;
    }
}