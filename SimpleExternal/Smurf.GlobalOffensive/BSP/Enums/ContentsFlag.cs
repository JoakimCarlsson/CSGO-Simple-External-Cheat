using System;

namespace Smurf.GlobalOffensive.BSP.Enums
{
    [Flags]
    public enum ContentsFlag
    {
        ContentsEmpty = 0,
        ContentsSolid = 0x1,
        ContentsWindow = 0x2,
        ContentsAux = 0x4,
        ContentsGrate = 0x8,
        ContentsSlime = 0x10,
        ContentsWater = 0x20,
        ContentsMist = 0x40,
        ContentsOpaque = 0x80,
        ContentsTestfogvolume = 0x100,
        ContentsUnused = 0x200,
        ContentsUnused6 = 0x400,
        ContentsTeam1 = 0x800,
        ContentsTeam2 = 0x1000,
        ContentsIgnoreNodrawOpaque = 0x2000,
        ContentsMoveable = 0x4000,
        ContentsAreaportal = 0x8000,
        ContentsPlayerclip = 0x10000,
        ContentsMonsterclip = 0x20000,
        ContentsCurrent0 = 0x40000,
        ContentsCurrent90 = 0x80000,
        ContentsCurrent180 = 0x100000,
        ContentsCurrent270 = 0x200000,
        ContentsCurrentUp = 0x400000,
        ContentsCurrentDown = 0x800000,
        ContentsOrigin = 0x1000000,
        ContentsMonster = 0x2000000,
        ContentsDebris = 0x4000000,
        ContentsDetail = 0x8000000,
        ContentsTranslucent = 0x10000000,
        ContentsLadder = 0x20000000,
        ContentsHitbox = 0x40000000
    }
}
