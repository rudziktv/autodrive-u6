using System;
using UnityEngine;

namespace Core.Components.Lights
{
    [Serializable]
    public class PropertyValuePair<T>
    {
        [SerializeField] private string propertyName;
        [SerializeField] private T value;
        
        public T Value => value;
        public int PropertyID => Shader.PropertyToID(propertyName);
        public string PropertyName => propertyName;
    }
}