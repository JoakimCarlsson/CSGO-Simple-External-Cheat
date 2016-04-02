using System.Text;
using Smurf.GlobalOffensive.Enums;

namespace Smurf.GlobalOffensive.Utils
{
    class MiscUtils
    {
        public static bool ShouldUpdate(bool checkKnife = true, bool checkGrenades = true, bool checkMisc = true)
        {
            //What we do here is if we are not inside the csgo window it will not update.
            //if (WindowTitle != Smurf.GameTitle)
            //    return false;

            if (Core.LocalPlayer == null)
                return false;

            if (Core.LocalPlayerWeapon == null)
                return false;

            if (Core.Client.State != SignonState.Full)
                return false;

            if (checkMisc)
                if (Core.LocalPlayerWeapon.ClassName == "none" ||
                    Core.LocalPlayerWeapon.ClassName == "BaseEntity" ||
                    Core.LocalPlayerWeapon.ClassName == "CC4" ||
                    Core.LocalPlayerWeapon.ClassName == "CBreakableProp")
                    return false;

            if (checkGrenades)
                if (Core.LocalPlayerWeapon.ClassName == "CDecoyGrenade" ||
                    Core.LocalPlayerWeapon.ClassName == "CHEGrenade" ||
                    Core.LocalPlayerWeapon.ClassName == "CFlashbang" ||
                    Core.LocalPlayerWeapon.ClassName == "CMolotovGrenade" ||
                    Core.LocalPlayerWeapon.ClassName == "CIncendiaryGrenade" ||
                    Core.LocalPlayerWeapon.ClassName == "CSmokeGrenade")
                    return false;

            if (checkKnife)
                if (Core.LocalPlayerWeapon.ClassName == "CKnife")
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
