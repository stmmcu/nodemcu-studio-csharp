using System.Runtime.InteropServices;

namespace NodeMCU_Studio_2015
{
    class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern void Sleep(uint time);
    }
}
