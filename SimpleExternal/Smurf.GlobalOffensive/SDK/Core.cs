using System;
using System.Diagnostics;
using System.Threading;
using Memory;
using Smurf.GlobalOffensive.Feauters;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.SDK
{
    public static class Core
    {
        private static bool _isAttached;
        public static int ClientState;
        public const string GameTitle = "Counter-Strike: Global Offensive";
        internal static IntPtr HWnd;


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
        public static AimAssist AimAssist { get; set; }
        public static SkinChanger SkinChanger { get; set; }
        public static KeyUtils KeyUtils { get; set; }
        public static GameClient Client { get; private set; }
        public static IntPtr ClientBase { get; private set; }
        public static IntPtr EngineBase { get; private set; }

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

            SkinChanger = new SkinChanger();
            ControlRecoil = new Rcs();
            TriggerBot = new TriggerBot();
            KeyUtils = new KeyUtils();
            BunnyJump = new BunnyJump();
            Settings = new Settings();
            SoundEsp = new SoundEsp();
            Radar = new Radar();
            NoFlash = new NoFlash();
            AutoPistol = new AutoPistol();
            Glow = new Glow();
            AimAssist = new AimAssist();

            var enginePtr = Memory.Read<IntPtr>(EngineBase + Offsets.ClientState.Base);

            if (enginePtr == IntPtr.Zero)
                throw new Exception("Couldn't find Engine Ptr - are you sure your offsets are up to date?");

            Client = new GameClient(enginePtr);
            _isAttached = true;
        }
    }
}