using System;
using Core.Utils;
using JetBrains.Annotations;
using Systems.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Interactions
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float rayDistance = 2f;
        [SerializeField] private LayerMask raycastLayer;
        [SerializeField] private LayerMask interactionLayer;
        [SerializeField] private Camera interactionCamera;

        [CanBeNull]
        public Interactable TryGetInteractable(Vector3 origin, Vector3 direction)
        {
            return TryGetInteractable(new Ray(origin, direction));
        }
        
        [CanBeNull]
        public Interactable TryGetInteractable(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, rayDistance, raycastLayer)) return null;
            // Debug.Log($"{((1 << hit.collider.gameObject.layer) & interactionLayer) != 0}");
            if (!interactionLayer.ContainsMask(hit.collider.gameObject.layer))
                return null;
            // Debug.Log(hit.collider.gameObject.layer);
            hit.collider.gameObject.TryGetComponent<Interactable>(out var interactable);

            // return hit.collider.gameObject.GetComponent<Interactable>();
            return interactable;
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            Debug.Log("Interact Invoked");

            var pos = interactionCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var ray = interactionCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            var interactable = TryGetInteractable(ray);
            
            if (!interactable) return;
            Debug.Log(interactable.name + " is interacted with");
            interactable.SimpleInteract();
        }

        private void FixedUpdate()
        {
            // var pos = interactionCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var ray = interactionCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            var interactable = TryGetInteractable(ray);

            if (!interactable)
            {
                GUIManager.Instance.InteractionHUD.HideInteractionHUD();
                return;
            }
            
            GUIManager.Instance.InteractionHUD.ShowInteractionHUD(interactable.Details);
        }
    }
}