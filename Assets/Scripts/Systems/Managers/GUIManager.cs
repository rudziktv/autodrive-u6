using System;
using GUI.Game.CircularMenu;
using GUI.Game.HUD.InteractionHUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Managers
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance { get; private set; }

        private UIDocument _ui;
        private VisualElement Root => _ui.rootVisualElement;

        public InteractionHUDController InteractionHUD { get; private set; }

        private void Awake()
        {
            _ui = GetComponent<UIDocument>();
            InteractionHUD = new InteractionHUDController();
            InteractionHUD.Initialize(Root);
            
            Instance = this;
        }
    }
}