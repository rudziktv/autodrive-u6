using Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public sealed class MFAPlusController : UIController<MFAPlus>
    {
        public MFAPlusController(MFAPlus context, VisualElement root)
        {
            var model = new MFAPlusModel(this, new VisualElement(), "Default");
            OnInitialize(context, root, model);
        }
    }
}