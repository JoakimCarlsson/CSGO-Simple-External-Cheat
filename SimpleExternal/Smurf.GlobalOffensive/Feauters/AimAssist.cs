using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    //Todo
    //1. Get a target, and check the bones that are visible.
    //2. Humanized smooth aim to the target. Faster and faster until get come into a certin distance of the target than we randonize it.
    //3. Berizer curve or something simular to that to make it so the line won't be a perfect line.
    //4. Generate a random point somewhere around the target we'll initial aim at and than aim at target again. 
    //5. Maybe make RCS a bit more randomized.
    public class AimAssist
    {
        #region Fields

        #endregion

        #region Methods

        public void Update()
        {

        }

        //private Vector3 AngelToTarget(Player target)
        //{
        //    Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
        //    Vector3 aimView = target.GetBonePos(target, _targetBone);

        //    Vector3 dst = myView.CalcAngle(aimView);
        //    dst = dst.NormalizeAngle();
        //    return dst;
        //}

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            viewAngles = viewAngles.NormalizeAngle();
            Core.Memory.Write((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        private Player GetTarget()
        {
            IEnumerable<Player> validTargets = Core.Objects.Players.Where(p => p.IsAlive && !p.IsDormant && p.Id != Core.LocalPlayer.Id);

            if (_isVisible)
                validTargets = validTargets.Where(p => p.SeenBy(Core.LocalPlayer));
            if (_targetEnemies)
                validTargets = validTargets.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_targetAllies)
                validTargets = validTargets.Where(p => p.Team == Core.LocalPlayer.Team);

            switch (_aimMode)
            {
                case 1:
                    validTargets = validTargets.OrderBy(p => Vector3.Distance(p.Position, Core.LocalPlayer.Position));
                    break;
                case 2:
                    // we need to use w2s to see whos closest to our xhair.
                    break;
            }

            foreach (Player player in validTargets)
            {
                Vector3 dst = AngelToTarget(player);
                float fov = MathUtils.Fov(_viewAngels, dst, Vector3.Distance(Core.LocalPlayer.Position, player.Position) / 10);
                Console.WriteLine(fov);

                if (fov <= _fov)
                    return player;
            }
            return null;
        }
        #endregion
    }
}
