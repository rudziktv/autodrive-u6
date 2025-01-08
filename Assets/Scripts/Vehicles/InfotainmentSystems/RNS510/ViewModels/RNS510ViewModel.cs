using UI;
using UnityEngine.UIElements;

namespace Vehicles.InfotainmentSystems.RNS510.ViewModels
{
    public class RNS510ViewModel : UIModel<RNS510>
    {
        public RNS510Assets Assets => Context.Assets;
        
        public RNS510ViewModel(UIController<RNS510> ctr, VisualElement view, string name) : base(ctr, view, name) { }
    }
}