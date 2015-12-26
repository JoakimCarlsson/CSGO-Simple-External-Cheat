using System;
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

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Smurf Bot 0.1 \n For local development use only. Do not redistribute");
            Console.Title = "Smurf Bot";

            Process process = Process.GetProcessesByName("csgo")[0];
            Smurf.GlobalOffensive.Smurf.Attach(process);

            thread1.IsBackground = true;
            thread1.Priority = ThreadPriority.Highest;
            thread1.Start();
            
            while (true)
            {
                Smurf.GlobalOffensive.Smurf.Objects.Update();
                Smurf.GlobalOffensive.Smurf.ControlRecoil.Update();

                Thread.Sleep(5);
            }
        }

        private static void UpdateInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("State: {0}\n\n", Smurf.GlobalOffensive.Smurf.Client.State);

                if (Smurf.GlobalOffensive.Smurf.Client.InGame && Smurf.GlobalOffensive.Smurf.LocalPlayer != null && Smurf.GlobalOffensive.Smurf.LocalPlayer.IsValid)
                {
                    var me = Smurf.GlobalOffensive.Smurf.LocalPlayer;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("ID:\t\t{0}", me.Id);
                    Console.WriteLine("Health:\t\t{0}", me.Health);
                    Console.WriteLine("Armor:\t\t{0}", me.Armor);
                    Console.WriteLine("Position:\t{0}", me.Position);
                    Console.WriteLine("Team:\t\t{0}", me.Team);
                    Console.WriteLine("Player Count:\t{0}", Smurf.GlobalOffensive.Smurf.Objects.Players.Count);
                    Console.WriteLine("Weapon Count:\t{0}", Smurf.GlobalOffensive.Smurf.Objects.Weapons.Count);
                    Console.WriteLine("Entity Count:\t{0}", Smurf.GlobalOffensive.Smurf.Objects.Entities.Count);
                    Console.WriteLine("Shots Fired: \t{0}", me.ShotsFired);
                    Console.WriteLine("VecPunch: \t{0}", me.VecPunch);
                    var t = me.Target;
                    Console.WriteLine("Target:\t{0}", t != null ? t.Id.ToString() : "none");

                    Thread.Sleep(500);
                }
            }
        }
    }
}
