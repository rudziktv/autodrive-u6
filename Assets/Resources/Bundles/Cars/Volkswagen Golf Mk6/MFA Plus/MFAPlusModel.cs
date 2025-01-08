using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public class MFAPlusModel : UIModel<MFAPlus>, IGetController<MFAPlusController, MFAPlus>
    {
        public MFAPlusController Controller => GetController() as MFAPlusController;
        public InputActions Actions => Context.Actions;
        public MFAPlusComputer Computer => Context.Computer;
        public MFAPlusDataset Dataset => Computer.CurrentDataset;
        public MFAPlusAssets Assets => Context.Assets;
        public MFAPlusModel(UIController<MFAPlus> ctr, VisualElement view, string name) : base(ctr, view, name) { }
        
        public virtual void ShowHeader() { }

        protected Coroutine ShowHeader(string header, float duration = 5)
        {
            return StartCoroutine(ShowHeaderCoroutine(header, duration));
        }

        private IEnumerator ShowHeaderCoroutine(string header, float duration)
        {
            var headerElement = View.Q<Label>("header");
            var topSect = View.Q<VisualElement>("top-sect");

            headerElement.text = header;
            topSect.AddToClassList("header-shown");

            if (duration == 0) yield break;
            yield return new WaitForSeconds(duration);
            topSect.RemoveFromClassList("header-shown");
        }
    }
}