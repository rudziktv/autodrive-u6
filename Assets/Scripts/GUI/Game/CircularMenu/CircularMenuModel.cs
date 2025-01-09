using Core.Patterns.UI;
using Systems.Managers;
using UnityEngine.UIElements;

namespace GUI.Game.CircularMenu
{
    public class CircularMenuModel : UIModel<GUIManager>
    {
        public CircularMenuOptions MenuOptions => Context.CircularMenuOptions;
        
        public CircularMenuModel(UIController<GUIManager> ctr, VisualElement view, string name) : base(ctr, view, name) { }
    }
}