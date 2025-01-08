using UI;
using UnityEngine.UIElements;

namespace Game.GUI.CircularMenu
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