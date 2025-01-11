using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Systems.Interactions
{
    public class Interactable : MonoBehaviour
    {
        [field: SerializeField] public InteractableDetails Details { get; private set; }
        [Header("Interaction Events")]
        [FormerlySerializedAs("OnInteract")] public UnityEvent interact;


        public virtual void SimpleInteract()
        {
            interact?.Invoke();
        }
    }
}