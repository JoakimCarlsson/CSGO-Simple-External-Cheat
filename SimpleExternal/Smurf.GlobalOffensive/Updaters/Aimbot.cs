using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            var players = Smurf.Objects.Players.Where(p => p.Id != Smurf.LocalPlayer.Id && !p.IsDormant && p.Health > 0);
            foreach (var player in players)
            {
               //Console.WriteLine("Player ID: {0}" + player.GetBonePos((int)player.BaseAddress, 10), player.Id);    
            }
            //var player = Smurf.Objects.Players.First(p => p.Id != Smurf.LocalPlayer.Id);
            //Console.WriteLine(player.GetBonePos((int) player.BaseAddress, 6));
        }
    }
}
