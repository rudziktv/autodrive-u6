using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models.MFI
{
    public class MFIAvgSpeed : MFIModel
    {
        private Label _avgSpeed;
        
        public MFIAvgSpeed(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "AvgSpeed", "Speed") { }
        
        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _avgSpeed = CreateValue(unit: "km/h", text: Dataset.AvgSpeed);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _avgSpeed.text = Dataset.AvgSpeed;
        }
    }
}