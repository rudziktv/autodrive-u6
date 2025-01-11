using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models.MFI
{
    public class MFISpeed : MFIModel
    {
        private Label _speed;
        
        public MFISpeed(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "Speed") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-name");
            View.AddToClassList("no-avg");
            View.AddToClassList("no-dataset");

            _speed = CreateValue(unit: "km/h", name: "speed", text: "100");

            // var sect = View.Q<VisualElement>("values-sect");
            View.Q<VisualElement>("values-sect").style.justifyContent = Justify.FlexEnd;
        }
    }
}