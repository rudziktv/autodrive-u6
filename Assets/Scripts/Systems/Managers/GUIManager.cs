using GUI.Game.CircularMenu;
using UnityEngine;

namespace Systems.Managers
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