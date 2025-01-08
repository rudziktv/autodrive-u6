using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.GUI.CircularMenu.Item
{
    [Serializable]
    public class CircularMenuItemModel : INotifyPropertyChanged
    {
        #region PropertyChanged

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

        #endregion

        public VisualElement View { get; private set; }
        
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public StyleBackground StyleBackground { get; private set; }
        [field: SerializeField] public Texture2D Icon { get; private set; }
        [field: SerializeField] public StyleRotate Rotation { get; private set; }
        [field: SerializeField] public StyleRotate ContentRotation { get; private set; }

        public CircularMenuItemModel(VisualElement view, CircularMenuItemOptions options, float rotation = 0f)
        {
            View = view;
            
            ItemName = options.ItemName;
            Icon = options.Icon;

            StyleBackground = new()
            {
                value = Icon
            };
            
            Rotation = new StyleRotate(new Rotate(new Angle(rotation)));
            ContentRotation = new StyleRotate(new Rotate(new Angle(-rotation)));

            var el = view.Q<VisualElement>("item");
            
            el.RegisterCallback<MouseEnterEvent>(evt =>
            {
                View.AddToClassList("item-hover");
            });

            el.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                View.RemoveFromClassList("item-hover");
            });
        }
    }
}