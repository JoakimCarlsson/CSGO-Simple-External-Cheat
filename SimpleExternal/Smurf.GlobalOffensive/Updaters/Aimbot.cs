using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public static Player ActiveTarget;
        private static int _fov;
        private static int _bones = 6;
        private static string _randomBones;
        private static int _aimSmooth;
        private static bool _aimAllies;
        private static bool _enabled;
        private static bool _aimEnemies;
        public IEnumerable<Player> ValidTargets;
        private static Vector3 _viewAngels;
        #endregion

        #region Methods
        public void Update()
        {

            if (!Smurf.Objects.ShouldUpdate())
                return;

            if (Smurf.LocalPlayerWeapon.Clip1 <= 0)
                return;

            _viewAngels = Smurf.Memory.Read<Vector3>((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles));

            ReadSettings();

            if (!_enabled)
                return;

            if (Smurf.KeyUtils.KeyIsDown(_aimKey))
            {
                if (ActiveTarget == null)
                {
                    ActiveTarget = GetTarget();
                    _bones = GetRandomBone();
                }


                if (ActiveTarget != null)
                    DoAimbot();
            }

            if (Smurf.KeyUtils.KeyWentUp(_aimKey))
            {
                ActiveTarget = null;
                Thread.Sleep(1); //If we don't sleep, the key will go up and on and lock onto another target. 
            }
        }

        private int GetRandomBone()
        {
            var tmpString = _randomBones.Split(',');
            int[] tmpBone = Array.ConvertAll(tmpString, int.Parse);
            Random random = new Random();
            int randomIndex = random.Next(0, tmpBone.Length);

            return tmpBone[randomIndex];
        }

        public static float AngleDifference(Vector3 viewAngle, Vector3 dst)
        {
            float num1 = (viewAngle.X - dst.X);
            float num2 = (viewAngle.Y - dst.Y);
            bool flag = 180.0 > num1;
            int num3 = 180.0 > (double)num2 ? 1 : 0;
            if (!flag)
                num1 -= 360f;
            if (num3 == 0)
                num2 -= 360f;
            if (0.0 > num1)
                num1 = num1 - num1 - num1;
            if (0.0 > num2)
                num2 = num2 - num2 - num2;
            return (float)(num1 + (double)num2);
        }

        private void DoAimbot()
        {
            if (!ActiveTarget.IsAlive)
                return;

            var myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
            var aimView = ActiveTarget.GetBonePos((int)ActiveTarget.BaseAddress, _bones);

            var dst = myView.CalcAngle(aimView);

            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            //Aimbot RCS
            if (Smurf.ControlRecoil._rcsEnabled)
            {
                dst = ControlRecoil(dst);
            }
            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            //Smooth
            SmoothAim(_viewAngels, dst);
        }

        private void SmoothAim(Vector3 viewAngles, Vector3 dst)
        {
            var smoothAngle = dst - viewAngles;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            smoothAngle /= _aimSmooth;
            smoothAngle += viewAngles;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            if (smoothAngle != Vector3.Zero)
                Smurf.ControlRecoil.SetViewAngles(smoothAngle);
        }

        private static Vector3 ControlRecoil(Vector3 dst)
        {
            dst.X -= Smurf.LocalPlayer.VecPunch.X * Smurf.ControlRecoil._maxPitch;
            dst.Y -= Smurf.LocalPlayer.VecPunch.Y * Smurf.ControlRecoil._maxYaw;
            //dst.Z -= Smurf.LocalPlayer.VecPunch.Z * 2f;
            return dst;
        }

        private Player GetTarget()
        {
            var validTargets = Smurf.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id && p.SeenBy(Smurf.LocalPlayer));
            if (_aimEnemies)
                validTargets = validTargets.Where(p => p.Team != Smurf.LocalPlayer.Team);
            if (_aimAllies)
                validTargets = validTargets.Where(p => p.Team == Smurf.LocalPlayer.Team);

            validTargets = validTargets.OrderBy(p => (p.Position - Smurf.LocalPlayer.Position).Length());
            foreach (Player validTarget in validTargets)
            {
                var myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
                var aimView = validTarget.GetBonePos((int)validTarget.BaseAddress, _bones);
                var dst = myView.CalcAngle(aimView);

                if (AngleDifference(_viewAngels, dst) <= _fov)
                    return validTarget;
            }
            return null;
        }

        private void ReadSettings()
        {
            try
            {
                _enabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
                _aimKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Key"), 16);
                _fov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Fov");
                _randomBones = Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Bone");
                _aimSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Smooth");
                _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Enemies");
                _aimAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Friendly");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
