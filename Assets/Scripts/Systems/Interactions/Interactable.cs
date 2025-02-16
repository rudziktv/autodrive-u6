using Core.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Systems.Interactions
{
    public class Interactable : MonoBehaviour
    {
        [field: SerializeField] public InteractableDetails Details { get; private set; }
        [Header("Interaction Events")]
        [FormerlySerializedAs("OnInteract")] public UnityEvent interact;

        public UnityEvent onInteractionStarted;
        public UnityEvent onInteractionEnded;


        public virtual void SimpleInteract()
        {
            interact?.Invoke();
        }

        public virtual void InteractionStarted()
        {
            onInteractionStarted?.Invoke();
            InputSystem.actions.FindAction(GlobalInputs.INTERACT)
                .canceled += InteractionEnded;
        }

        private void InteractionEnded(InputAction.CallbackContext ctx)
            => InteractionEnded();

        public virtual void InteractionEnded()
        {
            onInteractionEnded?.Invoke();
            InputSystem.actions.FindAction(GlobalInputs.INTERACT)
                .canceled -= InteractionEnded;
        }
    }
}