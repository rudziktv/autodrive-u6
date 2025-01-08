using UI;
using UnityEngine;
using UnityEngine.InputSystem;
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
            set
            {
                var oldIndex = _list.selectedIndex;
                _list.selectedIndex = value;
                _list.RefreshItem(oldIndex);
                _list.RefreshItem(Index);
                Debug.Log(Index);
            }
        }
        
        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            View.AddToClassList("temp-hidden");
            View.AddToClassList("bottom-hidden");

            var main = View.Q<VisualElement>("main");
            View.styleSheets.Add(Assets.SettingsStyle);
            // var container = new VisualElement();
            // container.style.flexGrow = 1;

            var header = View.Q<Label>("header");
            header.text = "SETTINGS";

            _list = new ListView
            {
                focusable = false,
                pickingMode = PickingMode.Ignore,
                fixedItemHeight = 48,
                makeItem = CreateItem,
                bindItem = BindItem,
                name = "settings-list"
            };
            main.Add(_list);

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
            Actions.Development.ComputerUp.performed += SelectPrevItem;
            Actions.Development.ComputerDown.performed += SelectNextItem;
        }

        public override void OnViewUnbind()
        {
            base.OnViewUnbind();
            Actions.Development.ComputerUp.performed -= SelectPrevItem;
            Actions.Development.ComputerDown.performed -= SelectNextItem;
        }

        private VisualElement CreateItem()
        {
            var view = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                },
                pickingMode = PickingMode.Ignore
            };
            view.Add(new Label
            {
                name = "setting-name"
            });
            view.Add(new VisualElement
            {
                name = "selected-item-icon"
            });
            return view;
        }

        private void BindItem(VisualElement view, int i)
        {
            var item = _list.itemsSource[i];
            view.Q<Label>("setting-name").text = item.ToString();
                
            if (Index == i)
                view.AddToClassList("selected-item");
            else
                view.RemoveFromClassList("selected-item");
        }

        private void SelectNextItem(InputAction.CallbackContext ctx)
        {
            if (Index + 1 < _list.itemsSource.Count)
                Index++;
            // _list.RefreshItems();
            _list.ScrollToItem(Index);
        }

        private void SelectPrevItem(InputAction.CallbackContext ctx)
        {
            if (Index - 1 >= 0)
                Index--;
            // _list.RefreshItems();
            _list.ScrollToItem(Index);
        }

        // private void 
    }
}