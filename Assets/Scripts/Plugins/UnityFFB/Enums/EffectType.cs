namespace Plugins.UnityFFB.Enums
{
    public enum EffectType : ushort
    {
        HapticConstant = (ushort)(1u << 0),
        HapticSine = (ushort)(1u << 1),
        HapticSquare = (ushort)(1u << 2),
        HapticTriangle = (ushort)(1u << 3),
        HapticSawtoothup = (ushort)(1u << 4),
        HapticSawtoothdown = (ushort)(1u << 5),
        HapticRamp = (ushort)(1u << 6),
        HapticSpring = (ushort)(1u << 7),
        HapticDamper = (ushort)(1u << 8),
        HapticInertial = (ushort)(1u << 9),
        HapticFriction = (ushort)(1u << 10),
        HapticLeftRight = (ushort)(1u << 11),
        HapticReserved1 = (ushort)(1u << 12),
        HapticReserved2 = (ushort)(1u << 13),
        HapticReserved3 = (ushort)(1u << 14),
        HapticCustom = (ushort)(1u << 15)
    }
}