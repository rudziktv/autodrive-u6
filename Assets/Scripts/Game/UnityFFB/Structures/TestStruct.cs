using System.Runtime.InteropServices;

namespace Game.UnityFFB.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TestStruct
    {
        public int a;
        public InnerTestStruct inner;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct InnerTestStruct
    {
        public int b;
    }
}