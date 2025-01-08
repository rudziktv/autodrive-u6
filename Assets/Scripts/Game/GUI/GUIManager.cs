using System;
using Game.GUI.CircularMenu;
using UnityEngine;

namespace Game.GUI
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private CircularMenuOptions circularMenuOptions;
        
        private CircularMenuController _circularMenu;
        public CircularMenuOptions CircularMenuOptions => circularMenuOptions;

        private void Start()
        {
            InitializeCircularMenu();
        }

        private void InitializeCircularMenu()
        {
            var view = CircularMenuOptions.UIDocument.rootVisualElement;
            _circularMenu = new CircularMenuController();
            var model = new CircularMenuMainView(_circularMenu, view);
            _circularMenu.Initialize(this, view, model);
        }
    }
}