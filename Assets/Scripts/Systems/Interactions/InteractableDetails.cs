using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Definitions.GUI;
using Core.Helpers;
using UnityEngine;

namespace Systems.Interactions
{
    [Serializable]
    public class InteractableDetails : INotifyPropertyChanged
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public CursorDefinition Cursor { get; private set; }
        // [field: SerializeField] public Vector2 CursorHotspot { get; private set; } = Vector2.zero;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
        public static InteractableDetails Empty => new();
    }
}