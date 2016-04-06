using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    //Todo
    //1. Get a target, and check the bones that are visible. For this we need BSP parsing. Might add that some day...
    //2. Humanized smooth aim to the target. Faster and faster until get come into a certin distance of the target than we randonize it.
    //3. Berizer curve or something simular to that to make it so the line won't be a perfect line.
    //4. Generate a random point somewhere around the target we'll initial aim at and than aim at target again. 
    //5. Maybe make RCS a bit more randomized.

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
        private Player _target;
        private bool _aimSpotted = true;
        private bool _aimEnemies = true;
        private bool _aimAllies = false;
        private int _aimFov = 6;
        private int _perferdAimbone = 5;
        private WinAPI.VirtualKeyShort _aimKey = (WinAPI.VirtualKeyShort) 0x02;
        private Vector3 _viewAngels;

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            if (Core.KeyUtils.KeyIsDown(_aimKey))
            {
                _viewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));

                if (_target == null)
                    GetTarget();
                else
                    Aim();
            }
            if (Core.KeyUtils.KeyWentUp(_aimKey))
            {
                _target = null;
                Thread.Sleep(20); //If we do not sleep here we'll lock onto another target for a brief moment and it looks weird.
            }

        }

        private void Aim()
        {
            if (!_target.IsAlive)
                return; //If we kill our target we return, else we'll lock onto a random point in the air.

            //todo. get a random point around the targets. 
            //Must be give a new random point if we get a new target. 
            //'Smart' smooth to the target, fast until we come close than slow down / make it random.
            //Make the durve to the target be random, maybe beizer curves.

            //Vector3 target = AngleToTarget(_target, _perferdAimbone);
            //SetViewAngles(target);
            //PrintTargetInfo(); //Useless but good for debugging. (maybe)
        }

        private void PrintTargetInfo()
        {
            Console.WriteLine("Target Id: \t{0}", _target.Id);
            Console.WriteLine("Target Health: \t{0}", _target.Health);
        }

        private void GetTarget()
        {
            var tempTargets = Core.Objects.Players.Where(p => p.Id != Core.LocalPlayer.Id && p.IsAlive && !p.IsDormant);
            if (_aimSpotted)
                tempTargets = tempTargets.Where(p => p.SeenBy(Core.LocalPlayer));
            if (_aimEnemies)
                tempTargets = tempTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_aimAllies)
                tempTargets = tempTargets.Where(p => p.Team == Core.LocalPlayer.Team);

            foreach (Player player in tempTargets)
            {
                Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
                Vector3 aimView = player.GetBonePos(player, _perferdAimbone);
                Vector3 dst = myView.CalcAngle(aimView);

                dst = dst.NormalizeAngle();
                var fov = MathUtils.Fov(_viewAngels, dst, Vector3.Distance(Core.LocalPlayer.Position, player.Position) / 10);
                if (fov <= _aimFov)
                {
                    _target = player;
                    return;
                }
            }
        }

        private Vector3 AngleToTarget(Player target, int boneIndex)
        {
            Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
            Vector3 aimView = target.GetBonePos(target, boneIndex);
            Vector3 dst = myView.CalcAngle(aimView);
            dst = dst.NormalizeAngle();
            return dst;
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            viewAngles = viewAngles.NormalizeAngle();
            Core.Memory.Write((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }
        #endregion
    }
}
