namespace Smurf.GlobalOffensive.SDK
{
    internal static class Offsets
    {

        public static class Misc
        {
            public static int EntityList = 0x4AD3A64;
            public static int Jump = 0x4F6A684;
            public static int GlowIndex = 0xA320;
            public static int GlowObject = 0x4FEE5AC;
            public static int ForceAttack = 0x2F13A50;
            public static int Sensitivity = 0xAB5EF4;
        }

        public static class ClientState
        {
            public static int Base = 0x5CC524;
            public static int LocalPlayerIndex = 0x00000178;
            public static int GameState = 0x00000100;
            public static int ViewAngles = 0x00004D0C;
        }

        public static class BaseEntity
        {
            public static int VecOrigin = 0x134;
            public static int Team = 0xF0;
            public static int Armor = 0xA8F4;
            public static int Health = 0xFC;
            public static int Dormant = 0xE9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
            public static int Spotted = 0x939;
            public static int BoneMatrix = 0x2698;
            public static int SpottedByMask = 0x97C;
        }

        public static class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x2EE8;
            public static int VecVelocity = 0x110;
            public static int GunGameImmune = 0x38B0;
            public static int FlashMaxAlpha = 0xA304;
        }

        public static class LocalPlayer
        {
            public static int Base = 0xAB06EC;
            public static int CrosshairId = 0xAA70;
            public static int VecViewOffset = 0x104;
            public static int VecPunch = 0x301C;
            public static int ShotsFired = 0xA2C0;
        }

        public static class Weapon
        {
            public static int State = 0x31F8;
            public static int Clip1 = 0x3204;
            public static int WeaponId = 0x32EC; //3270
            public static int ZoomLevel = 0x3350;
        }
    }
}