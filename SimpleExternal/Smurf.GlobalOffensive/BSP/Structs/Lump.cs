using Smurf.GlobalOffensive.BSP.Enums;

namespace Smurf.GlobalOffensive.BSP.Structs
{
    public struct Lump
    {
        public int Offset, Length, Version, FourCc;
        public LumpType Type;
    }
}
