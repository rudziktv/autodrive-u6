using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions
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