using System.Runtime.InteropServices;

namespace Game.UnityFFB.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FFBDeviceInfo
    {
        public uint ID;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;
        public int MaxEffects;
        public int MaxPlayingEffects;
    }
}