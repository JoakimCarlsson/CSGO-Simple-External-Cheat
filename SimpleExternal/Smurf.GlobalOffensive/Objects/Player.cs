using System;
using System.Numerics;
using Smurf.GlobalOffensive.SDK;

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

            int wepptr = Core.Memory.Read<int>(baseAdress + Offsets.Player.ActiveWeapon);
            int wepptr1 = wepptr & 0xfff;
            return new Weapon(Core.Memory.Read<IntPtr>(Core.ClientBase + Offsets.Misc.EntityList + (wepptr1 - 1)*0x10));
        }

        public Vector3 GetBonePos(Player player, int bone)
        {
            int matrix = Core.Memory.Read<int>(player.BaseAddress + Offsets.BaseEntity.BoneMatrix);
            Vector3 bonePos = new Vector3
            {
                X = Core.Memory.Read<float>((IntPtr) (matrix + 0x30*bone + 0xC)),
                Y = Core.Memory.Read<float>((IntPtr) (matrix + 0x30*bone + 0x1C)),
                Z = Core.Memory.Read<float>((IntPtr) (matrix + 0x30*bone + 0x2C))
            };
            return bonePos;
        }
    }
}