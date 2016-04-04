namespace Smurf.GlobalOffensive
{
    internal class Offsets
    {

        public class Misc
        {
            public static int ViewMatrix = 0x4A8CFF4;
            public static int EntityList = 0x4A9B4E4;
            public static int LocalPlayer = 0xA804CC;
            public static int Jump = 0x4F30168;
            public static int GlowIndex = 0xA310;
            public static int GlowObject = 0x4FB0C2C;
            public static int Sensitivity = 0xA85CD4;
        }

        public class ClientState
        {
            public static int Base = 0x610344; //ClientState
            public static int LocalPlayerIndex = 0x178;
            public static int GameState = 0x100; //Ingame
            public static int ViewAngles = 0x4D0C;
        }

        public class BaseEntity
        {
            public static int Position = 0x134; //m_vecOrigin
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

        public class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x2EE8; // m_hActiveWeapon
            public static int VecVelocity = 0x110;
            public static int GunGameImmune = 0x38A0;
            public static int FlashMaxAlpha = 0xA2F4;
        }

        public class LocalPlayer
        {
            public static int CrosshairId = 0xA954;
            public static int VecViewOffset = 0x104;
            public static int VecPunch = 0x3018;
            public static int ShotsFired = 0xA2B0;
        }

        public class Weapon
        {
            public static int State = 0x31E8;
            public static int Clip1 = 0x31F4;
            public static int WeaponId = 0x32DC;
            public static int ZoomLevel = 0x3330;
        }
    }
}