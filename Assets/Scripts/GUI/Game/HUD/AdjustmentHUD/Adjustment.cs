using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Game.HUD.AdjustmentHUD
{
    [RequireComponent(typeof(UIDocument))]
    public class Adjustment : MonoBehaviour
    {
        private UIDocument _document;
        
        private AdjustmentHUDController _adjustmentHUD;

        private void Start()
        {
            _document = GetComponent<UIDocument>();

            _adjustmentHUD = new(this, _document.rootVisualElement);
        }
    }
}