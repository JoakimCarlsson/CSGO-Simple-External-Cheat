using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive.Patchables
{
    class Offsets
    {
        public class Misc
        {
            public static int EntityList = 0x04A5A904;
            public static int ViewMatrix = 0x04A4C4C4;
            public static int LocalPlayer = 0x00A6C49C;
        }

        public class ClientState
        {
            //ClientState
            public static int Base = 0x006062B4;
            public static int LocalPlayerIndex = 0x160;
            public static int GameState = 0x100;
            public static int ViewAngles = 0x00004D0C;
        }

        public class GameResources
        {

        }

        public class BaseEntity
        {
            public static int Position = 0x134;
            public static int Team = 0xF0;
            public static int Armor = 0xC4F4;
            public static int Health = 0xFC;
            public static int Dormant = 0xE9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
        }

        public class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
        }
        public class LocalPlayer
        {
            public static int CrosshairId = 0xC550;
            public static int VecViewOffset = 0x00000104;
            public static int VecPunch = 0x00004C28;
            public static int ShotsFired = 0x0000BEB0;
        }
        public class Weapon
        {

        }
    }
}
