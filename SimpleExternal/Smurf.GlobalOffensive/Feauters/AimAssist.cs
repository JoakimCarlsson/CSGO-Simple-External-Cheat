using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class AimAssist
    {
        #region Fields

        private bool _aimAssistEnabled;
        private WinAPI.VirtualKeyShort _aimKey;
        private bool _aimHumanized;
        private bool _aimSpotted;
        private bool _aimEnemies;
        private bool _aimAllies;
        private int _aimFov;
        private int _aimBone;
        private int _aimSpeed;

        private Player _aimTarget;
        private readonly List<Player> _players = new List<Player>();

        #endregion

        #region Properties
        private Vector3 ViewAngels { get; set; }
        #endregion

        #region Methos

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_aimAssistEnabled)
                return;

            GetPlayers();

            if (Core.KeyUtils.KeyIsDown(_aimKey))
            {
                ViewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));

                if (_aimTarget == null)
                {
                    _aimTarget = GetTarget();
                }
                else
                {
                    Aim();
                }
            }
            if (Core.KeyUtils.KeyWentUp(_aimKey))
            {
                _aimTarget = null;
                Thread.Sleep(20);
            }
        }

        private void ReadSettings()
        {
            _aimAssistEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Enabled");
            _aimKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Aim Key"), 16);
            _aimFov = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Fov");
            _aimHumanized = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Humanized");
            _aimSpotted = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Spotted");
            _aimEnemies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Enemies");
            _aimAllies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Allies");
            _aimSpeed = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Speed");
            _aimBone = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Bone");
        }

        private void GetPlayers()
        {
            for (var i = 0; i < 64; i++)
            {
                var entity = new BaseEntity(Core.Objects.GetEntityPtr(i));

                if (!entity.IsValid)
                    continue;

                if (entity.IsPlayer())
                    _players.Add(new Player(Core.Objects.GetEntityPtr(i)));
            }
        }

        private void Aim()
        {
            Vector3 destination = AngleToTarget(_aimTarget, _aimBone);


            if (_aimHumanized)
            {
                float yScale = 5.7f;
                float xScale = 1.5f;

                Vector3 vecCurrentViewAngles = ViewAngels;
                Vector3 vecViewAngleDelta = destination - vecCurrentViewAngles;

                vecViewAngleDelta += new Vector3(vecViewAngleDelta.Y / yScale, vecViewAngleDelta.X / xScale, 0.0f);
                vecViewAngleDelta /= _aimSpeed;
                Vector3 vecViewAngles = vecCurrentViewAngles + vecViewAngleDelta;

                SetViewAngles(vecViewAngles);
            }
            else
            {
                SetViewAngles(destination);
            }
        }

        private void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            viewAngles = viewAngles.NormalizeAngle();
            Core.Memory.Write((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        private Player GetTarget()
        {
            var tempTargets = _players.Where(p => p.Id != Core.LocalPlayer.Id && p.IsAlive && !p.IsDormant);
            if (_aimSpotted)
                tempTargets = tempTargets.Where(p => p.SeenBy(Core.LocalPlayer));
            if (_aimEnemies)
                tempTargets = tempTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_aimAllies)
                tempTargets = tempTargets.Where(p => p.Team == Core.LocalPlayer.Team);

            foreach (Player player in tempTargets)
            {
                Vector3 dst = AngleToTarget(player, _aimBone);
                var fov = MathUtils.Fov(ViewAngels, dst, Vector3.Distance(Core.LocalPlayer.Position, player.Position) / 10);
                Console.WriteLine(fov);
                if (fov <= _aimFov)
                {
                    return player;
                }
            }
            return null;
        }

        private Vector3 AngleToTarget(Player target, int boneIndex)
        {
            Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
            Vector3 aimView = target.GetBonePos(target, boneIndex);
            Vector3 dst = myView.CalcAngle(aimView);
            dst = dst.NormalizeAngle();
            return dst;
        }
        #endregion
    }
}
