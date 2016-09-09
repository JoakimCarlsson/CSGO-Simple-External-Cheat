namespace Smurf.GlobalOffensive
{
    internal static class Offsets
    {

        public static class Misc
        {
            public static int EntityList = 0x04A57EA4;
            public static int LocalPlayer = 0x00A3A43C;
            public static int Jump = 0x04EED318;
            public static int GlowIndex = 0x0000A310;
            public static int GlowObject = 0x04F6DAD4;
            public static int Sensitivity = 0x00A36D0C;
        }

        public static class ClientState
        {
            public static int Base = 0x005BB2C4; //ClientState
            public static int LocalPlayerIndex = 0x178;
            public static int GameState = 0x100; //Ingame
            public static int ViewAngles = 0x4D0C;
        }

        public static class BaseEntity
        {
            public static int Position = 0x134; //m_vecOrigin
            public static int Team = 0xF0;
            public static int Armor = 0xA8F4;
            public static int Health = 0xFC;
            public static int Dormant = 0xE9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
            public static int Spotted = 0x00000939;
            public static int BoneMatrix = 0x2698;
            public static int SpottedByMask = 0x0000097C;
        }

        public static class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x2EE8; // m_hActiveWeapon
            public static int VecVelocity = 0x110;
            public static int GunGameImmune = 0x000038A0;
            public static int FlashMaxAlpha = 0x0000A2F4;
        }

        public static class LocalPlayer
        {
            public static int CrosshairId = 0x0000AA44;
            public static int VecViewOffset = 0x104;
            public static int VecPunch = 0x3018;
            public static int ShotsFired = 0x0000A2B0;
        }

        public static class Weapon
        {
            public static int State = 0x000031E8;
            public static int Clip1 = 0x000031F4;
            public static int WeaponId = 0x000032DC;
            public static int ZoomLevel = 0x00003340;
        }
    }
}