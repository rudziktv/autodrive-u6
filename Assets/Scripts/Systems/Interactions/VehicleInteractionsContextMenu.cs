using Core.Components.Sounds;
using UnityEditor;
using UnityEngine;

namespace Systems.Interactions
{
    public static class VehicleInteractionsContextMenu
    {
        [MenuItem("GameObject/Interactions/Button Interactable", false, 10)]
        public static void CreateButtonInteractable(MenuCommand command)
        {
            GameObject customObject = new GameObject("Button Interaction");
            var collider = customObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.01f, 0.01f, 0.01f);
            customObject.AddComponent<InteractableButton>();
            GameObjectUtility.SetParentAndAlign(customObject, command.context as GameObject);
            Undo.RegisterCreatedObjectUndo(customObject, "Create " + customObject.name);
            Selection.activeObject = customObject;
            customObject.layer = LayerMask.NameToLayer("Interactions");
        }
        
        [MenuItem("GameObject/Interactions/Emitting Button Interactable", false, 10)]
        public static void CreateButtonInteractableWithEmitter(MenuCommand command)
        {
            GameObject customObject = new GameObject("Button Interaction");
            var collider = customObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.01f, 0.01f, 0.01f);
            customObject.AddComponent<InteractableButton>();
            customObject.AddComponent<FMODCustomEmitter>();
            GameObjectUtility.SetParentAndAlign(customObject, command.context as GameObject);
            Undo.RegisterCreatedObjectUndo(customObject, "Create " + customObject.name);
            Selection.activeObject = customObject;
            customObject.layer = LayerMask.NameToLayer("Interactions");
        }
    }
}