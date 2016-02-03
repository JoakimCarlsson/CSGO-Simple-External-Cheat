using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        #region Fields
        private static WinAPI.VirtualKeyShort _aimKey;
        private static Vector3 _newViewAngles;
        public Player ActiveTarget;
        private static int _fov;
        private static int _bones;
        private static int _aimSmooth;
        private static bool _aimAllies;
        private static bool _enabled;
        private static bool _aimEnemies;
        private static bool _aimJump = true;
        public IEnumerable<Player> ValidTargets;
        #endregion
        #region Methods
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
            {
                ActiveTarget = null;
                Thread.Sleep(1); //If we don't sleep, the key will go up and on and lock onto another target. 
            }
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
            var validTargets = Smurf.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id && p.SeenBy(Smurf.LocalPlayer));
            if (_aimEnemies)
                validTargets = validTargets.Where(p => p.Team != Smurf.LocalPlayer.Team);
            if (_aimAllies)
                validTargets = validTargets.Where(p => p.Team == Smurf.LocalPlayer.Team);

            return validTargets.FirstOrDefault();
        }

        private void ReadSettings()
        {
            try
            {
                _enabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
                _aimKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Key"), 16);
                _fov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Fov");
                _bones = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Bone");
                _aimSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Smooth");
                _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Enemies");
                _aimAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Friendly");
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
