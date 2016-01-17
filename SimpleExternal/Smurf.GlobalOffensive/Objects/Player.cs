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
        //TODO This will return the velocity of the localplayer.
        public int Velocity => GetVelocity();
        public bool InAir => IsInAir();
        private bool IsInAir()
        {
            return Flags == 256;
        }

        private int GetVelocity()
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
            return new Weapon(Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.EntityList + (wepptr1 - 1) * 0x10));
        }
        public Vector3 GetBonePos(int baseAdress, int bone)
        {
            int bMatrix = Smurf.Memory.Read<int>((IntPtr)(baseAdress + Offsets.BaseEntity.BoneMatrix));
            Vector3 vec = new Vector3
            {
                X = Smurf.Memory.Read<float>((IntPtr)(bMatrix + (0x30 * bone) + 0xC)),
                Y = Smurf.Memory.Read<float>((IntPtr)(bMatrix + (0x30 * bone) + 0x1C)),
                Z = Smurf.Memory.Read<float>((IntPtr)(bMatrix + (0x30 * bone) + 0x2C))
            };
            return vec;
        }

    }
}
