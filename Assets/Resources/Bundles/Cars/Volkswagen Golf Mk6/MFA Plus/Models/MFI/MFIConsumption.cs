using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFIConsumption : MFIModel
    {
        private Label _cons;
        private Label _units;
        
        public MFIConsumption(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "Consumption") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-avg");
            View.AddToClassList("no-dataset");

            _cons = CreateValue(unit: "l/h", text: "0.0");
            _units = View.Q<Label>("units");
        }
    }
}