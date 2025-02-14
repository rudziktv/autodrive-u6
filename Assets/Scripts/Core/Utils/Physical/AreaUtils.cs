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
        
        /// <summary>
        /// Calculates circle area based on radius.
        /// </summary>
        /// <param name="r">Radius</param>
        /// <returns>Area x²</returns>
        public static float CalculateCircleArea(float r)
            => 2f * Mathf.PI * Mathf.Pow(r, 2);
        
        /// <summary>
        /// Calculates circle area based on diameter.
        /// </summary>
        /// <param name="d">Diameter</param>
        /// <returns>Area x²</returns>
        public static float CalculateCircleAreaDiameter(float d)
            => CalculateCircleArea(d / 2f);

        /// <summary>
        /// Calculates cylinder area based on cylinder diameter and height.
        /// KEEP UNITS CONSISTENT!
        /// </summary>
        /// <param name="d">Diameter</param>
        /// <param name="h">Height</param>
        /// <returns>Area</returns>
        public static float CalculateCylinderAreaDiameter(float d, float h)
            => CalculateCircleAreaDiameter(d) + CalculateCylinderSideAreaDiameter(d, h);
        
        /// <summary>
        /// Calculates cylinder area based on cylinder radius and height.
        /// KEEP UNITS CONSISTENT!
        /// </summary>
        /// <param name="r">Radius</param>
        /// <param name="h">Height</param>
        /// <returns>Area</returns>
        public static float CalculateCylinderArea(float r, float h)
            => CalculateCircleArea(r) + CalculateCylinderSideArea(r, h);
        
        /// <summary>
        /// Calculates cylinder area based on cylinder diameter and height.
        /// KEEP UNITS CONSISTENT!
        /// </summary>
        /// <param name="d">Diameter</param>
        /// <param name="h">Height</param>
        /// <returns>Area</returns>
        public static float CalculateCylinderSideAreaDiameter(float d, float h)
            => Mathf.PI * d * h;
        
        /// <summary>
        /// Calculates cylinder area based on cylinder radius and height.
        /// KEEP UNITS CONSISTENT!
        /// </summary>
        /// <param name="r">Radius</param>
        /// <param name="h">Height</param>
        /// <returns>Area</returns>
        public static float CalculateCylinderSideArea(float r, float h)
            => CalculateCylinderSideAreaDiameter(r * 2f, h);
    }
}