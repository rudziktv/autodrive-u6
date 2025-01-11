using Core.Entities.Vehicle;
using Core.Entities.Vehicle.Enums;
using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.Enums;
using Systems.Devices.Infotainments.RNS510.ViewModels;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510
{
    public sealed class RNS510Controller : UIController<RNS510>
    {
        public RNS510Assets Assets => Context.Assets;
        public VehicleController Vehicle => Context.Vehicle;
        private RNS510ViewModel CurrentModelRNS => (RNS510ViewModel)CurrentModel;

        public RNS510Controller(RNS510 context, VisualElement root)
        {
            // var mediaScreen = Assets.GetMediaScreenByLoad().Instantiate();
            var model = new RNS510ViewModel(this, new VisualElement(), "Default");
            Initialize(context, root, model);
        }

        public override void Initialize(RNS510 context, VisualElement root, UIModel<RNS510> model)
        {
            base.Initialize(context, root, model);
            Vehicle.ElectricityManager.ElectricityStateChanged += OnElectricityStateChanged;
        }

        private void OnElectricityStateChanged(ElectricityState state)
        {
            CurrentModelRNS.OnElectricityStateChanged(state);
        }

        public void RNS510ButtonClicked(RNS510Button button)
        {
            CurrentModelRNS.RNS510ButtonClicked(button);
        }
    }
}