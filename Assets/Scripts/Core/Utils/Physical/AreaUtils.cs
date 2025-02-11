using UnityEngine;

namespace Core.Utils.Physical
{
    public static class AreaUtils
    {
        public static float ConvertMm2ToM2(float volume)
            => volume / Mathf.Pow(10, 6);
        
        public static float ConvertCm2ToM2(float volume)
            => volume / Mathf.Pow(10, 4);
        
        public static float ConvertDm2ToM2(float volume)
            => volume / Mathf.Pow(10, 2);
    }
}