using System.Runtime.InteropServices;

namespace Plugins.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public struct HapticLeftRight
    {
        /* Header */
        /// <summary>
        /// SDL_HAPTIC_LEFTRIGHT
        /// </summary>
        public ushort type;
        
        /* Replay */
        public uint length;

        /* Rumble */
        /// <summary>
        /// Control of the large controller motor.
        /// </summary>
        public ushort large_magnitude;
        /// <summary>
        /// Control of the small controller motor.
        /// </summary>
        public ushort small_magnitude;
    }
}