using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{

    //Vector PRotator::Randomize(Vector vAngles)
    //{

    //    if (abs(curX - destX) < .05f)
    //    {
    //        destX = rand() % (int)(gCvars.aim_human_scale * 10) + 1;
    //        destX /= 500;
    //        int positive = rand() % 2 + 1;
    //        if (positive == 2)
    //            destX = -destX;
    //    }

    //    if (abs(curY - destY) < .05f)
    //    {
    //        destY = rand() % (int)(gCvars.aim_human_scale * 10) + 1;
    //        destY /= 500;
    //        int positive = rand() % 2 + 1;
    //        if (positive == 2)
    //            destY = -destY;
    //    }

    //    int speed = 20 - int(gCvars.aim_human_speed);
    //    curX += (destX - curX) / ((15 * speed) + 10);

    //    curY += (destY - curY) / ((15 * speed) + 10);

    //    vAngles.x += curX;
    //    vAngles.y += curY;

    //    lastX = curX;
    //    lastY = curY;

    //    return vAngles;

    //}

    public class AimAssist
    {
        #region Fields

        private WinAPI.VirtualKeyShort _aimKey = (WinAPI.VirtualKeyShort)0x05;
        private Player _aimTarget;
        private bool _aimAssistEnabled = true;
        private bool _aimHumanized = true;
        private bool _aimSpotted = true;
        private bool _aimEnemies = true;
        private bool _aimAllies = false;
        private int _aimFov = 50;
        private int _aimBone = 5;
        private int _aimState = 0;
        private int _aimSpeed = 50;
        private readonly List<Player> _players = new List<Player>();

        #endregion

        #region Properties
        public Vector3 ViewAngels { get; set; }
        #endregion

        #region Methos

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

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
                Thread.Sleep(20); //If we don't do this sleep we might end up getting another target and for a brief ms it will lock onto another target. Looks werid.
            }
        }

        private void GetPlayers()
        {
            for (var i = 0; i < 64; i++) //All we really care about are the players, and they should be in the first 64 entries.
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
            SetViewAngles(destination);
        }

        public void SetViewAngles(Vector3 viewAngles)
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
