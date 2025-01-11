using UnityEditor;
using UnityEngine;

namespace Core.Definitions.GUI
{
    [CreateAssetMenu(menuName = "GUI/Cursor Definition")]
    public class CursorDefinition : ScriptableObject
    {
        [field: SerializeField] public Texture2D Texture { get; private set; }
        [field: SerializeField] public Vector2 Hotspot { get; private set; }
    }
}