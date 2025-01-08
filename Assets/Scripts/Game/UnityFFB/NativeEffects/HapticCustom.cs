using System;
using System.Runtime.InteropServices;

namespace Game.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential, Size = 56, Pack = 1)]
    public class HapticCustom
    {
        /* Header */
        /// <summary>
        /// SDL_HAPTIC_CONSTANT
        /// </summary>
        public ushort type;
        /// <summary>
        /// Direction of the effect.
        /// </summary>
        public HapticDirection direction;

        /* Replay */
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

        /* Custom */
        /// <summary>
        /// Axes to use, minimum of one.
        /// </summary>
        public byte channels;
        /// <summary>
        /// Sample periods.
        /// </summary>
        public ushort period;
        /// <summary>
        /// Amount of samples.
        /// </summary>
        public ushort samples;
        /// <summary>
        /// Should contain channels*samples items.
        /// </summary>
        // [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)]
        public IntPtr data;
        
        /* Envelope */
        /// <summary>
        /// Duration of the attack.
        /// </summary>
        public ushort attack_length;
        /// <summary>
        /// Level at the start of the attack.
        /// </summary>
        public ushort attack_level;
        /// <summary>
        /// Duration of the fade.
        /// </summary>
        public ushort fade_length;
        /// <summary>
        /// Level at the end of the fade.
        /// </summary>
        public ushort fade_level;

        // private short Size => (short)(channels * samples);
    }
}