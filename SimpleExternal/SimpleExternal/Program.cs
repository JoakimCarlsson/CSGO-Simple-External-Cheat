using System;
using System.Diagnostics;

namespace SimpleExternal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Smurf Bot 0.1 \n For local development use only. Do not redistribute");
            Console.Title = "Smurf Bot";

            Process process = Process.GetProcessesByName("csgo")[0];

            Smurf.GlobalOffensive.Smurf.Attach(process);

            while (true)
            {
                Smurf.GlobalOffensive.Smurf.Objects.Update();
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
                    Console.WriteLine("ObjectCount:\t{0}", Smurf.GlobalOffensive.Smurf.Objects.Players.Count);
                    Console.WriteLine("Shots Fired: \t{0}", me.ShotsFired);
                    Console.WriteLine("VecPunch: \t{0}", me.VecPunch);
                    var t = me.Target;
                    Console.WriteLine("Target:\t{0}", t != null ? t.Id.ToString() : "none");
                }
            }
        }
    }
}
