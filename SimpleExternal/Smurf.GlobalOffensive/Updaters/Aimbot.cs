using System;
using System.Linq;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        private Player _activeTarget;
        private bool _aimbotActive;
        private int _aimbotBone;
        private int _aimbotFov;
        private WinAPI.VirtualKeyShort _aimbotKey;
        private int _aimbotSmooth;
        private bool _aimEnemies;
        private bool _aimFriendly;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            ReadSettings();

            if (!_aimbotActive)
                return;

            if (Smurf.KeyUtils.KeyIsDown(_aimbotKey))
            {
                if (_activeTarget == null)
                {
                    _activeTarget = GetTarget();
                }
            }
        }

        private Player GetTarget()
        {
            var validTargets =
                Smurf.Objects.Players.Where(p => p.IsValid && p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id);
            validTargets = validTargets.Where(p => p.SeenBy((int) Smurf.LocalPlayer.BaseAddress));
            if (_aimEnemies)
                validTargets = validTargets.Where(p => p.Team != Smurf.LocalPlayer.Team);
            if (_aimFriendly)
                validTargets = validTargets.Where(p => p.Team == Smurf.LocalPlayer.Team);

            _activeTarget = validTargets.FirstOrDefault();

            return null;
        }

        private void ReadSettings()
        {
            try
            {
                _aimbotActive = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
                _aimbotKey =
                    (WinAPI.VirtualKeyShort)
                        Convert.ToInt32(Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Key"), 16);
                _aimbotSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Smooth");
                _aimbotFov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Fov");
                _aimbotBone = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Bone");
                _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Enemies");
                _aimFriendly = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Friendly");
            }
            catch (Exception e)
            {
            }
        }
    }
}