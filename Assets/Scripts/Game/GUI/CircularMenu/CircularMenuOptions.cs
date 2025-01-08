using System;
using Game.GUI.CircularMenu.Item;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.GUI.CircularMenu
{
    [Serializable]
    public class CircularMenuOptions
    {
        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        [field: SerializeField] public VisualTreeAsset ItemPrefab { get; private set; }
        [field: SerializeField] public CircularMenuItemOptions[] Options { get; private set; }
        
        // [SerializeField] private UIDocument uiDocument;
        // [SerializeField] private CircularMenuItemOptions[] options;
        //
        // [SerializeField] private VisualTreeAsset itemPrefab;
        
        // [field: SerializeField] public UIDocument TestingBackingField { get; private set; }
        
        // public UIDocument UIDocument => uiDocument;
        // public CircularMenuItemOptions[] Options => options;
        //
        // public VisualTreeAsset ItemPrefab => itemPrefab;
    }
}