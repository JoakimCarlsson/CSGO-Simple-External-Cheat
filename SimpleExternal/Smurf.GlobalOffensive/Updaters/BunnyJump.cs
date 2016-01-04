using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class BunnyJump
    {
        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            if (!_bunnyJumpEnabled)
                return;

            if (Smurf.LocalPlayer.GetVelocity() <= 0)
            return;

            if (Smurf.KeyUtils.KeyIsDown(32)) //Space
            {
                Smurf.Memory.Write(Smurf.ClientBase + Offsets.Misc.Jump, Smurf.LocalPlayer.Flags == 256 ? 4 : 5);
            }
        }

        private bool _bunnyJumpEnabled = true;
    }
}
