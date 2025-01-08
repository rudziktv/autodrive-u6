using UI;
using UnityEngine.UIElements;

namespace Game.GUI.CircularMenu
{
    public class CircularMenuModel : UIModel<GUIManager>
    {
        public CircularMenuOptions MenuOptions => Context.CircularMenuOptions;
        
        public CircularMenuModel(UIController<GUIManager> ctr, VisualElement view, string name) : base(ctr, view, name) { }
    }
}