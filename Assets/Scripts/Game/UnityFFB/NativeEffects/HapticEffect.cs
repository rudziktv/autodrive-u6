using System;
using System.Runtime.InteropServices;
using Utils;

namespace Game.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Explicit, Size = 72)]
    public struct HapticEffect
    {
        /// <summary>
        /// Effect type.
        /// </summary>
        // [FieldOffset(0)] public ushort type;
        
        [FieldOffset(0)] public HapticConstant constant;
        [FieldOffset(0)] public HapticPeriodic periodic;
        [FieldOffset(0)] public HapticCondition condition;
        [FieldOffset(0)] public HapticRamp ramp;
        [FieldOffset(0)] public HapticLeftRight leftright;
        [FieldOffset(0)] public HapticCustom custom;
    }
}