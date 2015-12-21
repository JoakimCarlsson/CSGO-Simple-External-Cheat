using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Smurf Bot 0.1 \n For local development use only. Do not redistribute");
            Console.Title = "Smurf Bot";

            Process process = Process.GetProcessesByName("csgo")[0];

            Smurf.Attach(process);

            while (true)
            {
                Smurf.Objects.Update();
                Console.Clear();

                Console.WriteLine("State: {0}\n\n", Smurf.Client.State);

                if (Smurf.Client.InGame && Smurf.Me != null && Smurf.Me.IsValid)
                {
                    var me = Smurf.Me;
                    Console.WriteLine("ID:\t\t{0}", me.Id);
                    Console.WriteLine("Health:\t\t{0}", me.Health);
                    Console.WriteLine("Armor:\t\t{0}", me.Armor);
                    Console.WriteLine("Position:\t{0}", me.Position);
                    Console.WriteLine("Team:\t\t{0}", me.Team);
                    Console.WriteLine("ObjectCount:\t{0}", Smurf.Objects.Players.Count);

                    var t = me.Target;
                    Console.WriteLine("Target:\t{0}", t != null ? t.Id.ToString() : "none");
                }

                Thread.Sleep(500);
            }

        }
    }
}
