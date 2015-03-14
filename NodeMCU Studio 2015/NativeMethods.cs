using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NodeMCU_Studio_2015
{
    class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern void Sleep(uint time);
    }
}
