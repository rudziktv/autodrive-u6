using System;
using System.Runtime.InteropServices;
using Plugins.UnityFFB.NativeEffects;

// ReSharper disable InconsistentNaming

namespace Plugins.UnityFFB
{
    public static class NativeSDL3
    {
        public const string DLL = "libFFB.so";
        
        [DllImport(DLL)] public static extern void InitSDL();
        [DllImport(DLL)] public static extern void DisposeSDL();


        #region SimpleHaptics

        // [DllImport(DLL)] public static extern int GetHapticCount();
        // [DllImport(DLL)] public static extern uint GetHapticID(int index);
        // [DllImport(DLL)] public static extern IntPtr GetHapticName(uint hapticID);
        // [DllImport(DLL)] public static extern void TryToTestHaptic(uint hapticID);
        // [DllImport(DLL)] public static extern bool AdvancedHapticTest(uint hapticID);

        #endregion
        
        [DllImport(DLL)] public static extern void OpenHaptic(uint hapticID);
        [DllImport(DLL)] public static extern void CloseHaptic(uint hapticID);
        [DllImport(DLL)] public static extern void CloseAllHaptics();
        
        [DllImport(DLL)] public static extern void SetLogger(IntPtr logger);
        
        public delegate void LoggerDelegate(string msg);

        [DllImport(DLL)]
        public static extern IntPtr TestingStruct(ref HapticCondition test_struct);

        [DllImport(DLL)]
        public static extern IntPtr FullEffectTest(ref HapticEffect test_struct);

        [DllImport(DLL)]
        public static extern void DebugSizes();

        [DllImport(DLL)] public static extern IntPtr GetHapticDevicesInfo(ref int count);


        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CreateEffect(uint hapticID, IntPtr effect);
        
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateOrCreateEffect(uint hapticID, ref int effect_id, IntPtr effect);
        
        [DllImport(DLL)] public static extern void StopEffect(uint hapticID, int effect_id);
        [DllImport(DLL)] public static extern void StopAllEffects(uint hapticID);
        
        [DllImport(DLL)] public static extern int CreateConstantEffect(uint hapticID, ref HapticConstant constant);
        [DllImport(DLL)] public static extern int CreatePeriodicEffect(uint hapticID, ref HapticPeriodic periodic);
        [DllImport(DLL)] public static extern int CreateConditionEffect(uint hapticID, ref HapticCondition condition);
        [DllImport(DLL)] public static extern int CreateRampEffect(uint hapticID, ref HapticRamp ramp);
        [DllImport(DLL)] public static extern int CreateLeftRightEffect(uint hapticID, ref HapticLeftRight leftright);
        [DllImport(DLL)] public static extern int CreateCustomEffect(uint hapticID, ref HapticCustom custom);
        
        [DllImport(DLL)] public static extern void UpdateOrCreateConstantEffect(uint hapticID, ref int effect_id, ref HapticConstant constant);
        [DllImport(DLL)] public static extern void UpdateOrCreatePeriodicEffect(uint hapticID, ref int effect_id, ref HapticPeriodic periodic);
        [DllImport(DLL)] public static extern void UpdateOrCreateConditionEffect(uint hapticID, ref int effect_id, ref HapticCondition condition);
        [DllImport(DLL)] public static extern void UpdateOrCreateRampEffect(uint hapticID, ref int effect_id, ref HapticRamp ramp);
        [DllImport(DLL)] public static extern void UpdateOrCreateLeftRightEffect(uint hapticID, ref int effect_id, ref HapticLeftRight leftright);
        [DllImport(DLL)] public static extern void UpdateOrCreateCustomEffect(uint hapticID, ref int effect_id, ref HapticCustom custom);
        
        // [DllImport(DLL)] public static extern void HapticPeriodicEffect(uint length, ushort period, short magnitude);
        // [DllImport(DLL)] public static extern void HapticSpringEffect(uint length, ushort deadband, short left_coefficient, short right_coefficient, ushort left_saturation, ushort right_saturation);
        
        // JOYSTICK IMPORTS --NOT USED--
        // [DllImport(DLL)] public static extern int GetJoystickCount();
        // [DllImport(DLL)] public static extern uint GetJoystickID(int index);
        // [DllImport(DLL)] public static extern IntPtr GetJoystickName(uint joystickID);
    }
}