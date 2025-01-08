using System.Runtime.InteropServices;

namespace Game.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HapticCondition
    {
        /* Header */
        /// <summary>
        /// SDL_HAPTIC_SPRING, SDL_HAPTIC_DAMPER,
        /// SDL_HAPTIC_INERTIA or SDL_HAPTIC_FRICTION
        /// </summary>
        public ushort type;
        /// <summary>
        /// Direction of the effect. - Not used ATM.
        /// </summary>
        public HapticDirection direction;

        /// <summary>
        /// Duration of the effect.
        /// </summary>
        public uint length;
        /// <summary>
        /// Delay before starting the effect.
        /// </summary>
        public ushort delay;

        /* Trigger */
        /// <summary>
        /// Button that triggers the effect.
        /// </summary>
        public ushort button;
        /// <summary>
        /// How soon it can be triggered again after button.
        /// </summary>
        public ushort interval;
        
        
        /* Condition */
        /// <summary>
        /// Level when joystick is to the positive side; max 0xFFFF.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] right_sat;
        /// <summary>
        /// Level when joystick is to the negative side; max 0xFFFF.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] left_sat;
        /// <summary>
        /// How fast to increase the force towards the positive side.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] right_coeff;
        /// <summary>
        /// How fast to increase the force towards the negative side.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] left_coeff;
        /// <summary>
        /// Size of the dead zone; max 0xFFFF: whole axis-range when 0-centered.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] deadband;
        /// <summary>
        /// Position of the dead zone.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] center;
    }
}