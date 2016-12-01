using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class NoFlash
    {
        private bool _noFlashActive;

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_noFlashActive)
                return;



            if (Core.LocalPlayer.FlashMaxAlpha > 1)
                Core.Memory.Write(Core.LocalPlayer.BaseAddress + Offsets.Player.FlashMaxAlpha, 0);
        }

        private void ReadSettings()
        {
            _noFlashActive = Core.Settings.GetBool("Misc", "No Flash");
        }
    }
}