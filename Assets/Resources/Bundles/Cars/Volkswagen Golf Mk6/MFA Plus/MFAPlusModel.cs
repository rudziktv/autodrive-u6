using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public class MFAPlusModel : UIModel<MFAPlus>, IGetController<MFAPlusController, MFAPlus>
    {
        public MFAPlusController Controller => GetController() as MFAPlusController;
        public InputActions Actions => Context.Actions;
        public MFAPlusComputer Computer => Context.Computer;
        public MFAPlusDataset Dataset => Computer.CurrentDataset;
        public MFAPlusModel(UIController<MFAPlus> ctr, VisualElement view, string name) : base(ctr, view, name) { }
    }
}