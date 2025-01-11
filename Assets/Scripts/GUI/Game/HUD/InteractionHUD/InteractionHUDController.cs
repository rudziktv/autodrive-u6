using Core.Patterns.UI;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace GUI.Game.HUD.InteractionHUD
{
    public class InteractionHUDController
    {
        private InteractableDetails _details;
        
        public VisualElement View { get; private set; }

        public void Initialize(VisualElement view)
        {
            View = view;
        }

        public void ShowInteractionHUD(InteractableDetails details)
        {
            if (_details == details || string.IsNullOrEmpty(details.Name))
                return;
            _details = details;
            
            View.dataSource = details;
            if (details.Cursor)
                Cursor.SetCursor(details.Cursor.Texture, details.Cursor.Hotspot, CursorMode.Auto);
        }

        public void HideInteractionHUD()
        {
            _details = null;
            View.dataSource = InteractableDetails.Empty;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}