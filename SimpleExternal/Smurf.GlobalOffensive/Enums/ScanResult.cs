using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive.Data.Enums
{
    public struct ScanResult
    {
        public bool Success;
        public IntPtr Address;
        public IntPtr Base;
        public IntPtr Offset;
    }

}

