using Core.Entities.Vehicle;
using Core.Entities.Vehicle.Enums;
using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.Enums;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510.ViewModels
{
    public class RNS510ViewModel : UIModel<RNS510>
    {
        public RNS510Assets Assets => Context.Assets;
        public VehicleController Vehicle => Context.Vehicle;
        
        public RNS510ViewModel(UIController<RNS510> ctr, VisualElement view, string name) : base(ctr, view, name) { }
        
        public virtual void RNS510ButtonClicked(RNS510Button button) { }
        public virtual void OnElectricityStateChanged(ElectricityState state) { }
    }
}