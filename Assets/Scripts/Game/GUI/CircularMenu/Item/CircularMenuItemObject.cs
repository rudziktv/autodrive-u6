
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.GUI.CircularMenu.Item
{
    [CreateAssetMenu]
    public class CircularMenuItemObject : ScriptableObject
    {
        [Header("Simple binding.")] public string SimpleLabel;
        
        
    }
}