using Core.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Interactions
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private float rayDistance = 2f;
        [SerializeField] private LayerMask raycastLayer;
        [SerializeField] private LayerMask interactionLayer;
        [SerializeField] private Camera interactionCamera;

        [CanBeNull]
        public InteractableItem TryToInteract(Vector3 origin, Vector3 direction)
        {
            return TryToInteract(new Ray(origin, direction));
        }
        
        [CanBeNull]
        public InteractableItem TryToInteract(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, rayDistance, raycastLayer)) return null;
            // Debug.Log($"{((1 << hit.collider.gameObject.layer) & interactionLayer) != 0}");
            if (!interactionLayer.ContainsMask(hit.collider.gameObject.layer))
                return null;
            Debug.Log(hit.collider.gameObject.layer);
            hit.collider.gameObject.TryGetComponent<InteractableItem>(out var interactable);
            Debug.Log(interactable);
            return hit.collider.gameObject.GetComponent<InteractableItem>();
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            Debug.Log("Interact Invoked");

            var pos = interactionCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var ray = interactionCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            var interactable = TryToInteract(ray);
            
            if (interactable == null) return;
            Debug.Log(interactable.name + " is interacted with");
            interactable.SimpleInteract();
        }
    }
}