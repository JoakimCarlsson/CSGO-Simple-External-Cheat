using System;
using System.Diagnostics;
using BlueRain;
using CsGoApplicationAimbot;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;
using Smurf.GlobalOffensive.Updaters;
using DataBase;
using Smurf.GlobalOffensive.Properties;

namespace Smurf.GlobalOffensive
{
    public static class Smurf
    {
        private static bool _isAttached;
        public static int ClientState;
        public static Connection connection = new Connection();
        public static NativeMemory Memory { get; private set; }
        public static Settings Settings;
        public static LocalPlayer LocalPlayer => Objects.LocalPlayer;
        public static Weapon LocalPlayerWeapon => Objects.LocalPlayerWeapon;
        public static ObjectManager Objects { get; private set; }
        public static Rcs ControlRecoil { get; set; }
        public static TriggerBot TriggerBot { get; set; }
        public static SoundManager SoundManager { get; set; }
        public static BunnyJump BunnyJump { get; set; }
        public static SoundESP SoundEsp { get; set; }
        public static Offsets Offsets { get; set; }
        public static KeyUtils KeyUtils { get; set; }
        public static GameClient Client { get; private set; }
        public static IntPtr ClientBase { get; private set; }
        public static IntPtr EngineBase { get; private set; }

        /// <summary>
        ///     Initializes Orion by attaching to the specified CSGO process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="isInjected">if set to <c>true</c> [is injected].</param>
        public static void Attach(Process process, bool isInjected = false)
        {
            if (_isAttached)
                return;

            // We won't require the injector for now - we're completely passive.
            if (isInjected)
                Memory = new LocalProcessMemory(process);
            else
                Memory = new ExternalProcessMemory(process);

            ClientBase = Memory.GetModule("client.dll").BaseAddress;
            EngineBase = Memory.GetModule("engine.dll").BaseAddress;
            ClientState = Memory.Read<int>(EngineBase + Offsets.ClientState.Base);

            //Console.WriteLine(($"Client Base Address: 0x{ClientBase}"));
            //Console.WriteLine(($"Engine Base Address: 0x{EngineBase}"));
            //Console.WriteLine(("Initializing ObjectManager.."));

            Objects = new ObjectManager(ClientBase + Offsets.Misc.EntityList, 128);
            ControlRecoil = new Rcs();
            TriggerBot = new TriggerBot();
            KeyUtils = new KeyUtils();
            BunnyJump = new BunnyJump();
            SoundEsp = new SoundESP();
            Settings = new Settings(true);
            Offsets = new Offsets();
            ManageAudio();
            Offsets.UpdateOffsets();

            var enginePtr = Memory.Read<IntPtr>(EngineBase + Offsets.ClientState.Base);

            //Console.WriteLine($"Engine Pointer: 0x{enginePtr}");

            if (enginePtr == IntPtr.Zero)
                throw new Exception("Couldn't find Engine Ptr - are you sure your offsets are up to date?");

            //Console.WriteLine(("Initializing GameClient.."));

            Client = new GameClient(enginePtr);

           //Console.WriteLine($"Smurf attached successfully to process with ID {process.Id}.");

            _isAttached = true;
        }

        public static string GetUsername()
        {
            var Settings1 = new Settings();
            return Settings1.GetString("User", "Username");
        }
        public static string GetPassword()
        {
            var Settings1 = new Settings();
            return Settings1.GetString("User", "Password");
        }
        private static void ManageAudio()
        {
            SoundManager = new SoundManager(2);
            SoundManager.Add(0, Resources.heartbeatloop );
            SoundManager.Add(1, Resources.beep);
        }
    }
}
