using System.Text;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.SDK;

namespace Smurf.GlobalOffensive.Utils
{
    class MiscUtils
    {
        public static bool ShouldUpdate(bool checkKnife = true, bool checkGrenades = true, bool checkMisc = true)
        {
            //if (WindowTitle != Smurf.GameTitle)
            //    return false;

            if (Core.LocalPlayer == null)
                return false;

            if (Core.LocalPlayerWeapon == null)
                return false;

            if (Core.Client.State != SignonState.Full)
                return false;

            if (checkMisc)
                if (Core.LocalPlayerWeapon.WeaponType == WeaponType.Unkown)
                    return false;

            if (checkGrenades)
                if (Core.LocalPlayerWeapon.WeaponType == WeaponType.Grenade)
                    return false;

            if (checkKnife)
                if (Core.LocalPlayerWeapon.WeaponType == WeaponType.Knife)
                    return false;

            return true;
        }

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var builder = new StringBuilder(nChars);
            var handle = WinAPI.GetForegroundWindow();

            if (WinAPI.GetWindowText(handle, builder, nChars) > 0)
                return builder.ToString();

            return null;
        }
    }
}
