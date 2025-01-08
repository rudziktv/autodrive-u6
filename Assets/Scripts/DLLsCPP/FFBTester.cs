using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Game.GUI.Debug.ForceFeedbackTester;
using Game.UnityFFB;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace DLLsCPP
{
    public class FFBTester : MonoBehaviour
    {
        [SerializeField]
        private UIDocument ui;
        
        
        private List<HapticDevice> _hapticDevices = new();
        private VisualElement UIRoot => ui.rootVisualElement;
        private DropdownField _dropdown;
        private uint _selectedDevice;
        private int SelectedIndex => _dropdown.index;
        
        private FFBTesterModel _testerModel;

        private static void UnityLogger(string message)
        {
            Debug.Log($"DLL: {message}");
        }
        
        private delegate void LoggerDelegate(string message);
        
        private void Start()
        {
            HapticManager.Initialize();
            
            LoggerDelegate logger = UnityLogger;
            IntPtr loggerPtr = Marshal.GetFunctionPointerForDelegate(logger);
            NativeSDL3.SetLogger(loggerPtr);
            
            _hapticDevices = HapticManager.EnumerateHapticDevices().ToList();
            
            _dropdown = UIRoot.Q<DropdownField>("ffb-dropdown");

            _dropdown.choices = _hapticDevices.Select(h => h.HapticName).ToList();
            _dropdown.RegisterValueChangedCallback(OnSelectedDeviceChanged);
            
            UIRoot.Q<Button>("simple-test").clicked += OnSimpleTestClick;
            UIRoot.Q<Button>("advanced-test").clicked += OnAdvancedTestClick;

            _testerModel = new FFBTesterModel(ui);
        }

        private void OnAdvancedTestClick()
        {
            if (SelectedIndex == -1)
                return;
            //
            // Debug.Log("AdvancedTestPerformed? " + AdvancedHapticTest(_selectedDevice));
            // _hapticDevices[SelectedIndex].HapticPeriodicEffect(5000, 1000, 10000);
            _hapticDevices[SelectedIndex].HapticSpringEffect(5000, 0, 10000, 10000, 10000, 10000);
        }

        private void OnSimpleTestClick()
        {
            if (SelectedIndex == -1)
                return;
        }

        private void OnSelectedDeviceChanged(ChangeEvent<string> evt)
        {
            if (SelectedIndex == -1)
                return;
            var deviceId = UIRoot.Q<Label>("device-id");
            var deviceName = UIRoot.Q<Label>("device-name");
            

            HapticManager.CloseAllHaptics();
            _selectedDevice = _hapticDevices[SelectedIndex];
            _hapticDevices[SelectedIndex].OpenHaptic();
            
            deviceId.text = _selectedDevice.ToString();
            deviceName.text = _hapticDevices[SelectedIndex];
        }

        private void OnApplicationQuit()
        {
            HapticManager.CloseAllHaptics();
            HapticManager.Dispose();
        }
    }
}