using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFIOilTemp : MFIModel
    {
        private Label _oilTemp;
        
        public MFIOilTemp(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "OilTemp", "Oil temperature") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-avg");
            View.AddToClassList("no-dataset");

            _oilTemp = CreateValue(unit: "\u00b0C", text: "---");
        }
    }
}