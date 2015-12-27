using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Objects
{
    public class Player : BaseEntity
    {
        public Player(IntPtr baseAddress) : base(baseAddress)
        {           

        }
        public Vector3 VecVelocity => ReadField<Vector3>(Offsets.Player.VecVelocity);

        public int GetVelocity()
        {
            var vector2 = new Vector2(Smurf.LocalPlayer.VecVelocity.X, Smurf.LocalPlayer.VecVelocity.Y);
            var length = vector2.Length();
            var velocity = length;

            return (int)velocity;
        }
    }
}
