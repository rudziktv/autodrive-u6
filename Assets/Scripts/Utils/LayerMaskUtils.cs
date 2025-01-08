using UnityEngine;

namespace Utils
{
    public static class LayerMaskUtils
    {
        public static bool ContainsMask(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) != 0;
        }
    }
}