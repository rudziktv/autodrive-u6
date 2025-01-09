using System.Runtime.InteropServices;

namespace Plugins.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential, Size = 48, Pack = 1)]
    public struct HapticPeriodic
    {
        /* Header */
        /// <summary>
        /// SDL_HAPTIC_SINE, SDL_HAPTIC_SQUARE
        /// SDL_HAPTIC_TRIANGLE, SDL_HAPTIC_SAWTOOTHUP or
        /// SDL_HAPTIC_SAWTOOTHDOWN 
        /// </summary>
        public ushort type;
        /// <summary>
        /// Direction of the effect.
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
        
        
        /* Periodic */
        /// <summary>
        /// Period of the wave.
        /// </summary>
        public ushort period;
        /// <summary>
        /// Peak value; if negative, equivalent to 180 degrees extra phase shift.
        /// </summary>
        public short magnitude;
        /// <summary>
        /// Mean value of the wave.
        /// </summary>
        public short offset;
        /// <summary>
        /// Positive phase shift given by hundredth of a degree.
        /// </summary>
        public ushort phase;
        
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
    }
}