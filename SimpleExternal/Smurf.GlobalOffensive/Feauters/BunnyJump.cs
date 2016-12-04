using System;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class BunnyJump
    {
        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_bunnyJumpEnabled)
                return;

            if (Core.LocalPlayer.Velocity <= 100)
                return;

            BHop();
        }

        private void BHop()
        {
            if (Core.KeyUtils.KeyIsDown(_bunnyJumpKey))
                Core.Memory.Write(Core.ClientBase + Offsets.Misc.Jump, Core.LocalPlayer.InAir ? 4 : 5);
        }

        private void ReadSettings()
        {
            _bunnyJumpEnabled = Core.Settings.GetBool("Bunny Jump", "Bunny Jump Enabled");
            _bunnyJumpKey =
                (WinAPI.VirtualKeyShort) Convert.ToInt32(Core.Settings.GetString("Bunny Jump", "Bunny Jump Key"), 16);
        }

        #endregion

        #region Fields

        private bool _bunnyJumpEnabled;
        private WinAPI.VirtualKeyShort _bunnyJumpKey;

        #endregion
    }
}