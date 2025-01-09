using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.ViewModels;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510
{
    public sealed class RNS510Controller : UIController<RNS510>
    {
        public RNS510Assets Assets => Context.Assets;
        private RNS510ViewModel CurrentModelRNS => (RNS510ViewModel)CurrentModel;

        public RNS510Controller(RNS510 context, VisualElement root)
        {
            // var mediaScreen = Assets.GetMediaScreenByLoad().Instantiate();
            var model = new RNS510ViewModel(this, new VisualElement(), "Default");
            Initialize(context, root, model);
        }
    }
}