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
            get => _className ?? (_className = GetClassName());
            set => _className = value;
        }
        public int Id => ReadField<int>(Offsets.BaseEntity.Index);
        public Vector3 Position => ReadField<Vector3>(Offsets.BaseEntity.VecOrigin);
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
        //public float Distance => Vector3.(Core.LocalPlayer.VecOrigin, VecOrigin);
        public float DistanceMeters => Vector3.Distance(Core.LocalPlayer.Position, Position)*0.01905f;
        public int ShotsFired => ReadField<int>(Offsets.LocalPlayer.ShotsFired);
        private int VirtualTable => ReadField<int>(0x08);
        public bool GunGameImmune => ReadField<bool>(Offsets.Player.GunGameImmune);
        public Vector3 VecPunch => ReadField<Vector3>(Offsets.LocalPlayer.VecPunch);
        public long SpottedByMask => ReadField<long>(Offsets.BaseEntity.SpottedByMask);
        public bool IsWeapon()
        {
            return
                ClassId == (int)WeaponIds.Ak47 ||
                ClassId == (int)WeaponIds.DEagle ||
                ClassId == (int)WeaponIds.Aug ||
                ClassId == (int)WeaponIds.Awp ||
                ClassId == (int)WeaponIds.Sawedoff ||
                ClassId == (int)WeaponIds.G3Sg1 ||
                ClassId == (int)WeaponIds.Scar20 ||
                ClassId == (int)WeaponIds.Elite ||
                ClassId == (int)WeaponIds.Fiveseven ||
                ClassId == (int)WeaponIds.Glock ||
                ClassId == (int)WeaponIds.Hkp2000 ||
                ClassId == (int)WeaponIds.M4A4 ||
                ClassId == (int)WeaponIds.Mp7 ||
                ClassId == (int)WeaponIds.Mp9 ||
                ClassId == (int)WeaponIds.P250 ||
                ClassId == (int)WeaponIds.P90 ||
                ClassId == (int)WeaponIds.Sg556 ||
                ClassId == (int)WeaponIds.Ssg08 ||
                ClassId == (int)WeaponIds.Taser ||
                ClassId == (int)WeaponIds.Tec9 ||
                ClassId == (int)WeaponIds.Ump45 ||
                ClassId == (int)WeaponIds.Knife ||
                ClassId == (int)WeaponIds.DecoyGrenade ||
                ClassId == (int)WeaponIds.HeGrenade ||
                ClassId == (int)WeaponIds.IncendiaryGrenade ||
                ClassId == (int)WeaponIds.MolotovGrenade ||
                ClassId == (int)WeaponIds.SmokeGrenade ||
                ClassId == (int)WeaponIds.Flashbang ||
                ClassId == (int)WeaponIds.Famas ||
                ClassId == (int)WeaponIds.Mac10 ||
                ClassId == (int)WeaponIds.Galil ||
                ClassId == (int)WeaponIds.M249 ||
                ClassId == (int)WeaponIds.Mag7 ||
                ClassId == (int)WeaponIds.Nova ||
                ClassId == (int)WeaponIds.Negev ||
                ClassId == (int)WeaponIds.Ump45 ||
                ClassId == (int)WeaponIds.Xm1014 ||
                ClassId == (int)WeaponIds.M4A1Silencer ||
                ClassId == (int)WeaponIds.Bizon ||
                ClassId == (int)WeaponIds.P90 ||
                ClassId == (int)WeaponIds.UspSilencer;
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