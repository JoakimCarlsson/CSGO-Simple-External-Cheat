using System;
using System.Threading;
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
            if (Core.LocalPlayerWeapon.WeaponGroup != "Pistol")
                return;

            ReadSettigns();

            if (!_autoPistol)
                return;

            //TODO Fix so we only shoot if we are active in the csgo window.
            if (Core.KeyUtils.KeyIsDown(_autoPistolKey))
            {
                if (!(new TimeSpan(DateTime.Now.Ticks - _lastShot).TotalMilliseconds >= _delay))
                    return;

                _lastShot = DateTime.Now.Ticks;

                Shoot();
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

        public void Shoot()
        {
            Thread.Sleep(8);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(8);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
    }
}