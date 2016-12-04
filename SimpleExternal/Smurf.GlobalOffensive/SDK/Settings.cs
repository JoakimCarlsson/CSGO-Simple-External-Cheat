using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;

namespace Smurf.GlobalOffensive.SDK
{
    public class Settings
    {
        #region Fields
        private static readonly FileIniDataParser Parser = new FileIniDataParser();
        private IniData _data;
        private WinAPI.VirtualKeyShort _reloadConfigKey;
        public static string Path;

        #endregion

        #region Constructor
        public Settings()
        {
            Path = GetProcessPath("csgo");
            if (!File.Exists(Path))
            {
                CreateConfigFile();
            }
            _data = Parser.ReadFile(Path);
        }
        #endregion

        #region Methods
        public void Update()
        {
            _reloadConfigKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString("Misc", "Reload Config Key"), 16);
            if (Core.KeyUtils.KeyWentDown(_reloadConfigKey))
            {
                _data = Parser.ReadFile(Path);
            }
        }

        private void CreateConfigFile()
        {
            #region WeaponList
            List<string> snipersList = new List<string>
            {
                "AWP",
                "SSG08",
                "SCAR20",
                "G3SG1"
            };
            List<string> machineGunList = new List<string>
            {
                "M249",
                "Negev"
            };

            List<string> heavyList = new List<string>
            {
                "NOVA",
                "XM1014",
                "Sawedoff",
                "Mag7"
            };

            List<string> smgList = new List<string>
            {
                "MAC10",
                "MP9",
                "MP7",
                "UMP45",
                "Bizon",
                "P90"
            };

            List<string> pistolList = new List<string>
            {
                "DEagle",
                "Elite",
                "FiveSeven",
                "Glock",
                "P228",
                "P250",
                "HKP2000",
                "Tec9"
            };

            List<string> rifleList = new List<string>
            {
                "GalilAR",
                "AK47",
                "SG556",
                "Famas",
                "M4A1",
                "Aug"
            };
            #endregion

            StringBuilder builder = new StringBuilder();
            //Bunny Jump
            builder.AppendLine("[Bunny Jump]");
            builder.AppendLine("Bunny Jump Enabled = True");
            builder.AppendLine("Bunny Jump Key = 0x20").AppendLine();

            //Glow
            builder.AppendLine("[Glow ESP]");
            builder.AppendLine("Glow ESP Enabled = False");
            builder.AppendLine("Glow ESP Allies = False");
            builder.AppendLine("Glow ESP Enemies = False").AppendLine();

            //SoundESP
            builder.AppendLine("[Sound ESP]");
            builder.AppendLine("Sound ESP = True");
            builder.AppendLine("Sound Range = 10");
            builder.AppendLine("Sound Interval = 1000");
            builder.AppendLine("Sound Volume = 100").AppendLine();

            //Skin Changer
            builder.AppendLine("[Skin Changer]");
            builder.AppendLine("Force Update Key = 0x24");
            builder.AppendLine("Skin Changer = True");
            builder.AppendLine("Skin Changer StatTrak = False");
            builder.AppendLine("Skin Changer StatTrak Count = 1337");
            builder.AppendLine("Knife Changer = False");
            builder.AppendLine("Deagle = 328");
            builder.AppendLine("Duals = 28");
            builder.AppendLine("Fiveseven = 427");
            builder.AppendLine("Glock = 353");
            builder.AppendLine("Ak47 = 180");
            builder.AppendLine("Aug = 39");
            builder.AppendLine("Awp = 344");
            builder.AppendLine("Famas = 492");
            builder.AppendLine("G3Sg1 = 39");
            builder.AppendLine("Galilar = 39");
            builder.AppendLine("M249 = 39");
            builder.AppendLine("M4A4 = 309");
            builder.AppendLine("Mac10 = 433");
            builder.AppendLine("P90 = 39");
            builder.AppendLine("Ump45 = 39");
            builder.AppendLine("Xm1014 = 39");
            builder.AppendLine("Bizon = 39");
            builder.AppendLine("Mag7 = 39");
            builder.AppendLine("Negev = 39");
            builder.AppendLine("Sawedoff = 39");
            builder.AppendLine("P2000 = 39");
            builder.AppendLine("Tec9 = 39");
            builder.AppendLine("Mp7 = 39");
            builder.AppendLine("Mp9 = 39");
            builder.AppendLine("Nova = 39");
            builder.AppendLine("P250 = 39");
            builder.AppendLine("Scar20 = 39");
            builder.AppendLine("Sg556 = 39");
            builder.AppendLine("Ssg08 = 39");
            builder.AppendLine("M4A1 = 326");
            builder.AppendLine("Usp = 313");
            builder.AppendLine("Cz75A = 270");
            builder.AppendLine("Revolver = 39");
            builder.AppendLine("Knife = 409");

            //Misc
            builder.AppendLine("[Misc]");
            builder.AppendLine("Mouse Movement = False");
            builder.AppendLine("Radar = True");
            builder.AppendLine("InCross Trigger Bot = False");
            builder.AppendLine("Bone Trigger Bot = False");
            builder.AppendLine("Hitbox Trigger Bot = True");
            builder.AppendLine("No Flash = False");
            builder.AppendLine("Reload Config Key = 0x35").AppendLine();

            foreach (var weapon in pistolList)
            {
                builder.AppendLine("[" + weapon + "]");
                //Auto Pistol
                builder.AppendLine("Auto Pistol = False");
                builder.AppendLine("Auto Pistol Key = 0x12");
                builder.AppendLine("Auto Pistol Delay = 0").AppendLine();
                //RCS
                builder.AppendLine("Rcs Enabled = False");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = True");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 128");
                builder.AppendLine("Trigger Delay FirstShot Min = 98");
                builder.AppendLine("Trigger Delay Shots Max = 68");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }
            foreach (var weapon in rifleList)
            {
                builder.AppendLine("[" + weapon + "]");
                //RCS
                builder.AppendLine("Rcs Enabled = True");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = False");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Burst Enabled = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 35");
                builder.AppendLine("Trigger Delay FirstShot Min = 35");
                builder.AppendLine("Trigger Delay Shots Max = 35");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }
            foreach (var weapon in smgList)
            {
                builder.AppendLine("[" + weapon + "]");
                //RCS
                builder.AppendLine("Rcs Enabled = True");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = True");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Burst Enabled = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 35");
                builder.AppendLine("Trigger Delay FirstShot Min = 35");
                builder.AppendLine("Trigger Delay Shots Max = 35");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }
            foreach (var weapon in snipersList)
            {
                builder.AppendLine("[" + weapon + "]");
                //RCS
                builder.AppendLine("Rcs Enabled = False");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = True");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Burst Enabled = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 128");
                builder.AppendLine("Trigger Delay FirstShot Min = 98");
                builder.AppendLine("Trigger Delay Shots Max = 68");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }
            foreach (var weapon in machineGunList)
            {
                builder.AppendLine("[" + weapon + "]");
                //RCS
                builder.AppendLine("Rcs Enabled = False");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = True");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Burst Enabled = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 35");
                builder.AppendLine("Trigger Delay FirstShot Min = 35");
                builder.AppendLine("Trigger Delay Shots Max = 35");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }
            foreach (var weapon in heavyList)
            {
                builder.AppendLine("[" + weapon + "]");
                //RCS
                builder.AppendLine("Rcs Enabled = False");
                builder.AppendLine("Rcs Start = 1");
                builder.AppendLine("Rcs Force Max Yaw = 2");
                builder.AppendLine("Rcs Force Min Yaw = 2");
                builder.AppendLine("Rcs Force Max Pitch = 2");
                builder.AppendLine("Rcs Force Min Pitch = 2").AppendLine();
                //Trigger
                builder.AppendLine("Trigger Enabled = True");
                builder.AppendLine("Trigger Key = 0x12");
                builder.AppendLine("Trigger Dash = False");
                builder.AppendLine("Trigger When Zoomed = False");
                builder.AppendLine("Trigger Enemies = True");
                builder.AppendLine("Trigger Allies = False");
                builder.AppendLine("Trigger Burst Enabled = False");
                builder.AppendLine("Trigger Spawn Protected = False");
                builder.AppendLine("Trigger Delay FirstShot Max = 35");
                builder.AppendLine("Trigger Delay FirstShot Min = 35");
                builder.AppendLine("Trigger Delay Shots Max = 35");
                builder.AppendLine("Trigger Delay Shots Min = 35").AppendLine();

                //Aim Assist
                builder.AppendLine("Aim Enabled = True");
                builder.AppendLine("Aim Key = 0x12");
                builder.AppendLine("Aim Fov = 50");
                builder.AppendLine("Aim Humanized = True");
                builder.AppendLine("Aim Spotted = True");
                builder.AppendLine("Aim Enemies = True");
                builder.AppendLine("Aim Allies = False");
                builder.AppendLine("Aim Speed = 50");
                builder.AppendLine("Aim Bone = 5").AppendLine();
            }


            if (!File.Exists(Path))
            {
                using (StreamWriter streamWriter = new StreamWriter(Path))
                {
                    streamWriter.WriteLine(builder);
                }
            }
        }

        public string GetProcessPath(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            if (processes.Length > 0)
            {
                string tempString = processes[0].MainModule.FileName;
                //Ugly way to hardcode it, but the process name will never change.
                tempString = tempString.Replace("csgo.exe", "");
                tempString += "Config.ini";
                return tempString;
            }
            else
            {
                return string.Empty;
            }
        }

        public int GetInt(string section, string key)
        {
            try
            {
                var keyValue = _data[section][key];
                var setting = int.Parse(keyValue);
                return setting;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error: {0},\nSection: {1}\nKey: {2}", e.Message, section, key);
#endif

            }
            return 0;
        }

        public string GetString(string section, string key)
        {
            try
            {
                var keyValue = _data[section][key];
                var setting = keyValue;
                return setting;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error: {0},\nSection: {1}\nKey: {2}", e.Message, section, key);
#endif

            }
            return "M4A1";
        }

        public uint GetUInt(string section, string key)
        {
            var keyValue = _data[section][key];
            var setting = uint.Parse(keyValue);
            return setting;
        }

        public float GetFloat(string section, string key)
        {
            try
            {
                var keyValue = _data[section][key];
                var setting = float.Parse(keyValue);
                return setting;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error: {0},\nSection: {1}\nKey: {2}", e.Message, section, key);
#endif

            }
            return 0;
        }

        public bool GetBool(string section, string key)
        {
            try
            {
                var keyValue = _data[section][key];
                var setting = bool.Parse(keyValue);
                return setting;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error: {0},\nSection: {1}\nKey: {2}", e.Message, section, key);
#endif

            }
            return false;
        }

        public WinAPI.VirtualKeyShort GetKey(string section, string key)
        {
            try
            {
                var keyValue = _data[section][key];
                var button = (WinAPI.VirtualKeyShort)int.Parse(keyValue);
                return button;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error: {0},\nSection: {1}\nKey: {2}", e.Message, section, key);
#endif

            }
            return WinAPI.VirtualKeyShort.ACCEPT;
        }
        #endregion
    }
}