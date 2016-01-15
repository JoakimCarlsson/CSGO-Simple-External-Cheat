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

            //var player = Smurf.Objects.Players.First(p => p.Id != Smurf.LocalPlayer.Id);
            //Console.WriteLine(player.GetBonePos((int) player.BaseAddress, 6));
        }
    }
}
