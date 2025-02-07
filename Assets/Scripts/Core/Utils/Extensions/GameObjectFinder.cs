using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class GameObjectFinder
    {
        public static GameObject FindChildWithTag(this GameObject gameObject, string tag)
            => RecursiveChildFinder.FindChildWithTagRecursive(gameObject, tag);
    }
}