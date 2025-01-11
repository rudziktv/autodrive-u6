using System;
using UnityEngine;

namespace Core.Helpers
{
    [Serializable]
    public class InterfaceWrapper<T> where T : class
    {
        [SerializeField] private MonoBehaviour reference;

        public T Value => reference as T; // Konwersja na interfejs

        public InterfaceWrapper(MonoBehaviour reference)
        {
            this.reference = reference;
        }
    }
}