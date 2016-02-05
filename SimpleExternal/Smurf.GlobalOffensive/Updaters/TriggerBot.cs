using System;
using System.Threading;

namespace Smurf.GlobalOffensive.Updaters
{
    public class TriggerBot
    {
        #region Methods

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            ReadSettings();

            if (!_triggerEnabled)
                return;

            if (Smurf.KeyUtils.KeyIsDown(_triggerKey))
            {
                var target = Smurf.LocalPlayer.Target;
                if (target != null &&
                    ((_triggerAllies && target.Team == Smurf.LocalPlayer.Team) ||
                     (_triggerEnemies && target.Team != Smurf.LocalPlayer.Team)))
                {
                    if (!AimOntarget)
                    {
                        AimOntarget = true;
                        _triggerLastTarget = DateTime.Now.Ticks;
                    }
                    else
                    {
                        if (
                            !(new TimeSpan(DateTime.Now.Ticks - _triggerLastTarget).TotalMilliseconds >= _delayFirstShot))
                            return;
                        if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _delayShots))
                            return;
                        _triggerLastShot = DateTime.Now.Ticks;

                        if (_spawnProtection)
                            if (target.GunGameImmune)
                                return;
                        if (_triggerDash)
                            if (Smurf.LocalPlayer.Velocity > 1)
                                return;

                        Shoot();
                    }
                }
            }
            else
                AimOntarget = false;

            #endregion
        }

        private void ReadSettings()
        {
            try
            {
                _triggerEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Enabled");
                _triggerKey =
                    (WinAPI.VirtualKeyShort)
                        Convert.ToInt32(Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Key"), 16);
                _triggerEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Enemies");
                _triggerAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Allies");
                _spawnProtection = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Spawn Protected");
                _delayFirstShot = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Delay FirstShot");
                _delayShots = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Delay Shots");
                _triggerDash = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Dash");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public void Shoot()
        {
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }

        #region Fields

        public bool AimOntarget;
        private long _triggerLastTarget;
        private long _triggerLastShot;
        private bool _triggerEnabled;
        private bool _triggerAllies;
        private bool _triggerEnemies;
        private bool _spawnProtection;
        private bool _triggerDash;
        private int _delayFirstShot;
        private int _delayShots;

        private WinAPI.VirtualKeyShort _triggerKey;

        #endregion
    }
}