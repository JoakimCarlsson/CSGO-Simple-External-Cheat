using System;

namespace Smurf.GlobalOffensive.BSP.Enums
{
    [Flags]
    public enum SurfFlag
    {
        SurfLight = 0x1,
        SurfSky2D = 0x2,
        SurfSky = 0x4,
        SurfWarp = 0x8,
        SurfTrans = 0x10,
        SurfNoportal = 0x20,
        SurfTrigger = 0x40,
        SurfNodraw = 0x80,
        SurfHint = 0x100,
        SurfSkip = 0x200,
        SurfNolight = 0x400,
        SurfBumplight = 0x800,
        SurfNoshadows = 0x1000,
        SurfNodecals = 0x2000,
        SurfNochop = 0x4000,
        SurfHitbox = 0x8000
    }
}
