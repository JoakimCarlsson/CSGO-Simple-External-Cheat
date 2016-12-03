using System;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class AutoPistol
    {
        private bool _autoPistol;
        private WinAPI.VirtualKeyShort _autoPistolKey;
        private int _delay;
        private long _lastShot;

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            if (Core.LocalPlayerWeapon.WeaponType != WeaponType.Pistol)
                return;

            ReadSettigns();

            if (!_autoPistol)
                return;

            if (Core.KeyUtils.KeyIsDown(_autoPistolKey))
            {
                if (!(new TimeSpan(DateTime.Now.Ticks - _lastShot).TotalMilliseconds >= _delay))
                    return;

                _lastShot = DateTime.Now.Ticks;

                Engine.ForceAttack(0, 12, 10);
            }
        }

        private void ReadSettigns()
        {
            try
            {
                _autoPistol = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Auto Pistol");
                _delay = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Auto Pistol Delay");
                _autoPistolKey = (WinAPI.VirtualKeyShort) Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Auto Pistol Key"), 16);
            }
            catch (Exception e)
            {
                #if DEBUG
                Console.WriteLine(e.Message);
                #endif

            }
        }
    }
}