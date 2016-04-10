using System;
using System.Linq;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class AimAssist
    {
        #region Fields
        private Player _target;
        private bool _aimSticky = false;
        private bool _aimSpotted = true;
        private bool _aimEnemies = true;
        private bool _aimAllies = false;
        private bool _humanize = true;
        private int _aimFov = 25;
        private int _aimState = 0;
        private int _aimStyle = 1;
        private int _perferdAimbone = 5;
        private WinAPI.VirtualKeyShort _aimKey = (WinAPI.VirtualKeyShort)0x06;
        public Vector3 ViewAngels;

        #endregion

        #region Methods

        public void Update()
        {
            if (Core.KeyUtils.KeyIsDown(_aimKey))
            {
                if (_humanize)
                {
                    HumanizedAimbot();
                }
                else
                {
                    Aimbot();
                }
            }
        }

        private void Aimbot()
        {
            
        }

        private void HumanizedAimbot()
        {
            switch (_aimState)
            {
                case 1:
                    GetRandomPoint();
                    ReachPoint(); //We reach a random point around the target so it looks we over aim.
                    break;

                case 2:
                    ReachTarget(); //Reach the final destiniton.
                    break;
            }
        }

        private void ReachTarget()
        {
            
        }

        private void ReachPoint()
        {
            
        }

        private void GetRandomPoint()
        {
            
        }

        private void GetTarget(int aimMode)
        {
            var tempTargets = Core.Objects.Players.Where(p => p.Id != Core.LocalPlayer.Id && p.IsAlive && !p.IsDormant);
            if (_aimSpotted)
                tempTargets = tempTargets.Where(p => p.SeenBy(Core.LocalPlayer));
            if (_aimEnemies)
                tempTargets = tempTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_aimAllies)
                tempTargets = tempTargets.Where(p => p.Team == Core.LocalPlayer.Team);

            switch (aimMode)
            {
                case 1:
                    //Closest to LocalPlayer.
                    tempTargets = tempTargets.OrderBy(p => Vector3.DistanceTo(Core.LocalPlayer.Position, p.Position));
                    break;
                case 2:
                    //Closest to crosshair.
                    break;
            }

            foreach (Player player in tempTargets)
            {
                Vector3 dst = AngleToTarget(player, _perferdAimbone);
                var fov = MathUtils.Fov(ViewAngels, dst, Vector3.DistanceTo(Core.LocalPlayer.Position, player.Position) / 10);
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
        #endregion
    }
}
