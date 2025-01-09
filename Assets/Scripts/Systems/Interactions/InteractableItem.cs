using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interactions
{
    public class InteractableItem : MonoBehaviour
    {
        public UnityEvent OnInteract;

        public void SimpleInteract()
        {
            OnInteract?.Invoke();
        }
    }
}