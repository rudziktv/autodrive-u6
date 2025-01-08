using System.Runtime.InteropServices;

namespace Game.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential, Size = 44, Pack = 1)]
    public struct HapticRamp
    {
        /* Header */
        /// <summary>
        /// SDL_HAPTIC_RAMP
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

        /* Ramp */
        /// <summary>
        /// Beginning strength level.
        /// </summary>
        public short start;
        /// <summary>
        /// Ending strength level.
        /// </summary>
        public short end;
        
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