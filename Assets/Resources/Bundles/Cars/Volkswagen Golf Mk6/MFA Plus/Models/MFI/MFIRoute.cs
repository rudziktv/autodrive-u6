using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFIRoute : MFIModel
    {
        private Label _route;
        
        public MFIRoute(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "Route") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("no-avg");
            
            _route = CreateValue(unit: "km", text: Dataset.Route.ToString("0"));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _route.text = Dataset.Route.ToString("0");
        }
    }
}