using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Debug.ForceFeedbackTester
{
    [Serializable]
    public class FFBConditionEffectModel : INotifyPropertyChanged
    {
        private readonly VisualElement _root;
        
        [field: SerializeField] public uint Length { get; set; }
        [field: SerializeField] public ushort RightSat { get; set; }
        [field: SerializeField] public ushort LeftSat { get; set; }
        [field: SerializeField] public short RightCoeff { get; set; }
        [field: SerializeField] public short LeftCoeff { get; set; }
        [field: SerializeField] public ushort Deadband { get; set; }
        [field: SerializeField] public short Center { get; set; }

        public FFBConditionEffectModel(VisualElement root)
        {
            // Root.bind
            _root = root;
            _root.dataSource = this;
        }


        #region INotifyPropertyChanged

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
        
    }
}