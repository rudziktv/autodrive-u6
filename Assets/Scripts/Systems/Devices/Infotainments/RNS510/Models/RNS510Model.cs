using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510.Models
{
    public class RNS510Model : UIModel<RNS510>
    {
        public RNS510Controller Controller => (RNS510Controller)GetController();
        public RNS510Assets Assets => Context.Assets;
        public RNS510Model(UIController<RNS510> ctr, string name) : base(ctr, new VisualElement(), name) { }
    }
}