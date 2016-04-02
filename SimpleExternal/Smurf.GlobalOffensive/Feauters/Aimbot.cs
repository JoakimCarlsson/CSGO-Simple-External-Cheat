using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

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
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_aimbotEnabled)
                return;

            _viewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));

            if (Core.KeyUtils.KeyIsDown(_aimbotKey))
            {
                if (_activeTarget == null)
                {
                    _activeTarget = GetTarget();
                    _aimbotBone = GetRandomBone();
                }

                if (_activeTarget != null)
                    DoAimbot();
            }

            if (Core.KeyUtils.KeyWentUp(_aimbotKey))
            {
                _activeTarget = null;
                Thread.Sleep(10);
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

            if (!_activeTarget.SeenBy(Core.LocalPlayer))
                return;

            if (Core.LocalPlayerWeapon.Clip1 == 0)
                return;

            if (_aimbotZoomed)
                if (Core.LocalPlayerWeapon.ZoomLevel <= 0)
                    return;

            var myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
            var aimView = _activeTarget.GetBonePos(_activeTarget, _aimbotBone);
            var dst = myView.CalcAngle(aimView);

            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            if (Core.ControlRecoil.RcsEnabled)
                dst = ControlRecoil(dst);

            dst = dst.NormalizeAngle();
            dst = dst.ClampAngle();

            if (_aimbotSmooth > 0)
                 SmoothAim(dst);
            else
                Core.ControlRecoil.SetViewAngles(dst);
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
                Core.ControlRecoil.SetViewAngles(smoothAngle);
        }

        private static Vector3 ControlRecoil(Vector3 dst)
        {
            dst.X -= Core.LocalPlayer.VecPunch.X * Core.ControlRecoil.MaxPitch;
            dst.Y -= Core.LocalPlayer.VecPunch.Y * Core.ControlRecoil.MaxYaw;
            return dst;
        }

        private Player GetTarget()
        {
            var validTargets = Core.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Core.LocalPlayer.Id && p.SeenBy(Core.LocalPlayer));
            if (_aimbotEnemies)
                validTargets = validTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_aimbotAllies)
                validTargets = validTargets.Where(p => p.Team == Core.LocalPlayer.Team);

            validTargets = validTargets.OrderBy(p => (p.Position - Core.LocalPlayer.Position).Length());

            foreach (Player validTarget in validTargets)
            {
                Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
                Vector3 aimView = validTarget.GetBonePos(validTarget, _aimbotBone);
                Vector3 dst = myView.CalcAngle(aimView);
                dst = dst.NormalizeAngle();

                float fov = MathUtils.Fov(_viewAngels, dst);
                Console.WriteLine(fov);

                if (fov <= _aimbotFov)
                    return validTarget;
            }
            return null;
        }

        private void ReadSettings()
        {
            try
            {
                _aimbotEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
                _aimbotKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Aimbot Key"), 16);
                _aimbotFov = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aimbot Fov");
                _randomBones = Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Aimbot Bone");
                _aimbotSmooth = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aimbot Smooth");
                _aimbotEnemies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aimbot Aim Enemies");
                _aimbotAllies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aimbot Aim Friendly");
                _aimbotZoomed = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aimbot When Zoomed");
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
