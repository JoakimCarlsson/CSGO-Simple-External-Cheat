using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class NoFlash
    {
        private bool _noFlashActive;
        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_noFlashActive)
                return;

            if (Smurf.LocalPlayer.FlashMaxAlpha > 1)
                Smurf.Memory.Write(Smurf.LocalPlayer.BaseAddress+Offsets.Player.FlashMaxAlpha, 0);
        }

        private void ReadSettings()
        {
            _noFlashActive = Smurf.Settings.GetBool("Misc", "No Flash");
        }
    }
}
