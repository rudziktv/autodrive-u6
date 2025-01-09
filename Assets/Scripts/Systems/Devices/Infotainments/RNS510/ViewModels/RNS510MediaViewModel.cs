using Core.Patterns.UI;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510.ViewModels
{
    public class RNS510MediaViewModel : RNS510ViewModel
    {
        public RNS510MediaViewModel(UIController<RNS510> ctr, VisualElement view) : base(ctr, view, "Media") { }
    }
}