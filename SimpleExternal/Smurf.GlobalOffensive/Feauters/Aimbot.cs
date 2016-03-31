using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Aimbot
    {
        #region Fields
        private static Player _activeTarget;
        private static Vector3 _viewAngels;
        private static WinAPI.VirtualKeyShort _aimbotKey;
        private static int _aimbotFov;
        private static int _aimbotBone;
        private static string _randomBones;
        private static int _aimbotSmooth;
        private static bool _aimbotAllies;
        private static bool _aimbotEnabled;
        private static bool _aimbotEnemies;
        private static bool _aimbotZoomed;
        #endregion

        #region Methods
        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            ReadSettings();

            if (!_aimbotEnabled)
                return;

            _viewAngels = Smurf.Memory.Read<Vector3>((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles));

            if (Smurf.KeyUtils.KeyIsDown(_aimbotKey))
            {
                if (_activeTarget == null)
                {
                    _activeTarget = GetTarget();
                    _aimbotBone = GetRandomBone();
                }

                if (_activeTarget != null)
                    DoAimbot();
            }

            if (Smurf.KeyUtils.KeyWentUp(_aimbotKey))
            {
                _activeTarget = null;
                Thread.Sleep(10); //If we don't sleep, the key will go up and on and lock onto another target. 
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



        private static void DoAimbot()
        {
            if (!_activeTarget.IsAlive)
                return;

            if (!_activeTarget.SeenBy(Smurf.LocalPlayer))
                return;

            if (_aimbotZoomed)
            {
                if (Smurf.LocalPlayerWeapon.ZoomLevel <= 0)
                {
                    return;
                }
            }

            var myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
            var aimView = _activeTarget.GetBonePos((int)_activeTarget.BaseAddress, _aimbotBone);

            var dst = myView.CalcAngle(aimView);

            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            //Aimbot RCS
            if (Smurf.ControlRecoil.RcsEnabled)
                dst = ControlRecoil(dst);

            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            //Smooth
            SmoothAim(dst);
        }

        private static void SmoothAim(Vector3 dst)
        {
            var smoothAngle = dst - _viewAngels;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            smoothAngle /= _aimbotSmooth;
            smoothAngle += _viewAngels;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            if (smoothAngle != Vector3.Zero)
                Smurf.ControlRecoil.SetViewAngles(smoothAngle);
        }

        private static Vector3 ControlRecoil(Vector3 dst)
        {
            dst.X -= Smurf.LocalPlayer.VecPunch.X * Smurf.ControlRecoil.MaxPitch;
            dst.Y -= Smurf.LocalPlayer.VecPunch.Y * Smurf.ControlRecoil.MaxYaw;
            return dst;
        }

        private Player GetTarget()
        {
            var validTargets = Smurf.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Smurf.LocalPlayer.Id && p.SeenBy(Smurf.LocalPlayer));
            if (_aimbotEnemies)
                validTargets = validTargets.Where(p => p.Team != Smurf.LocalPlayer.Team);
            if (_aimbotAllies)
                validTargets = validTargets.Where(p => p.Team == Smurf.LocalPlayer.Team);

            validTargets = validTargets.OrderBy(p => (p.Position - Smurf.LocalPlayer.Position).Length());

            foreach (Player validTarget in validTargets)
            {
                var myView = Smurf.LocalPlayer.Position + Smurf.LocalPlayer.VecView;
                var aimView = validTarget.GetBonePos((int)validTarget.BaseAddress, _aimbotBone);
                var dst = myView.CalcAngle(aimView);

                if (MathUtils.Fov(_viewAngels, dst) <= _aimbotFov)
                    return validTarget;
            }
            return null;
        }

        private void ReadSettings()
        {
            try
            {
                _aimbotEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
                _aimbotKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Key"), 16);
                _aimbotFov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Fov");
                _randomBones = Smurf.Settings.GetString(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Bone");
                _aimbotSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Smooth");
                _aimbotEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Enemies");
                _aimbotAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Aim Friendly");
                _aimbotZoomed = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot When Zoomed");
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif

            }
        }

        #endregion
    }
}
