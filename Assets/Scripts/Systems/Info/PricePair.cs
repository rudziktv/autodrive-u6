using System;
using UnityEngine;

namespace Systems.Info
{
    [Serializable]
    public class PricePair<T>
    {
        [field: SerializeField] public T Item { get; private set; }
        [field: SerializeField] public float Price { get; private set; }
    }
}