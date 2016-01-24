using System;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;

namespace Smurf.GlobalOffensive.Patchables
{
    public class Offsets
    {
        private static readonly FileIniDataParser Parser = new FileIniDataParser();
        private IniData _data;
        public Offsets()
        {
            if (!File.Exists("Offsets.ini"))
                CreateOffsetsFile();

            _data = Parser.ReadFile("Offsets.ini");

        }

        private void CreateOffsetsFile()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[Misc]");
            builder.AppendLine("EntityList = 0x4A5C9C4");
            builder.AppendLine("ViewMatrix = 0x4A4E584");
            builder.AppendLine("LocalPlayer = 0xA6E444");
            builder.AppendLine("Jump = 0x4AF150C").AppendLine();

            builder.AppendLine("[ClientState]");
            builder.AppendLine("ClientState = 0x6072C4"); //Base
            builder.AppendLine("LocalPlayerIndex = 0x178");
            builder.AppendLine("GameState = 0x100");
            builder.AppendLine("ViewAngles = 0x4D0C").AppendLine();

            builder.AppendLine("[BaseEntity]");
            builder.AppendLine("Position = 0x134");
            builder.AppendLine("Team = 0xF0");
            builder.AppendLine("Health = 0xFC");
            builder.AppendLine("Dormant = 0xE9");
            builder.AppendLine("Index = 0x64");
            builder.AppendLine("EntitySize = 0x10").AppendLine();

            builder.AppendLine("[Player]");
            builder.AppendLine("LifeState = 0x25B");
            builder.AppendLine("Flags = 0x100");
            builder.AppendLine("ActiveWeapon = 0x00002EE8");
            builder.AppendLine("VecVelocity = 0x00000110");
            builder.AppendLine("GunGameImmune = 0x00003890").AppendLine();

            builder.AppendLine("[LocalPlayer]");
            builder.AppendLine("CrosshairId = 0x0000A940");
            builder.AppendLine("VecViewOffset = 0x00000104");
            builder.AppendLine("VecPunch = 0x00003018");
            builder.AppendLine("ShotsFired = 0x0000A2A0").AppendLine();

            builder.AppendLine("[Weapon]");
            builder.AppendLine("Clip1 = 0x000031F4");

            if (!File.Exists("Offsets.ini"))
            {
                var sr = new StreamWriter("Offsets.ini");
                sr.WriteLine(builder);
                sr.Close();
            }
        }
        public void UpdateOffsets()
        {
            Misc.EntityList = Convert.ToInt32(GetString("Misc", "EntityList"), 16);
            Misc.ViewMatrix = Convert.ToInt32(GetString("Misc", "ViewMatrix"), 16);
            Misc.LocalPlayer = Convert.ToInt32(GetString("Misc", "LocalPlayer"), 16);
            Misc.Jump = Convert.ToInt32(GetString("Misc", "Jump"), 16);
        }

        public class Misc
        {
            public static int EntityList = 0x4A5C9C4;
            public static int ViewMatrix = 0x4A4E584;
            public static int LocalPlayer = 0xA6E444;
            public static int Jump;
        }

        public class ClientState
        {
            //ClientState
            public static int Base = 0x006072C4;
            public static int LocalPlayerIndex = 0x00000178;
            //Ingame
            public static int GameState = 0x100;
            public static int ViewAngles = 0x00004D0C;
        }

        public class BaseEntity
        {
            //m_vecOrigin
            public static int Position = 0x134;
            public static int Team = 0xF0;
            public static int Health = 0xFC;
            public static int Dormant = 0x000000E9;
            public static int Index = 0x64;
            public static int EntitySize = 0x10;
        }
        public class Player
        {
            public static int LifeState = 0x25B;
            public static int Flags = 0x100;
            public static int ActiveWeapon = 0x00002EE8; // m_hActiveWeapon
            public static int VecVelocity = 0x00000110;
            public static int GunGameImmune = 0x00003890; 
        }
        public class LocalPlayer
        {
            public static int CrosshairId = 0x0000A940;
            public static int VecViewOffset = 0x00000104;
            public static int VecPunch = 0x00003018;
            public static int ShotsFired = 0x0000A2A0;
        }
        public class Weapon
        {
            public static int Clip1 = 0x000031F4;
        }

        public string GetString(string section, string key)
        {
            var keyValue = _data[section][key];
            var setting = keyValue;
            return setting;
        }

    }
}
