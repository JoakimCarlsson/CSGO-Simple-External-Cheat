namespace Smurf.GlobalOffensive.SDK
{
    internal static class Offsets
    {

        public static class Misc
        {
            public static int EntityList = 0x04AC0CA4;
            public static int Jump = 0x04F5782C;
            public static int GlowIndex = 0x0000A320;
            public static int GlowObject = 0x04FD91C4;
            public static int ForceAttack = 0x02F00DF4;
            public static int Sensitivity = 0x00AA40B4;
        }

        public static class ClientState
        {
            public static int Base = 0x005C32C4; //ClientState
            public static int LocalPlayerIndex = 0x00000178;
            public static int GameState = 0x00000100; //Ingame
            public static int ViewAngles = 0x00004D0C;
        }

        public static class BaseEntity
        {
            public static int Position = 0x00000134; //m_vecOrigin
            public static int Team = 0xF0;
            public static int Armor = 0xA8F4;
            public static int Health = 0xFC;
            public static int Dormant = 0xE9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
            public static int Spotted = 0x00000939;
            public static int BoneMatrix = 0x00002698;
            public static int SpottedByMask = 0x0000097C;
        }

        public static class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x00002EE8;
            public static int VecVelocity = 0x110;
            public static int GunGameImmune = 0x000038B0;
            public static int FlashMaxAlpha = 0x0000A304;
        }

        public static class LocalPlayer
        {
            public static int Base = 0x00A9E8E4; //LocalPlayer
            public static int CrosshairId = 0xAA70;
            public static int VecViewOffset = 0x104;
            public static int VecPunch = 0x0000301C;
            public static int ShotsFired = 0x0000A2C0;
        }

        public static class Weapon
        {
            public static int State = 0x000031F8;
            public static int Clip1 = 0x00003204;
            public static int WeaponId = 0x000032EC;
            public static int ZoomLevel = 0x00003350;
        }
    }
}