using Core.Patterns.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Game.HUD.AdjustmentHUD
{
    public class MirrorAdjustmentHUDViewModel : UIModel<MonoBehaviour>
    {
        public MirrorAdjustmentHUDViewModel(UIController<MonoBehaviour> ctr, VisualElement view) : base(ctr, view, "MirrorAdjustmentHUD") { }

        private bool _isDragging;
        
        public override void OnViewBindOrCreate()
        {
            base.OnViewBindOrCreate();

            Debug.Log("OnViewBindOrCreate");
            
            var drag = View.Q<VisualElement>("mirror-control-drag");
            
            drag.RegisterCallback<MouseDownEvent>(evt =>
            {
                Debug.Log($"OnPointerDown: {evt.button}");
                _isDragging = true;
            });

            drag.RegisterCallback<MouseUpEvent>(evt =>
            {
                _isDragging = false;
            });
            
            drag.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (!_isDragging)
                    return;

                var top = drag.style.top;
                var left = drag.style.left;
                
                top.value = top.value.value + evt.mouseDelta.y;
                left.value = left.value.value + evt.mouseDelta.x;
                
                drag.style.top = top;
                drag.style.left = left;
                //
                // Debug.Log($"local Pos: {evt.localMousePosition}, org {evt.originalMousePosition}, {evt.mousePosition}");
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}