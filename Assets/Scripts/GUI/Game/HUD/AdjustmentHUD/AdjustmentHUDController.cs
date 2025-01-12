using Core.Patterns.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Game.HUD.AdjustmentHUD
{
    public sealed class AdjustmentHUDController : UIController<MonoBehaviour>
    {
        public AdjustmentHUDController(MonoBehaviour context, VisualElement root)
        {
            Initialize(context, root, new MirrorAdjustmentHUDViewModel(this, root.Q<VisualElement>("mirrors-adjustment-tab")));
        }

        protected override void AssignView(VisualElement view)
        {
            // view.style.flexGrow = 1;
            // Root.Add(view);
            // base.AssignView(view);
        }
    }
}