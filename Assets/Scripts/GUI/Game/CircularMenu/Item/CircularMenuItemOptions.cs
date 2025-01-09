using System;
using UnityEngine;
using UnityEngine.Events;

namespace GUI.Game.CircularMenu.Item
{
    [Serializable]
    public class CircularMenuItemOptions
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Texture2D Icon { get; private set; }
        [field: SerializeField] public UnityEvent Action { get; private set; }
        [field: SerializeField] public CircularMenuItemOptions[] ChildrenOptions { get; private set; }
        
        // [SerializeField] private string itemName;
        // [SerializeField] private Sprite icon;
        
        // [SerializeField] private CircularMenuItemOptions[] children;
        
        // public string ItemName => itemName;
        // public Sprite Icon => icon;
    }
}