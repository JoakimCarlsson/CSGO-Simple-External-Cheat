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

            _bunnyJumpEnabled = Smurf.Settings.GetBool("Bunny Jump", "Bunny Jump Enabled");
            _bunnyJumpKey = Smurf.Settings.GetKey("Bunny Jump", "Bunny Jump Key");

            if (!_bunnyJumpEnabled)
                return;

            if (Smurf.LocalPlayer.GetVelocity() <= 0)
                return;

            if (Smurf.KeyUtils.KeyIsDown(_bunnyJumpKey))
            {
                Smurf.Memory.Write(Smurf.ClientBase + Offsets.Misc.Jump, Smurf.LocalPlayer.InAir ? 4 : 5);
            }
        }
        #endregion

        #region Fields
        private bool _bunnyJumpEnabled;
        private WinAPI.VirtualKeyShort _bunnyJumpKey;
        #endregion
    }
}
