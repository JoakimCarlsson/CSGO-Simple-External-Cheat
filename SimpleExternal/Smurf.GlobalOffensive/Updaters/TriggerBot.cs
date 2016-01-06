using System;
using System.Threading;

namespace Smurf.GlobalOffensive.Updaters
{
    public class TriggerBot
    {
        #region Properties

        public bool AimOntarget { get; set; }
        private long TriggerLastTarget { get; set; }
        private long TriggerLastShot { get; set; }

        #endregion

        #region Fields
        private bool _triggerEnabled;
        private bool _triggerAllies;
        private bool _triggerEnemies;
        private bool _spawnProtection;
        private bool _triggerBurst;
        private bool _triggerBurstRandom;
        private int _burstMin;
        private int _burstMax;
        private int _delayFirstShot;
        private int _delayShots;
        private WinAPI.VirtualKeyShort _triggerKey;
        #endregion

        #region Methods
        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            _triggerEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Enabled");
            _triggerKey = Smurf.Settings.GetKey(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Key");
            _triggerEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Enemies");
            _triggerAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Allies");
            _spawnProtection = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Spawn Protected");
            _triggerBurst = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Burst Enabled");
            _triggerBurstRandom = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Burst Randomize");
            _burstMin = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Burst Shots Min");
            _burstMax = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Burst Shots Max");
            _delayFirstShot = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Delay FirstShot");
            _delayShots = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Trigger Delay Shots");


            if (!_triggerEnabled)
                return;


            if (Smurf.KeyUtils.KeyIsDown(_triggerKey))
            {
                var target = Smurf.LocalPlayer.Target;
                if (target == null)
                {
                    return;
                }

                if ((_triggerAllies && target.Team == Smurf.LocalPlayer.Team) || (_triggerEnemies && target.Team != Smurf.LocalPlayer.Team))
                {
                    if (!AimOntarget)
                    {
                        AimOntarget = true;
                        TriggerLastTarget = DateTime.Now.Ticks;
                    }
                    else
                    {
                        if (!(new TimeSpan(DateTime.Now.Ticks - TriggerLastTarget).TotalMilliseconds >= _delayFirstShot))
                            return;
                        if (!(new TimeSpan(DateTime.Now.Ticks - TriggerLastShot).TotalMilliseconds >= _delayShots))
                            return;

                        TriggerLastShot = DateTime.Now.Ticks;

                        if (_spawnProtection)
                            if (target.GunGameImmune)
                                return;

                        Shoot();
                    }
                }
            }
            else
                AimOntarget = false;

            #endregion
        }

        private static void Shoot()
        {
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
    }
}
