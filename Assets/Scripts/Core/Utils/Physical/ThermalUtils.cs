namespace Core.Utils.Physical
{
    public class ThermalUtils
    {
        // Thermal Capacity in kJ/kg*K
        public const float AIR_THERMAL_CAPACITY = 1.005f;
        public const float OIL_THERMAL_CAPACITY = 2.2f;
        public const float COOLANT_THERMAL_CAPACITY = 3.8f;
        public const float ALUMINIUM_THERMAL_CAPACITY = 0.9f;
        public const float CAST_IRON_THERMAL_CAPACITY = 0.5f;

        // Thermal Conductivity in kW/m²*K
        public const float ALUMINIUM_THERMAL_CONDUCTIVITY = 0.2f;
        public const float OIL_THERMAL_CONDUCTIVITY = 0.15f / 1000f;
        public const float COOLANT_THERMAL_CONDUCTIVITY = 0.5f / 1000f;
        
        // Diabetic Indexes
        public const float AIR_DIABETIC_INDEX = 1.4f;
    }
}