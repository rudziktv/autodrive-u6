using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFISpeedWarning : MFIModel
    {
        private Label _speedWarning;
        
        public MFISpeedWarning(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "SpeedWarning", "Speed warning") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-dataset");
            View.AddToClassList("no-avg");

            _speedWarning = CreateValue(unit: "km/h", text: "---");
        }
    }
}