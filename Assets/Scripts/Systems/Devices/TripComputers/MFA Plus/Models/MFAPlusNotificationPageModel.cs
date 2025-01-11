using Core.Patterns.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models
{
    public class MFAPlusNotificationPageModel : MFAPlusModel
    {
        private string _message;
        
        public MFAPlusNotificationPageModel(UIController<MFAPlus> ctr, VisualElement view, string message) : base(ctr,
            view, "Notification")
        {
            _message = message;
        }

        public void Show()
        {
            var main = View.Q<VisualElement>("main");
            main.Clear();
            main.Add(new Label(_message)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            });
            main.style.alignItems = Align.Center;
            main.style.justifyContent = Justify.Center;
            ShowHeader("INFORMATION", 0);
        }
    }
}