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
        private int _aimFov = 25;
        private int _perferdAimbone = 5;
        private int _aimSmooth = 100;
        public int AimState = 0;
        public Vector3 AimAt;
        private WinAPI.VirtualKeyShort _aimKey = (WinAPI.VirtualKeyShort)0x06;
        public Vector3 ViewAngels;
        private bool _gotRandomPoint;
        private float _tempX;
        private float _tempY;
        private float _tempZ;

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            if (Core.KeyUtils.KeyIsDown(_aimKey))
            {
                ViewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));

                if (_target == null)
                    GetTarget();
                else
                    Aim();
            }
            if (Core.KeyUtils.KeyWentUp(_aimKey))
            {
                _target = null;
                AimState = 0;
                _gotRandomPoint = false;
                Thread.Sleep(20); //If we do not sleep here we'll lock onto another target for a brief moment and it looks weird.
            }

        }

        private void Aim()
        {
            if (!_target.IsAlive)
                return; //If we kill our target we return, else we'll lock onto a random point in the air.

            switch (AimState)
            {
                case 0:
                    AimAt = GetAimPoint(true);
                    if (AimAt.ToString("0.0") == ViewAngels.ToString("0.0"))
                        AimState = 1; //We did reach the random point.
                    break;

                case 1: //Go from the random point to the real point.
                    AimAt = GetAimPoint();
                    break;
            }
            Vector3 smoothAngle = SmoothAngels(_aimSmooth);

            if (smoothAngle != Vector3.Zero)
                SetViewAngles(smoothAngle);
        }

        private Vector3 SmoothAngels(int smoothAmount)
        {
            var smoothAngle = AimAt - ViewAngels;
            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();
            smoothAngle /= smoothAmount;
            smoothAngle += ViewAngels;
            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();
            return smoothAngle;
        }

        private Vector3 GetAimPoint(bool randomPoint = false)
        {
            Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
            Vector3 aimView = _target.GetBonePos(_target, _perferdAimbone);

            if (!_gotRandomPoint)
            {
                if (randomPoint)
                {
                    _tempX = new Random().Next(-20, 20);
                    _tempY = new Random().Next(-20, 20);
                    _tempZ = new Random().Next(-20, 20);
                }
                _gotRandomPoint = true;
            }

            if (AimState == 0)
            {
                aimView.X += _tempX;
                aimView.Y += _tempY;
                aimView.Z += _tempZ;
            }

            Vector3 dst = myView.CalcAngle(aimView);
            dst = dst.NormalizeAngle();
            return dst;
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
                Vector3 dst = AngleToTarget(player, _perferdAimbone);
                var fov = MathUtils.Fov(ViewAngels, dst, Vector3.Distance(Core.LocalPlayer.Position, player.Position) / 10);
                Console.WriteLine(fov);
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
