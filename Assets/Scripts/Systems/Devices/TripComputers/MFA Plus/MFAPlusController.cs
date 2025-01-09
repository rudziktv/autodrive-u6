using System.Collections;
using System.Collections.Generic;
using Core.Entities.Vehicle.Enums;
using Core.Patterns.UI;
using Systems.Devices.TripComputers.MFA_Plus.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus
{
    public sealed class MFAPlusController : UIController<MFAPlus>
    {
        private const float SPLASH_SCREEN_TIMER = 4f;
        private const float SAFE_LOCK_TIMER = 5f;
        private Coroutine _splashScreenCoroutine;
        public MFAPlusAssets Assets => Context.Assets;

        private readonly List<string> _notifications = new();
        private Coroutine _notificationsCoroutine = null;
        private bool _areControlsLocked = false;
        // private List<string> _status;
        private MFAPlusModel CurrentModelMFA => (MFAPlusModel)CurrentModel;
        
        public MFAPlusController(MFAPlus context, VisualElement root)
        {
            var model = new MFAPlusModel(this, new VisualElement(), "Default");
            Initialize(context, root, model);
            
            context.Vehicle.ElectricityManager.OnElectricityStateChanged += OnElectricityStateChanged;
        }

        public override void Initialize(MFAPlus context, VisualElement root, UIModel<MFAPlus> model)
        {
            base.Initialize(context, root, model);
            Context.Vehicle.CodingVariables.StateChanged += CodingVariableChanged;
        }

        private void CodingVariableChanged(string key, string value)
        {
            if (key.StartsWith("door_"))
                DoorStateChanged(key, value);
            
            // WIP...
        }

        private void DoorStateChanged(string key, string value)
        {
            var door = key.Replace("door_", "");

            switch (value)
            {
                case "opened":
                    Root.AddToClassList($"open-{door}");
                    break;
                default:
                    Root.RemoveFromClassList($"open-{door}");
                    break;
            }
        }

        private void OnElectricityStateChanged(ElectricityState newState)
        {
            switch (newState)
            {
                case ElectricityState.Off:
                    break;
                case ElectricityState.OnlyAccessories:
                    OnOnlyAccessories();
                    break;
                case ElectricityState.Ignition:
                    OnIgnition();
                    break;
            }
            // throw new System.NotImplementedException();
        }

        private void OnOnlyAccessories()
        {
            if (_splashScreenCoroutine != null)
                Context.StopCoroutine(_splashScreenCoroutine);

            _splashScreenCoroutine = Context.StartCoroutine(SafeLockInformation());
        }

        private void OnIgnition()
        {
            if (_splashScreenCoroutine != null)
                Context.StopCoroutine(_splashScreenCoroutine);

            _splashScreenCoroutine = Context.StartCoroutine(IgnitionSplashScreen());
        }

        private IEnumerator SafeLockInformation()
        {
            var view = Assets.MainScreen.Instantiate();
            var text = new Label("Check\nSAFELOCK!\nOwner's manual!")
            {
                name = "SAFELOCK",
                style =
                {
                    marginTop = 12,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            view.Q<VisualElement>("main").Add(text);
            view.AddToClassList("clock-only");
            view.AddToClassList("temp-content-hidden");
            NavigateToAndDestroy(new MFAPlusModel(this, view, "SafeLockInformation"));

            yield return new WaitForSeconds(SAFE_LOCK_TIMER);
            view.Q<VisualElement>("main").Clear();
            view.AddToClassList("temp-hidden");
            text = null;
        }

        private IEnumerator IgnitionSplashScreen()
        {
            NavigateToAndDestroy(new MFAPlusModel(this, Assets.SplashScreen.Instantiate(), "SplashScreen"));
            yield return new WaitForSeconds(SPLASH_SCREEN_TIMER);
            NavigateToAndDestroy(new MFAPlusDataPageModel(this, Assets.MainScreen.Instantiate()));
            yield return new WaitForSeconds(3f);
            PushNotification("Warning!\nThis is preview of MFA Plus.\nBe aware there can be flaws.");
            yield return new WaitForSeconds(3f);
            PushNotification("Fill the gas!\nRange\n0 km");
        }

        private void PushNotification(string message)
        {
            _notifications.Add(message);
            _notificationsCoroutine ??= Context.StartCoroutine(NotificationCoroutine());
        }

        private IEnumerator NotificationCoroutine(float duration = 5f)
        {
            var i = 0;
            var previous = CurrentModelMFA;
            _areControlsLocked = true;
            
            while (i < _notifications.Count)
            {
                var view = Assets.MainScreen.Instantiate();
                var n = _notifications[i];
                var model = new MFAPlusNotificationPageModel(this, view, n);
                // model.ShowHeader()
                NavigateTo(model);
                model.Show();
                yield return new WaitForSeconds(duration);
                i++;
            }
            
            _notifications.Clear();
            _areControlsLocked = false;
            NavigateTo(previous);
            previous.ShowHeader();
        }

        public override void Dispose()
        {
            base.Dispose();
            Context.Vehicle.ElectricityManager.OnElectricityStateChanged -= OnElectricityStateChanged;
        }
    }
}