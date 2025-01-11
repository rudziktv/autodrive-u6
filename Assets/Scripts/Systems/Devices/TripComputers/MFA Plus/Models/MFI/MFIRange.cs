using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models.MFI
{
    public class MFIRange : MFIModel
    {
        private Label _range;
        public MFIRange(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "Range") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _range = CreateValue(text: "470", unit: "km");
            
            View.AddToClassList("no-avg");
            View.AddToClassList("no-dataset");
            View.AddToClassList("range-icon");
        }
    }
}