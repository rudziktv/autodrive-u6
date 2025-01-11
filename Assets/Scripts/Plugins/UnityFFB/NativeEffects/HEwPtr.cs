using System;
using System.Runtime.InteropServices;
using Core.Utils;

namespace Plugins.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Explicit)]
    public class HEwPtr
    {
        /// <summary>
        /// Effect type.
        /// </summary>
        [FieldOffset(0)] public ushort type;
        [FieldOffset(0)] public IntPtr constant;
        [FieldOffset(0)] public IntPtr periodic;
        [FieldOffset(0)] public IntPtr condition;
        [FieldOffset(0)] public IntPtr ramp;
        [FieldOffset(0)] public IntPtr leftright;
        [FieldOffset(0)] public IntPtr custom;

        public static HEwPtr CreateEffect(HapticConstant constant)
        {
            return new HEwPtr
            {
                type = Effects.HAPTIC_CONSTANT,
                constant = MarshalUtils.AllocStructureToPtr(constant)
            };
        }
        
        public static HEwPtr CreateEffect(HapticPeriodic periodic)
        {
            return new HEwPtr
            {
                type = periodic.type,
                periodic = MarshalUtils.AllocStructureToPtr(periodic)
            };
        }
        
        public static HEwPtr CreateEffect(HapticCondition condition)
        {
            return new HEwPtr
            {
                type = condition.type,
                condition = MarshalUtils.AllocStructureToPtr(condition)
            };
        }
        
        public static HEwPtr CreateEffect(HapticRamp ramp)
        {
            return new HEwPtr
            {
                type = ramp.type,
                ramp = MarshalUtils.AllocStructureToPtr(ramp)
            };
        }
        
        public static HEwPtr CreateEffect(HapticLeftRight leftright)
        {
            return new HEwPtr
            {
                type = leftright.type,
                leftright = MarshalUtils.AllocStructureToPtr(leftright)
            };
        }
        
        public static HEwPtr CreateEffect(HapticCustom custom)
        {
            return new HEwPtr
            {
                type = custom.type,
                custom = MarshalUtils.AllocStructureToPtr(custom)
            };
        }
        
        public void Free()
        {
            if (constant != IntPtr.Zero)
                Marshal.FreeHGlobal(constant);
            if (periodic != IntPtr.Zero)
                Marshal.FreeHGlobal(periodic);
            if (condition != IntPtr.Zero)
                Marshal.FreeHGlobal(condition);
            if (ramp != IntPtr.Zero)
                Marshal.FreeHGlobal(ramp);
            if (leftright != IntPtr.Zero)
                Marshal.FreeHGlobal(leftright);
            if (custom != IntPtr.Zero)
                Marshal.FreeHGlobal(custom);
        
            constant = IntPtr.Zero;
            periodic = IntPtr.Zero;
            condition = IntPtr.Zero;
            ramp = IntPtr.Zero;
            leftright = IntPtr.Zero;
            custom = IntPtr.Zero;
        }
    }
}