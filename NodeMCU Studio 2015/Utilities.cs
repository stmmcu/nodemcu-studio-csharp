using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.IO;

namespace NodeMCU_Studio
{
    class Utilities
    {
        public static string EscapeString(string input)
        {
            return input.Replace("\\", "\\\\");
        }
    }
}
