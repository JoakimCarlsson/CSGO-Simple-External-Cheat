using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        private static bool _enabled;
        private static WinAPI.VirtualKeyShort _aimKey;
        private static int _fov;
        private static int _bones;
        private static int _aimSmooth;
        private static bool _aimSpotted;
        private static bool _aimAllies;
        private static bool _aimEnemies;
        private static Vector3 _newViewAngles;
        private static Player activeTarget;

        public IEnumerable<Player> ValidTargets;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            if (Smurf.LocalPlayerWeapon.Clip1 <= 0)
                return;

            ReadSettings();

            if (!_enabled)
                return;

            //TODO get targets while we press the aimkey, this is just for debugging.
            ValidTargets = GetTargets();
            if (Smurf.KeyUtils.KeyIsDown(_aimKey))
            {
                DoAimbot(ValidTargets);
            }
        }

        private static void DoAimbot(IEnumerable<Player> validTargets)
        {
            foreach (var player in validTargets)
            {
                Vector3 myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
                Vector3 aimView = player.GetBonePos((int)player.BaseAddress, _bones) - Smurf.ControlRecoil.NewViewAngels;
                _newViewAngles = myView.CalcAngle(aimView);
                _newViewAngles.ClampAngle();
            }

            if (_newViewAngles != Vector3.Zero)
            {
                Console.WriteLine(_newViewAngles);
                Smurf.ControlRecoil.SetViewAngles(_newViewAngles);
            }
        }

        private IEnumerable<Player> GetTargets()
        {
            var validTarget = Smurf.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id);
            //Only gets the targets that is seen by me the localplayer.

            if (_aimSpotted)
                validTarget = validTarget.Where(p => p.SeenBy(Smurf.LocalPlayer));
            if (_aimEnemies)
                validTarget = validTarget.Where(p => !p.IsFriendly);
            if (_aimAllies)
                validTarget = validTarget.Where(p => p.IsFriendly);

            return validTarget;
        }

        private void ReadSettings()
        {
            _enabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
            _aimKey = Smurf.Settings.GetKey(Smurf.LocalPlayerWeapon.WeaponName, "Aim Key");
            _fov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim FOV");
            _bones = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Bone");
            _aimSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Smooth");
            _aimSpotted = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Spotted");
            _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Enemies");
            _aimAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Allies");
        }
    }
}
