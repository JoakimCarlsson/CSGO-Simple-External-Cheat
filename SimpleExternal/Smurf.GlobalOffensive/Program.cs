using System;
using System.Diagnostics;
using System.Threading;

namespace Smurf.GlobalOffensive
{
    internal class Program
    {
        #region Fields
        private static IntPtr _hWnd;
        private const string GameTitle = "Counter-Strike: Global Offensive";
        #endregion

        #region Methods
        private static void Main()
        {
            Thread thread1 = new Thread(PrintInfo);
            Thread thread2 = new Thread(UpdateBhop);
            Thread thread3 = new Thread(UpdateRcs);
            Thread thread4 = new Thread(UpdateSettings);
            Thread thread5 = new Thread(UpdateKeyUtils);
            Thread thread6 = new Thread(UpdateAutoPistol);
            Thread thread7 = new Thread(UpdateAimbot);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Cheat Squad";

            Console.WriteLine("> Waiting for CSGO to start up...");
            while ((_hWnd = WinAPI.FindWindowByCaption(_hWnd, GameTitle)) == IntPtr.Zero)
                Thread.Sleep(250);

            Console.Clear();

            Process[] process = Process.GetProcessesByName("csgo");
            Core.Attach(process[0]);

            StartThreads(thread1, thread2, thread3, thread4, thread5, thread6, thread7);

            while (true)
            {
                Core.Objects.Update();
                Core.TriggerBot.Update();
                Core.SoundEsp.Update();
                Core.Radar.Update();
                Core.Glow.Update();
                //Core.AimAssist.Update();
                Thread.Sleep(1);
            }
        }
        private static void UpdateAimbot()
        {
            while (true)
            {
                Core.AimAssist.Update();
                Thread.Sleep(1);
            }

        }
        private static void StartThreads(params Thread[] threads)
        {
            foreach (var thread in threads)
            {
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }
        private static void PrintInfo()
        {
#if DEBUG
            while (true)
            {
                Console.Clear();
                Console.WriteLine("State: {0}\n\n", Core.Client.State);

                if (Core.Client.InGame && Core.LocalPlayer != null && Core.LocalPlayerWeapon != null && Core.LocalPlayer.IsValid && Core.LocalPlayer.IsAlive)
                {
                    var me = Core.LocalPlayer;
                    var myWeapon = Core.LocalPlayerWeapon;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("ID:\t\t{0}", me.Id);
                    Console.WriteLine("Health:\t\t{0}", me.Health);
                    Console.WriteLine("Armor:\t\t{0}", me.Armor);
                    Console.WriteLine("Position:\t{0}", me.Position);
                    Console.WriteLine("Team:\t\t{0}", me.Team);
                    Console.WriteLine("Player Count:\t{0}", Core.Objects.Players.Count);
                    Console.WriteLine("Velocity: \t{0}", me.Velocity);
                    Console.WriteLine("Shots Fired: \t{0}", me.ShotsFired);
                    Console.WriteLine("VecPunch: \t{0}", me.VecPunch);
                    Console.WriteLine("Immune: \t{0}", me.GunGameImmune);
                    Console.WriteLine("Active Weapon: \t{0}", myWeapon.WeaponName);
                    Console.WriteLine("Clip1: \t\t{0}", myWeapon.Clip1);
                    Console.WriteLine("Flags: \t\t{0}", me.Flags);
                    Console.WriteLine("Flash: \t\t{0}", me.FlashMaxAlpha);
                    Console.WriteLine("Weapon Group: \t{0}", myWeapon.WeaponGroup);
                    Console.WriteLine("Zoom Level: \t{0}", myWeapon.ZoomLevel);
                    Console.WriteLine("Recoil Control Yaw: \t{0}", Core.ControlRecoil.RandomYaw);
                    Console.WriteLine("Recoil Control Pitch: \t{0}", Core.ControlRecoil.RandomPitch);
                    Console.WriteLine("Trigger Delay First: \t{0}", Core.TriggerBot._triggerDelayFirstRandomize);
                    Console.WriteLine("Trigger Delay Shots1: \t{0}", Core.TriggerBot._triggerDelayShotsRandomize);
                }

                Thread.Sleep(500);
            }
#endif

        }
        private static void UpdateBhop()
        {
            while (true)
            {
                Core.BunnyJump.Update();
                Thread.Sleep(5);
            }
        }
        private static void UpdateRcs()
        {
            while (true)
            {
                Core.ControlRecoil.Update();
                Thread.Sleep(5);
            }
        }
        private static void UpdateSettings()
        {
            while (true)
            {
                Core.Settings.Update();
                Thread.Sleep(10);
            }
        }
        private static void UpdateKeyUtils()
        {
            while (true)
            {
                Core.KeyUtils.Update();
                Core.NoFlash.Update();
                Thread.Sleep(10);
            }
        }
        private static void UpdateAutoPistol()
        {
            while (true)
            {
                Core.AutoPistol.Update();
                Thread.Sleep(1);
            }
        }
        #endregion

    }
}
