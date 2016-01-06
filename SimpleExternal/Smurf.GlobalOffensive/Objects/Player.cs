using System;
using System.Numerics;
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

        public Weapon GetCurrentWeapon(IntPtr baseAdress)
        {
            int wepptr = Smurf.Memory.Read<int>(baseAdress + Offsets.Player.ActiveWeapon);
            int wepptr1 = wepptr & 0xfff;
            return new Weapon(Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.EntityList + (wepptr1 - 1)*0x10));
        }

    }
}
