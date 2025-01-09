using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models.MFI
{
    public class MFIAvgFuelConsumption : MFIModel
    {
        private Label _fuelCons;
        
        public MFIAvgFuelConsumption(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "AvgFuelConsumption") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();

            var name = View.Q<Label>("name");
            name.text = "Consumption";

            _fuelCons = new Label("--.-")
            {
                name = "FuelCons",
            };
            _fuelCons.AddToClassList("values");

            var units = new Label("l/100km")
            {
                name = "units",
            };

            var sect = View.Q<VisualElement>("values-sect");
            sect.Add(_fuelCons);
            sect.Add(units);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _fuelCons.text = Dataset.AvgFuelConsumption;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _fuelCons = null;
        }
    }
}