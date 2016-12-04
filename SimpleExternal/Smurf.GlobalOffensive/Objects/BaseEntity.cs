using System;
using System.Numerics;
using System.Text;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.SDK;

namespace Smurf.GlobalOffensive.Objects
{
    /// <summary>
    ///     Base type for all entities in the game.
    /// </summary>
    public class BaseEntity : NativeObject
    {
        private uint _classId;
        private string _className;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseEntity" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        internal BaseEntity(IntPtr baseAddress) : base(baseAddress)
        {
        }

        private uint ClassId
        {
            get
            {
                if (_classId == 0)
                    _classId = GetClassId();
                return _classId;
            }
        }
        public string ClassName
        {
            get { return _className ?? (_className = GetClassName()); }
            set { _className = value; }
        }
        public int Id => ReadField<int>(Offsets.BaseEntity.Index);
        public Vector3 Position => ReadField<Vector3>(Offsets.BaseEntity.Position);
        public Vector3 VecView => ReadField<Vector3>(Offsets.LocalPlayer.VecViewOffset);
        public int Health => ReadField<int>(Offsets.BaseEntity.Health);
        public int Armor => ReadField<int>(Offsets.BaseEntity.Armor);
        public int Flags => ReadField<int>(Offsets.Player.Flags);
        public bool IsDormant => ReadField<int>(Offsets.BaseEntity.Dormant) == 1;
        public bool IsSpotted => ReadField<int>(Offsets.BaseEntity.Spotted) == 1;
        public bool IsAlive => ReadField<byte>(Offsets.Player.LifeState) == 0;
        public bool IsFriendly => Team == Core.LocalPlayer.Team;
        public int GlowIndex => ReadField<int>(Offsets.Misc.GlowIndex);
        public PlayerTeam Team => (PlayerTeam) ReadField<int>(Offsets.BaseEntity.Team);
        //public float Distance => Vector3.(Core.LocalPlayer.Position, Position);
        public float DistanceMeters => Vector3.Distance(Core.LocalPlayer.Position, Position)*0.01905f;
        public int ShotsFired => ReadField<int>(Offsets.LocalPlayer.ShotsFired);
        private int VirtualTable => ReadField<int>(0x08);
        public bool GunGameImmune => ReadField<bool>(Offsets.Player.GunGameImmune);
        public Vector3 VecPunch => ReadField<Vector3>(Offsets.LocalPlayer.VecPunch);
        public long SpottedByMask => ReadField<long>(Offsets.BaseEntity.SpottedByMask);
        public bool IsWeapon()
        {
            return
                ClassId == (int) ClassIds.AK47 ||
                ClassId == (int) ClassIds.DEagle ||
                ClassId == (int) ClassIds.WeaponAug ||
                ClassId == (int) ClassIds.WeaponAWP ||
                ClassId == (int) ClassIds.WeaponSawedoff ||
                ClassId == (int) ClassIds.WeaponG3SG1 ||
                ClassId == (int) ClassIds.SCAR17 ||
                //ClassId == (int) ClassIds.DualBerettas ||
                ClassId == (int) ClassIds.WeaponElite ||
                ClassId == (int) ClassIds.WeaponFiveSeven ||
                ClassId == (int) ClassIds.WeaponGlock ||
                ClassId == (int) ClassIds.WeaponHKP2000 ||
                ClassId == (int) ClassIds.WeaponM4A1 ||
                ClassId == (int) ClassIds.WeaponMP7 ||
                ClassId == (int) ClassIds.WeaponMP9 ||
                ClassId == (int) ClassIds.WeaponP250 ||
                ClassId == (int) ClassIds.WeaponP90 ||
                ClassId == (int) ClassIds.WeaponSG556 ||
                ClassId == (int) ClassIds.WeaponSSG08 ||
                ClassId == (int) ClassIds.WeaponTaser ||
                ClassId == (int) ClassIds.WeaponTec9 ||
                ClassId == (int) ClassIds.WeaponUMP45 ||
                ClassId == (int) ClassIds.Knife ||
                ClassId == (int) ClassIds.DecoyGrenade ||
                ClassId == (int) ClassIds.HEGrenade ||
                ClassId == (int) ClassIds.IncendiaryGrenade ||
                ClassId == (int) ClassIds.MolotovGrenade||
                ClassId == (int) ClassIds.SmokeGrenade ||
                ClassId == (int) ClassIds.Flashbang ||
                ClassId == (int) ClassIds.WeaponFamas ||
                ClassId == (int) ClassIds.WeaponMAC10 ||
                ClassId == (int) ClassIds.WeaponGalil ||
                ClassId == (int) ClassIds.WeaponM249 ||
                ClassId == (int) ClassIds.WeaponMag7 ||
                ClassId == (int) ClassIds.WeaponNOVA ||
                ClassId == (int) ClassIds.WeaponNegev ||
                ClassId == (int) ClassIds.WeaponUMP45 ||
                ClassId == (int) ClassIds.WeaponXM1014 ||
                ClassId == (int) ClassIds.WeaponM4A1 ||
                ClassId == (int) ClassIds.WeaponBizon ||
                ClassId == (int) ClassIds.WeaponP90 ||
                ClassId == (int) ClassIds.WeaponUSP;
        }

        public bool IsPlayer()
        {
            return ClassId == (int) ClassIds.CSPlayer;
        }

        private uint GetClassId()
        {
            try
            {
                var clientClass = GetClientClass();
                return clientClass != 0 ? Core.Memory.Read<uint>((IntPtr) ((long) clientClass + 20)) : clientClass;
            }
            catch
            {
                return 0;
            }
        }

        private uint GetClientClass()
        {
            try
            {
                if (VirtualTable == 0)
                    return 0;
                var function = Core.Memory.Read<uint>((IntPtr) (VirtualTable + 2*0x04));
                return function != 0xFFFFFFFF ? Core.Memory.Read<uint>((IntPtr) (function + 0x01)) : 0;
            }
            catch
            {
                return 0;
            }
        }

        internal string GetClassName()
        {
            try
            {
                var clientClass = GetClientClass();
                if (clientClass != 0)
                {
                    var ptr = Core.Memory.Read<int>((IntPtr) (clientClass + 8));
                    var name = Core.Memory.ReadString((IntPtr) ptr, Encoding.ASCII, 32);
                    return name;
                }
                return "none";
            }
            catch
            {
                return "none";
            }
        }

        public bool SeenBy(int entityIndex)
        {
            return (SpottedByMask & (0x1 << entityIndex)) != 0;
        }

        public bool SeenBy(BaseEntity ent)
        {
            return SeenBy(ent.Id - 1);
        }
    }
}