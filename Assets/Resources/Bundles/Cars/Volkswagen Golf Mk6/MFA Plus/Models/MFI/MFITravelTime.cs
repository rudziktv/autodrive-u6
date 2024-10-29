using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFITravelTime : MFIModel
    {
        private Label _hours;
        private Label _minutes;
        
        public MFITravelTime(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "TravelTime", "Travel time") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-avg");

            _hours = CreateValue("h", "0", "hours", "hours");
            _minutes = CreateValue(unit: "min", text: "0");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Dataset.TravelTimeHours == 0)
                View.AddToClassList("no-hours");
            else
                View.RemoveFromClassList("no-hours");
            
            _hours.text = Dataset.TravelTimeHours.ToString();
            _minutes.text = Dataset.TravelTimeMinutes.ToString();
        }
    }
}