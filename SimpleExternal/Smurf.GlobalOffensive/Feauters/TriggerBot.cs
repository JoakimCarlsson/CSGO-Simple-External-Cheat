using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class TriggerBot
    {
        #region Fields

        public bool AimOntarget;
        private long _triggerLastTarget;
        private long _triggerLastShot;
        private bool _triggerEnabled;
        private bool _triggerAllies;
        private bool _triggerEnemies;
        private bool _spawnProtection;
        private bool _triggerDash;
        private bool _triggerZoomed;
        private bool _inCrossTrigger;
        private int _delayFirstShot;
        private int _delayShots;
        public Vector3 ViewAngels;
        private WinAPI.VirtualKeyShort _triggerKey;
        private IEnumerable<Player> _validTargets;

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_triggerEnabled)
                return;

            if (Core.KeyUtils.KeyIsDown(_triggerKey))
            {
                ViewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));

                if (_triggerZoomed)
                    if (Core.LocalPlayerWeapon.ZoomLevel == 0)
                        return;

                if (_triggerDash)
                    if (Core.LocalPlayer.Velocity > 0)
                        return;

                if (_inCrossTrigger)
                {
                    InCrossTriggerBot();
                }
                else
                {
                    FaceItTriggerBot();
                }
            }
            else
                AimOntarget = false;
        }

        private void InCrossTriggerBot()
        {
                BaseEntity target = Core.LocalPlayer.Target;
                if (target != null && ((_triggerAllies && target.Team == Core.LocalPlayer.Team) || (_triggerEnemies && target.Team != Core.LocalPlayer.Team)))
                {
                    if (!AimOntarget)
                    {
                        AimOntarget = true;
                        _triggerLastTarget = DateTime.Now.Ticks;
                    }
                    else
                    {
                        if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastTarget).TotalMilliseconds >= _delayFirstShot))
                            return;
                        if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _delayShots))
                            return;

                        _triggerLastShot = DateTime.Now.Ticks;

                        if (_spawnProtection)
                            if (target.GunGameImmune)
                                return;

                        Shoot();
                    }
                }
        }

        private void FaceItTriggerBot()
        {
            GetValidTargets();
            foreach (Player validTarget in _validTargets)
            {
                Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;

                for (int i = 0; i < 81; i++)
                {
                    Vector3 aimView = validTarget.GetBonePos(validTarget, i);
                    Vector3 dst = myView.CalcAngle(aimView);
                    dst = dst.NormalizeAngle();
                    var fov = MathUtils.Fov(ViewAngels, dst, Vector3.Distance(Core.LocalPlayer.Position, validTarget.Position));
                    if (fov <= 4)
                    {
                        if (!AimOntarget)
                        {
                            AimOntarget = true;
                            _triggerLastTarget = DateTime.Now.Ticks;
                        }
                        else
                        {
                            if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastTarget).TotalMilliseconds >= _delayFirstShot))
                                return;
                            if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _delayShots))
                                return;

                            _triggerLastShot = DateTime.Now.Ticks;

                            Shoot();
                        }
                    }
                }
            }
        }

        private void GetValidTargets()
        {
            _validTargets = Core.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Core.LocalPlayer.Id && p.SeenBy(Core.LocalPlayer));
            if (_triggerEnemies)
                _validTargets = _validTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_triggerAllies)
                _validTargets = _validTargets.Where(p => p.Team == Core.LocalPlayer.Team);
            if (_spawnProtection)
                _validTargets = _validTargets.Where(p => !p.GunGameImmune);
        }

        private void ReadSettings()
        {
            try
            {
                _triggerEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Enabled");
                _triggerKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Trigger Key"), 16);
                _triggerEnemies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Enemies");
                _triggerAllies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Allies");
                _spawnProtection = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Spawn Protected");
                _delayFirstShot = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay FirstShot");
                _delayShots = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay Shots");
                _triggerDash = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Dash");
                _triggerZoomed = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger When Zoomed");
                _inCrossTrigger = Core.Settings.GetBool("Misc", "InCross Trigger Bot");
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
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
        #endregion
    }

}
