using System;
using System.Collections;
using Core.Entities.Vehicle;
using Core.Entities.Vehicle.Enums;
using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.Enums;
using Systems.Devices.Infotainments.RNS510.Models;
using Systems.Devices.Infotainments.RNS510.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510
{
    public sealed class RNS510Controller : UIController<RNS510>
    {
        private const float SPLASH_SCREEN_LENGTH = 6f;
        
        public RNS510Assets Assets => Context.Assets;
        public VehicleController Vehicle => Context.Vehicle;
        private RNS510ViewModel CurrentModelRNS => (RNS510ViewModel)CurrentModel;

        // private RNS510ViewModel _splashScreen;
        
        private Coroutine _splashScreenCoroutine;

        public RNS510ButtonModel ButtonManager { get; private set; }
        public RNS510PowerModel PowerManager { get; private set; }
        public RNS510SettingsModel SettingsManager { get; private set; }

        public RNS510Controller(RNS510 context, VisualElement root)
        {
            var model = new RNS510ViewModel(this, new VisualElement(), "Default");
            Initialize(context, root, model);
        }
        

        public override void Initialize(RNS510 context, VisualElement root, UIModel<RNS510> model)
        {
            base.Initialize(context, root, model);
            Vehicle.ElectricityManager.ElectricityStateChanged += OnElectricityStateChanged;
            
            ButtonManager = new RNS510ButtonModel(this);
            PowerManager = new RNS510PowerModel(this);
            SettingsManager = new RNS510SettingsModel(this);
            SettingsManager.UpdateFMODParameters();
            
            // var splashScreenView = Assets.GetSplashScreenByLoad().Instantiate();
            // splashScreenView.style.flexGrow = 1;
            // _splashScreen = new RNS510ViewModel(this, splashScreenView, "SplashScreen");
        }

        private void OnElectricityStateChanged(ElectricityState state)
        {
            PowerManager.OnElectricityStateChanged(state);
            CurrentModelRNS.OnElectricityStateChanged(state);
        }

        public void RNS510ButtonClicked(RNS510Button button)
        {
            ButtonManager.RNS510ButtonClicked(button);
            
            if (PowerManager == RNS510PowerState.On)
                CurrentModelRNS.RNS510ButtonClicked(button);
        }

        public void SplashScreen()
        {
            StopCoroutine(_splashScreenCoroutine);
            PowerManager.CurrentPowerState = RNS510PowerState.SplashScreen;
            _splashScreenCoroutine = StartCoroutine(SplashScreenCoroutine());
        }

        private IEnumerator SplashScreenCoroutine()
        {
            NavigateTo(new RNS510ViewModel(this, Assets.GetSplashScreenByLoad().Instantiate(), "SplashScreen"));
            yield return new WaitForSeconds(SPLASH_SCREEN_LENGTH);
            PowerManager.CurrentPowerState = RNS510PowerState.On;
            NavigateTo(new RNS510RadioViewModel(this,
                Assets.GetRadioScreenByLoad().Instantiate()));
        }
    }
}