using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            var targets = Smurf.Objects.Players.Where(p => p.Id != Smurf.LocalPlayer.Id && p.Health > 0  && !p.IsDormant /*&& !p.InAir*/);

            if (true)
                targets = targets.Where(x => x.SeenBy(Smurf.LocalPlayer));
            targets = targets.OrderBy(x => (x.Position - Smurf.LocalPlayer.Position).Length());
            Vector3 closest = Vector3.Zero;
            float closestFov = float.MaxValue;

            foreach (var target in targets)
            {

            }
        }
    }
}
