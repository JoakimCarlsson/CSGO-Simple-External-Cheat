using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace SimpleExternal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread thread1 = new Thread(UpdateInfo);
            Thread thread2 = new Thread(UpdateBHop);
            Thread thread3 = new Thread(UpdateRCS);
            Thread thread4 = new Thread(UpdateSettings);


            Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("Smurf Bot 0.1 \n For local development use only. Do not redistribute");
            Console.Title = "Smurf Bot";

            Process process = Process.GetProcessesByName("csgo")[0];
            Smurf.GlobalOffensive.Smurf.Attach(process);

            StartThreads(thread1, thread2, thread3, thread4);

            while (true)
            {
                //TODO add more threads
                Smurf.GlobalOffensive.Smurf.Objects.Update();
                Smurf.GlobalOffensive.Smurf.TriggerBot.Update();
                Smurf.GlobalOffensive.Smurf.KeyUtils.Update();
                Smurf.GlobalOffensive.Smurf.Aimbot.Update();
                Smurf.GlobalOffensive.Smurf.SoundEsp.Update();
                Thread.Sleep(10);
            }
        }

        private static void StartThreads(Thread thread1, Thread thread2, Thread thread3, Thread thread4)
        {
            List<Thread> threads = new List<Thread> { thread1, thread2, thread3, thread4 };

            foreach (Thread thread in threads)
            {
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }

        private static void UpdateInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("State: {0}\n\n", Smurf.GlobalOffensive.Smurf.Client.State);

                if (Smurf.GlobalOffensive.Smurf.Client.InGame && Smurf.GlobalOffensive.Smurf.LocalPlayer != null && Smurf.GlobalOffensive.Smurf.LocalPlayerWeapon != null &&Smurf.GlobalOffensive.Smurf.LocalPlayer.IsValid)
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
                    Console.WriteLine("Clip1: \t{0}",myWeapon.Clip1);
                }
                Thread.Sleep(500);
            }
        }
        private static void UpdateBHop()
        {
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.BunnyJump.Update();
                Thread.Sleep(10);
            }
        }
        private static void UpdateRCS()
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
    }
}
