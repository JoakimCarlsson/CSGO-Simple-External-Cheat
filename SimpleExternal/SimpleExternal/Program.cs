using System;
using System.Diagnostics;
using System.Threading;

namespace SimpleExternal
{
    internal class Program
    {
        private static Process[] _processesName;
        private static void Main(string[] args)
        {
           // LicenseGlobal.Seal.Initialize("6A5E0000");
            Thread thread1 = new Thread(PrintInfo);
            Thread thread2 = new Thread(UpdateBHop);
            Thread thread3 = new Thread(UpdateRcs);
            Thread thread4 = new Thread(UpdateSettings);
            Thread thread5 = new Thread(UpdateKeyUtils);


            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Smurf Bot";

            Process process = Process.GetProcessesByName("csgo")[0];
            Smurf.GlobalOffensive.Smurf.Attach(process);
            StartThreads(thread1, thread2, thread3, thread4, thread5);

            while (true)
            {
                Smurf.GlobalOffensive.Smurf.Objects.Update();
                Smurf.GlobalOffensive.Smurf.TriggerBot.Update();
                Smurf.GlobalOffensive.Smurf.SoundEsp.Update();
                Smurf.GlobalOffensive.Smurf.Radar.Update();
                Thread.Sleep(3);
            }
        }

        private static void StartThreads(params Thread[] threads)
        {
            foreach (var thread in threads)
            {
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }
        private static void PrintInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("State: {0}\n\n", Smurf.GlobalOffensive.Smurf.Client.State);
        
                if (Smurf.GlobalOffensive.Smurf.Client.InGame && Smurf.GlobalOffensive.Smurf.LocalPlayer != null && Smurf.GlobalOffensive.Smurf.LocalPlayerWeapon != null && Smurf.GlobalOffensive.Smurf.LocalPlayer.IsValid && Smurf.GlobalOffensive.Smurf.LocalPlayer.IsAlive)
                {
                    var me = Smurf.GlobalOffensive.Smurf.LocalPlayer;
                    var myWeapon = Smurf.GlobalOffensive.Smurf.LocalPlayerWeapon;
        
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("ID:\t\t{0}", me.Id);
                    Console.WriteLine("Health:\t\t{0}", me.Health);
                    Console.WriteLine("Armor:\t\t{0}", me.Armor);
                    Console.WriteLine("Position:\t{0}", me.Position);
                    Console.WriteLine("Team:\t\t{0}", me.Team);
                    Console.WriteLine("Player Count:\t{0}", Smurf.GlobalOffensive.Smurf.Objects.Players.Count);
                    Console.WriteLine("Velocity: \t{0}", me.Velocity);
                    Console.WriteLine("Shots Fired: \t{0}", me.ShotsFired);
                    Console.WriteLine("VecPunch: \t{0}", me.VecPunch);
                    Console.WriteLine("Immune: \t{0}", me.GunGameImmune);
                    Console.WriteLine("Active Weapon: \t{0}", myWeapon.WeaponName);
                    Console.WriteLine("Clip1: \t\t{0}", myWeapon.Clip1);
                    Console.WriteLine("Flags: \t\t{0}", me.Flags);
                    Console.WriteLine("Flash: \t\t{0}", me.FlashMaxAlpha);
                    Console.WriteLine("Weapon Group: \t{0}", myWeapon.WeaponGroup);
                }

                Thread.Sleep(500);
            }
        }
        private static void UpdateBHop()
        {
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.BunnyJump.Update();
                Thread.Sleep(5);
            }
        }
        private static void UpdateRcs()
        {
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.ControlRecoil.Update();
                Thread.Sleep(5);
            }
        }
        private static void UpdateSettings()
        {
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.Settings.Update();
                Thread.Sleep(10);
            }
        }
        private static void UpdateKeyUtils()
        {
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.KeyUtils.Update();
                Smurf.GlobalOffensive.Smurf.NoFlash.Update();
                Smurf.GlobalOffensive.Smurf.AutoPistol.Update();
                Thread.Sleep(10);
            }
        }
    }
}
