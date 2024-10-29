using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI
{
    public class MFIModel : MFAPlusModel
    {
        private readonly string _parameterName;

        public MFIModel(UIController<MFAPlus> ctr, VisualElement view, string name, string paramName = "") : base(ctr, view,
            name)
        {
            _parameterName = paramName == "" ? name : paramName;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            SetName(_parameterName);
        }

        protected virtual Label CreateValue(string text = "", string name = "val")
        {
            var value = new Label(text)
            {
                name = name
            };
            value.AddToClassList("values");
            return value;
        }
        
        protected virtual Label CreateValue(string unit, string text = "", string name = "val", string unitName = "units")
        {
            var value = new Label(text)
            {
                name = name
            };
            value.AddToClassList("values");

            var units = new Label(unit)
            {
                name = unitName
            };
            units.AddToClassList("units");

            var sect = View.Q<VisualElement>("values-sect");
            sect.Add(value);
            sect.Add(units);

            return value;
        }

        protected void SetName(string name)
        {
            var l = View.Q<Label>("name");
            l.text = name;
        }
    }
}