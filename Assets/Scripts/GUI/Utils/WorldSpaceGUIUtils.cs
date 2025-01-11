using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Utils
{
    public static class WorldSpaceGUIUtils
    {
        public const float WORLD_SPACE_GUI_INTERACTION_RANGE = 5f;
        
        public static Vector2 SetScreenToPanelSpaceFunction(Vector2 screenPosition, UIDocument ui, string name)
        {
            Debug.Log("SetScreenToPanelSpaceFunction started");

            var invalidPosition = new Vector2(float.NaN, float.NaN);
            
            if (Camera.main == null)
                return invalidPosition;
            
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(cameraRay, out var hit, WORLD_SPACE_GUI_INTERACTION_RANGE) ||
                hit.collider.gameObject.name != name)
            {
                return invalidPosition;
            }
            
            Debug.Log("SetScreenToPanelSpaceFunction git");
            
            var pixelUV = hit.textureCoord;
            
            pixelUV.y = 1f - pixelUV.y;
            pixelUV.x *= ui.panelSettings.targetTexture.width;
            pixelUV.y *= ui.panelSettings.targetTexture.height;
            
            Debug.Log($"SetScreenToPanelSpaceFunction {pixelUV}");

            
            return pixelUV;
        }
    }
}