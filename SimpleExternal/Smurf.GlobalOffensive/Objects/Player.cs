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
        public float FlashMaxAlpha => ReadField<float>(Offsets.Player.FlashMaxAlpha);
        public bool InAir => IsInAir();

        private bool IsInAir()
        {
            return Flags == 256 || Flags == 262;
        }

        public Weapon GetCurrentWeapon(IntPtr baseAdress)
        {
            if (!IsValid)
                return null;

            var wepptr = Smurf.Memory.Read<int>(baseAdress + Offsets.Player.ActiveWeapon);
            var wepptr1 = wepptr & 0xfff;
            return new Weapon(Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.EntityList + (wepptr1 - 1)*0x10));
        }

        public Vector3 GetBonePos(int baseAdress, int bone)
        {
            var bMatrix = Smurf.Memory.Read<int>((IntPtr) (baseAdress + Offsets.BaseEntity.BoneMatrix));
            var bonePos = new Vector3
            {
                X = Smurf.Memory.Read<float>((IntPtr) (bMatrix + 0x30*bone + 0xC)),
                Y = Smurf.Memory.Read<float>((IntPtr) (bMatrix + 0x30*bone + 0x1C)),
                Z = Smurf.Memory.Read<float>((IntPtr) (bMatrix + 0x30*bone + 0x2C))
            };
            return bonePos;
        }
    }
}