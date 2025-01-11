using Core.Patterns.UI;
using Systems.Managers;
using UnityEngine.UIElements;

namespace GUI.Game.CircularMenu
{
    public class CircularMenuController : UIController<GUIManager>
    {
        protected new CircularMenuMainView CurrentModel => base.CurrentModel as CircularMenuMainView;
        
        public void SetVisible(bool active)
        {
            
        }

        protected override void AssignView(VisualElement view) { }
    }
}