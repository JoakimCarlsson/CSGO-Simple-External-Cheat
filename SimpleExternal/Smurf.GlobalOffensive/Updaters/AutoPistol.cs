﻿using System;
using System.Threading;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class AutoPistol
    {
        private bool _autoPistol;
        private int _delay;
        private long _lastShot;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;
            if (Smurf.LocalPlayerWeapon.WeaponGroup != "Pistol")
                return;

            ReadSettigns();

            if (!_autoPistol)
                return;

            //TODO Fix so we only shoot if we are active in the csgo window.
            if (Smurf.KeyUtils.KeyIsDown(0x02))
            {
                if (!(new TimeSpan(DateTime.Now.Ticks - _lastShot).TotalMilliseconds >= _delay))
                    return;

                _lastShot = DateTime.Now.Ticks;

                Shoot();
            }
        }

        private void ReadSettigns()
        {
            _autoPistol = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Auto Pistol");
            _delay = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Auto Pistol Delay");
        }

        public void Shoot()
        {
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
    }
}