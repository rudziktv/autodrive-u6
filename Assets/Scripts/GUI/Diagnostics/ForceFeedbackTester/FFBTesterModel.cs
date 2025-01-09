using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugins.UnityFFB.Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI.Debug.ForceFeedbackTester
{
    [Serializable]
    public class FFBTesterModel : INotifyPropertyChanged
    {
        [field: SerializeField] public EffectType Type { get; set; }
        
        
        private readonly UIDocument _ui;
        private VisualElement Root => _ui.rootVisualElement;

        private VisualElement _settingPanelParent;
        
        
        // PANELS
        private VisualElement _constantPanel;
        private VisualElement _conditionPanel;
        
        // ASSETS
        private VisualTreeAsset _constantPanelAsset;
        private VisualTreeAsset _conditionPanelAsset;
        

        // MODELS
        private FFBConditionEffectModel _conditionModel;
        
        
        public FFBTesterModel(UIDocument ui)
        {
            _ui = ui;
            Root.dataSource = this;

            _settingPanelParent = Root.Q<VisualElement>("effect-detail-settings");
            
            _settingPanelParent.Clear();
            
            
            LoadResources();
            InstantiatePanels();
            
            _settingPanelParent.Add(_conditionPanel);
            _conditionModel = new(_conditionPanel);
            
            
            _conditionPanel.dataSource = _conditionModel;

            var type = Root.Q<EnumField>("type");
            type.RegisterValueChangedCallback(TypeChangedCallback);
        }

        private void TypeChangedCallback(ChangeEvent<Enum> evt)
        {
            _settingPanelParent.Clear();
            if ((EffectType)evt.newValue == EffectType.HapticConstant)
            {
                _settingPanelParent.Add(_constantPanel);
            }
            else
            {
                _settingPanelParent.Add(_conditionPanel);
            }
        }


        private void InstantiatePanels()
        {
            _constantPanel = _constantPanelAsset.Instantiate();
            _conditionPanel = _conditionPanelAsset.Instantiate();
        }

        private void LoadResources()
        {
            _constantPanelAsset = UnityEngine.Resources.Load<VisualTreeAsset>("GUI/Debug/Force Feedback Tester/ConstantEffect");
            _conditionPanelAsset = UnityEngine.Resources.Load<VisualTreeAsset>("GUI/Debug/Force Feedback Tester/ConditionEffect");
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