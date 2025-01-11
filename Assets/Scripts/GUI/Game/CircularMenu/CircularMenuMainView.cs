using System.Collections.Generic;
using Core.Patterns.UI;
using GUI.Game.CircularMenu.Item;
using Systems.Managers;
using UnityEngine.UIElements;

namespace GUI.Game.CircularMenu
{
    public class CircularMenuMainView : CircularMenuModel
    {
        private readonly List<CircularMenuItemModel> _items = new();
        private VisualElement _itemsContainer;
        
        public CircularMenuMainView(UIController<GUIManager> ctr, VisualElement view) : base(ctr, view, CircularMenuNames.MAIN_VIEW) { }

        protected override void OnViewBind()
        {
            base.OnViewBind();
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();

            _itemsContainer = View.Q<VisualElement>("menu-items");

            int i = 0;
            // foreach (var option in MenuOptions.Options)
            // {
            //     var rot = 45 * i;
            //     var itemView = MenuOptions.ItemPrefab.Instantiate();
            //     var model = new CircularMenuItemModel(itemView, option, rot);
            //     _items.Add(model);
            //     // itemView.style.rotate.value.angle = rot;
            //     _itemsContainer.Add(itemView);
            //     // itemView.Bind(model);
            //     itemView.dataSource = model;
            //     i++;
            //     if (i >= 8) break;
            // }
            
        }
    }
}