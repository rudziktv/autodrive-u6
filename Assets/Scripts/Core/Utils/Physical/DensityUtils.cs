using UnityEngine;

namespace Core.Utils.Physical
{
    public static class DensityUtils
    {
        // Material density in kg/m³, all in 15 *C
        
        // public const float AIR_DENSITY = 1.1225f; // g/cm³
        // public const float OIL_DENSITY = 0.85f; // g/cm³
        // public const float FUEL_DENSITY = 0.75f; // g/cm³
        // public const float COOLANT_DENSITY = 1.05f; // g/cm³

        /// <summary>
        /// Density of air, kg/m³
        /// </summary>
        public const float AIR_DENSITY = 1.225f;

        /// <summary>
        /// Density of oil, kg/m³
        /// </summary>
        public const float OIL_DENSITY = 850;

        /// <summary>
        /// Density of typical coolant (50/50 water-glycol), kg/m³
        /// </summary>
        public const float COOLANT_DENSITY = 1060;

        /// <summary>
        /// Density of petrol fuel E10, kg/m³
        /// </summary>
        public const float GASOLINE_E10_DENSITY = 740;

        /// <summary>
        /// Density of diesel fuel, kg/m³
        /// </summary>
        public const float DIESEL_DENSITY = 830;

        
        /// <summary>
        /// Get mass of the material, based on volume and density.
        /// </summary>
        /// <param name="volume">m³</param>
        /// <param name="density">kg/m³</param>
        /// <returns>kg</returns>
        public static float GetMass(float volume, float density)
            => volume * density;
        
        /// <summary>
        /// Get volume of the material, based on mass and density.
        /// </summary>
        /// <param name="mass">kg</param>
        /// <param name="density">kg/m³</param>
        /// <returns>m³</returns>
        public static float GetVolume(float mass, float density)
            => mass / density;
    }
}