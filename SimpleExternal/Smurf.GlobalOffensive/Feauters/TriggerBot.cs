using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool _triggerSpawnProtection;
        private bool _triggerDash;
        private bool _triggerZoomed;
        private bool _inCrossTrigger;
        public int _triggerDelayFirstRandomize;
        public int _triggerDelayShotsRandomize;
        private int _triggerDelayFirstShotMax;
        private int _triggerDelayFirstShotMin;
        private int _triggerDelayShotsMax;
        private int _triggerDelayShotsMin;
        public Vector3 ViewAngels;
        private WinAPI.VirtualKeyShort _triggerKey;
        private IEnumerable<Player> _validTargets;
        private bool randomized;

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

                RandomizeDelay();

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
                    if (!CheckDelay())
                        return;

                    _triggerLastShot = DateTime.Now.Ticks;

                    if (!_triggerSpawnProtection)
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
                    var fov = MathUtils.Fov(ViewAngels, dst, Vector3.DistanceTo(Core.LocalPlayer.Position, validTarget.Position));
                    if (fov <= 4)
                    {
                        if (!AimOntarget)
                        {
                            AimOntarget = true;
                            _triggerLastTarget = DateTime.Now.Ticks;
                        }
                        else
                        {
                            if (!CheckDelay())
                                return;

                            _triggerLastShot = DateTime.Now.Ticks;

                            Shoot();
                        }
                    }
                }
            }
        }

        private bool CheckDelay()
        {
            if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastTarget).TotalMilliseconds >= _triggerDelayFirstRandomize))
                return false;
            if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _triggerDelayShotsRandomize))
                return false;

            return true;
        }

        private void GetValidTargets()
        {
            _validTargets = Core.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Core.LocalPlayer.Id && p.SeenBy(Core.LocalPlayer));
            if (_triggerEnemies)
                _validTargets = _validTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_triggerAllies)
                _validTargets = _validTargets.Where(p => p.Team == Core.LocalPlayer.Team);
            if (!_triggerSpawnProtection)
                _validTargets = _validTargets.Where(p => !p.GunGameImmune);
        }

        private void RandomizeDelay()
        {
            _triggerDelayFirstRandomize = new Random().Next(_triggerDelayFirstShotMin, _triggerDelayFirstShotMax) + 1;
            _triggerDelayShotsRandomize = new Random().Next(_triggerDelayShotsMin, _triggerDelayShotsMax) + 1;
        }

        private void ReadSettings()
        {
            try
            {
                _triggerEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Enabled");
                _triggerKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Trigger Key"), 16);
                _triggerEnemies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Enemies");
                _triggerAllies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Allies");
                _triggerSpawnProtection = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Trigger Spawn Protected");
                _triggerDelayFirstShotMax = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay FirstShot Max");
                _triggerDelayFirstShotMin = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay FirstShot Min");
                _triggerDelayShotsMax = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay Shots Max");
                _triggerDelayShotsMin = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Trigger Delay Shots Min");
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
