using UnityEngine;

namespace Core.Utils.Physical
{
    public class VolumeUtils
    {
        public static float ConvertMm3ToM3(float volume)
            => volume / Mathf.Pow(10, 9);
        
        public static float ConvertCm3ToM3(float volume)
            => volume / Mathf.Pow(10, 6);
        
        public static float ConvertDm3ToM3(float volume)
            => volume / Mathf.Pow(10, 3);
    }
}