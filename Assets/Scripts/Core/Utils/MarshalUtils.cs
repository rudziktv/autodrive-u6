using System;
using System.Runtime.InteropServices;

namespace Core.Utils
{
    public static class MarshalUtils
    {
        public static IntPtr AllocStructureToPtr<T>(T structure, bool oldDelete = false)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, ptr, oldDelete);
            return ptr;
        }
    }
}