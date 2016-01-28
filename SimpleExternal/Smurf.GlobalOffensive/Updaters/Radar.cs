using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Radar
    {
        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            foreach (var player in Smurf.Objects.Players)
            {
                if (player.Team == Smurf.LocalPlayer.Team)
                    continue;

                if (!player.IsDormant && !player.IsSpotted)
                    Smurf.Memory.Write(player.BaseAddress + Offsets.BaseEntity.Spotted, 1);
            }
        }
    }
}
