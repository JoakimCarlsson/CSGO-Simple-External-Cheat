namespace Smurf.GlobalOffensive.Patchables
{
    class Offsets
    {
        public class Misc
        {
            public static int EntityList = 0x04A5B8F4;
            public static int ViewMatrix = 0x04A4D504;
            public static int LocalPlayer = 0x00A6D444;
            public static int Jump = 0x04AF0464;
        }

        public class ClientState
        {
            //ClientState
            public static int Base = 0x006062B4;
            public static int LocalPlayerIndex = 0x00000178;
            //Ingame
            public static int GameState = 0x100;
            public static int ViewAngles = 0x00004D0C;
        }

        public class GameResources
        {

        }

        public class BaseEntity
        {
            //m_vecOrigin
            public static int Position = 0x134;
            public static int Team = 0xF0;
            public static int Armor = 0xC4F4;
            public static int Health = 0xFC;
            public static int Dormant = 0xE9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
            public static int BoneMatrix = 0x000042A8;
        }

        public class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x00004AF8; // m_hActiveWeapon
            public static int VecVelocity = 0x00000110;
            public static int GunGameImmune = 0x54A0; 
        }
        public class LocalPlayer
        {
            public static int CrosshairId = 0x0000C550;
            public static int VecViewOffset = 0x00000104;
            public static int VecPunch = 0x00004C28;
            public static int ShotsFired = 0x0000BEB0;
        }
        public class Weapon
        {
            public static int ItemDefinitionIndex = 0x000032B4;
            public static int State = 0x00004DF8;
            public static int Clip1 = 0x00004E04;
            public static int NextPrimaryAttack = 0x00004DD8;
            public static int WeaponId = 0x00004EEC;
            public static int CanReload = 0x00004E45;
            public static int WeaponTableIndex = 0x00004E70;
            public static int AccuracyPenalty = 0x00004EC0;
            public static int ZoomLevel = 0x00004F40;
        }
    }
}
