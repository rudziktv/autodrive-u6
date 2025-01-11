using System.Runtime.InteropServices;

namespace Plugins.UnityFFB.NativeEffects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HapticDirection
    {
        /// <summary>
        /// The type of encoding.
        /// </summary>
        public byte type;
        
        /// <summary>
        /// The encoded direction.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] dir;
    }
}