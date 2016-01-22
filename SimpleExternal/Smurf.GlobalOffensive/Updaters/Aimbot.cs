using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        private static WinAPI.VirtualKeyShort _aimKey;
        private static Vector3 _newViewAngles;
        public Player ActiveTarget;
        private static int _fov;
        private static int _bones;
        private static int _aimSmooth;
        private static bool _aimSpotted;
        private static bool _aimAllies;
        private static bool _enabled;
        private static bool _aimEnemies;
        private static bool _aimJump = true;


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

            if (Smurf.KeyUtils.KeyIsDown(_aimKey))
            {
                if (ActiveTarget == null)
                    ActiveTarget = GetTarget();

                if (ActiveTarget != null)
                    DoAimbot();
            }

            if (Smurf.KeyUtils.KeyWentUp(_aimKey))
                ActiveTarget = null;
        }

        private void DoAimbot()
        {
            if (!ActiveTarget.IsAlive)
                return;

            var myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
            var aimView = ActiveTarget.GetBonePos((int)ActiveTarget.BaseAddress, _bones) - Smurf.ControlRecoil.NewViewAngels;
            Smurf.ControlRecoil.NewViewAngels = myView.CalcAngle(aimView);
            Smurf.ControlRecoil.NewViewAngels.ClampAngle();

            if (Smurf.ControlRecoil.NewViewAngels != Vector3.Zero)
            {
                Smurf.ControlRecoil.SetViewAngles(Smurf.ControlRecoil.NewViewAngels);
                Smurf.ControlRecoil.NewViewAngels = Vector3.Zero;
            }

        }

        private Player GetTarget()
        {
            var validTargets = Smurf.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id);
            if (_aimSpotted)
                validTargets = validTargets.Where(p => p.SeenBy(Smurf.LocalPlayer));
            if (_aimEnemies)
                validTargets = validTargets.Where(p => p.Team != Smurf.LocalPlayer.Team);
            if (_aimAllies)
                validTargets = validTargets.Where(p => p.Team == Smurf.LocalPlayer.Team);

            return validTargets.FirstOrDefault();
        }

        private void ReadSettings()
        {
            _enabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
            _aimKey = Smurf.Settings.GetKey(Smurf.LocalPlayerWeapon.WeaponName, "Aim Key");
            _fov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Fov");
            _bones = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Bone");
            _aimSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Smooth");
            _aimSpotted = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Spotted");
            _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Enemies");
            _aimAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Allies");
        }
    }
}
