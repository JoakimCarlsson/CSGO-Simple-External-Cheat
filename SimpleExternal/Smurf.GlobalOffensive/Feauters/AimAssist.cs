using System;
using System.Linq;
using System.Net.Configuration;
using System.Numerics;
using System.Threading;
using System.Windows.Forms.VisualStyles;
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
    public class AimAssist
    {
        #region Fields
        private Player _target;
        private bool _aimSpotted = true;
        private bool _aimEnemies = true;
        private bool _aimAllies = false;
        private bool _humanize = true;
        private int _aimFov = 25;
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
                    HumanAimbot();
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

        private void HumanAimbot()
        {

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
        #endregion
    }
}
