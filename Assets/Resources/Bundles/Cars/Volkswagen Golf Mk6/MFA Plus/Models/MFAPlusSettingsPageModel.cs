using UI;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models
{
    public class MFAPlusSettingsPageModel : MFAPlusModel
    {
        public MFAPlusSettingsPageModel(UIController<MFAPlus> ctr, VisualElement view) : base(ctr, view, "Settings") { }

        private ListView _list;

        private int Index
        {
            get => _list.selectedIndex;
            set => _list.selectedIndex = value;
        }
        
        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("temp-hidden");
            View.AddToClassList("bottom-hidden");

            var main = View.Q<VisualElement>("main");

            var header = View.Q<Label>("header");
            header.text = "SETTINGS";

            _list = new ListView
            {
                style =
                {
                    flexGrow = 1,
                    marginTop = 12,
                    marginBottom = 12
                }
            };
            main.Add(_list);

            _list.fixedItemHeight = 48;
            // _list.selected
            _list.makeItem = () => new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                }
            };
            _list.bindItem = (view, i) =>
            {
                var item = _list.itemsSource[i];
                view.Add(new Label(item.ToString()));
                
                if (Index == i)
                    view.AddToClassList("selected-item");
                else
                    view.RemoveFromClassList("selected-item");
            };

            _list.itemsSource = new[]
            {
                "Language",
                "MFI",
                "Comfort",
                "Lights",
                "Clock",
                "Winter tires",
                "Units",
                "Services",
                "Factory reset"
            };

            Index = 0;
        }

        protected override void OnViewBind()
        {
            base.OnViewBind();
            
        }
        
        // private void 
    }
}