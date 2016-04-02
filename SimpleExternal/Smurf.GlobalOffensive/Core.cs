using System;
using System.Diagnostics;
using System.Threading;
using Memory;
using Smurf.GlobalOffensive.Feauters;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive
{
    public static class Core
    {
        private static bool _isAttached;
        public static int ClientState;
        public const string GameTitle = "Counter-Strike: Global Offensive";

        public static NativeMemory Memory { get; private set; }
        public static Settings Settings { get; set; }
        public static LocalPlayer LocalPlayer => Objects.LocalPlayer;
        public static Weapon LocalPlayerWeapon => Objects.LocalPlayerWeapon;
        public static ObjectManager Objects { get; private set; }
        public static Rcs ControlRecoil { get; set; }
        public static TriggerBot TriggerBot { get; set; }
        public static BunnyJump BunnyJump { get; set; }
        public static SoundEsp SoundEsp { get; set; }
        public static AutoPistol AutoPistol { get; set; }
        public static Radar Radar { get; set; }
        public static Glow Glow { get; set; }
        public static NoFlash NoFlash { get; set; }
        public static Aimbot Aimbot { get; set; }
        public static KeyUtils KeyUtils { get; set; }
        public static GameClient Client { get; private set; }
        public static IntPtr ClientBase { get; private set; }
        public static IntPtr EngineBase { get; private set; }
        //public static Bsp Bsp;
        //private static string serverMap = "de_dust2";

        public static void Attach(Process process, bool isInjected = false)
        {

            if (_isAttached)
                return;

            if (isInjected)
                Memory = new LocalProcessMemory(process);
            else
                Memory = new ExternalProcessMemory(process);

            Thread.Sleep(2000);
            ClientBase = Memory.GetModule("client.dll").BaseAddress;
            EngineBase = Memory.GetModule("engine.dll").BaseAddress;
            ClientState = Memory.Read<int>(EngineBase + Offsets.ClientState.Base);
            Objects = new ObjectManager(ClientBase + Offsets.Misc.EntityList);
            ControlRecoil = new Rcs();
            TriggerBot = new TriggerBot();
            KeyUtils = new KeyUtils();
            BunnyJump = new BunnyJump();
            Settings = new Settings();
            SoundEsp = new SoundEsp();
            Radar = new Radar();
            NoFlash = new NoFlash();
            AutoPistol = new AutoPistol();
            Aimbot = new Aimbot();
            Glow = new Glow();

            var enginePtr = Memory.Read<IntPtr>(EngineBase + Offsets.ClientState.Base);

            if (enginePtr == IntPtr.Zero)
                throw new Exception("Couldn't find Engine Ptr - are you sure your offsets are up to date?");

            Client = new GameClient(enginePtr);
            _isAttached = true;
            //LoadMap();
        }

        //public static void LoadMap()
        //{
        //    string mapPath = @"C:\Games\steamapps\common\Counter-Strike Global Offensive\csgo\maps\"+serverMap+".bsp";
        //    if (File.Exists(mapPath))
        //    {
        //        Console.WriteLine("[BSP] Loading BSP...");
        //        Bsp = new Bsp(mapPath);
        //        Console.WriteLine("[BSP] Loaded, version {0}, map revision {1}\n[{7} brushes]\n[{8} brushsides]\n[{2} faces]\n[{3} originalFaces]\n[{4} surfedges]\n[{5} edges]\n[{6} vertices]\n[{9} nodes]\n[{10} leafs]",
        //            Bsp.Header.version.ToString(),
        //            Bsp.Header.mapRevision.ToString(),
        //            Bsp.Faces.Length.ToString(),
        //            Bsp.OriginalFaces.Length.ToString(),
        //            Bsp.Surfedges.Length.ToString(),
        //            Bsp.Edges.Count.ToString(),
        //            Bsp.Vertices.Length.ToString(),
        //            Bsp.Brushes.Length.ToString(),
        //            Bsp.Brushsides.Length.ToString(),
        //            Bsp.Nodes.Length.ToString(),
        //            Bsp.Leafs.Length.ToString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("[BSP] BSP \"{0}\" not found at \"{1}\"!", serverMap, mapPath);
        //    }
        //}
    }
}