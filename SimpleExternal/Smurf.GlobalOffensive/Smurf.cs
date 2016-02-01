using System;
using System.Diagnostics;
using System.Threading;
using BlueRain;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;
using Smurf.GlobalOffensive.Properties;
using Smurf.GlobalOffensive.Updaters;

namespace Smurf.GlobalOffensive
{
    public static class Smurf
    {
        private static bool _isAttached;
        public static int ClientState;

        public static NativeMemory Memory { get; private set; }
        public static Settings Settings { get; set; }
        public static LocalPlayer LocalPlayer => Objects.LocalPlayer;
        public static Weapon LocalPlayerWeapon => Objects.LocalPlayerWeapon;
        public static ObjectManager Objects { get; private set; }
        public static Rcs ControlRecoil { get; set; }
        public static TriggerBot TriggerBot { get; set; }
        public static BunnyJump BunnyJump { get; set; }
        public static SoundESP SoundEsp { get; set; }
        public static AutoPistol AutoPistol { get; set; }
        public static Radar Radar { get; set; }
        public static NoFlash NoFlash { get; set; }
        public static Aimbot Aimbot { get; set; }
        public static SoundManager SoundManager { get; set; }
        public static KeyUtils KeyUtils { get; set; }
        public static Glow Glow { get; set; }
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
            ControlRecoil = new Rcs();
            TriggerBot = new TriggerBot();
            KeyUtils = new KeyUtils();
            BunnyJump = new BunnyJump();
            Settings = new Settings();
            SoundEsp = new SoundESP();
            Radar = new Radar();
            NoFlash = new NoFlash();
            AutoPistol = new AutoPistol();
            Glow = new Glow();
            Aimbot = new Aimbot();
            SoundManager = new SoundManager(2);

            var enginePtr = Memory.Read<IntPtr>(EngineBase + Offsets.ClientState.Base);

            if (enginePtr == IntPtr.Zero)
                throw new Exception("Couldn't find Engine Ptr - are you sure your offsets are up to date?");

            Client = new GameClient(enginePtr);

            SoundManager.Add(0, Resources.beep);
            _isAttached = true;
        }
    }
}