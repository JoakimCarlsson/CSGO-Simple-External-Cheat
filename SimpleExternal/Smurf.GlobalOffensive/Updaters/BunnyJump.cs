using System.Threading;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class BunnyJump
    {
        #region Methods
        public void Update()
        {
                if (Smurf.LocalPlayer == null)
                    return;

                ReadSettings();

                if (!_bunnyJumpEnabled)
                    return;

                if (Smurf.LocalPlayer.Velocity <= 100)
                    return;

                BHop();
        }

        private void BHop()
        {
            if (Smurf.KeyUtils.KeyIsDown(_bunnyJumpKey))
                Smurf.Memory.Write(Smurf.ClientBase + Offsets.Misc.Jump, Smurf.LocalPlayer.InAir ? 4 : 5);
        }

        private void ReadSettings()
        {
            _bunnyJumpEnabled = Smurf.Settings.GetBool("Bunny Jump", "Bunny Jump Enabled");
            _bunnyJumpKey = Smurf.Settings.GetKey("Bunny Jump", "Bunny Jump Key");
        }
        #endregion

        #region Fields
        private bool _bunnyJumpEnabled;
        private WinAPI.VirtualKeyShort _bunnyJumpKey;
        #endregion
    }
}
