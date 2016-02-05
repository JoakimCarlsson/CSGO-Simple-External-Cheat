using System;
using System.Numerics;
using System.Text;
using Smurf.GlobalOffensive.Data.Enums;
using Smurf.GlobalOffensive.Patchables;

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
            set { _classId = value; }
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
        public int GlowIndex => ReadField<int>(Offsets.Misc.GlowIndex);
        public bool IsFriendly => Team == Smurf.LocalPlayer.Team;
        public PlayerTeam Team => (PlayerTeam) ReadField<int>(Offsets.BaseEntity.Team);
        public float Distance => Vector3.Distance(Smurf.LocalPlayer.Position, Position);
        public float DistanceMeters => Vector3.Distance(Smurf.LocalPlayer.Position, Position)*0.01905f;
        public int ShotsFired => ReadField<int>(Offsets.LocalPlayer.ShotsFired);
        private int VirtualTable => ReadField<int>(0x08);
        public bool GunGameImmune => ReadField<bool>(Offsets.Player.GunGameImmune);
        public Vector3 VecPunch => ReadField<Vector3>(Offsets.LocalPlayer.VecPunch);
        public long SpottedByMask => ReadField<long>(Offsets.BaseEntity.SpottedByMask);
        public bool IsWeapon()
        {
            return
                ClassId == (int) ClassIds.Ak47 ||
                ClassId == (int) ClassIds.DEagle ||
                ClassId == (int) ClassIds.Aug ||
                ClassId == (int) ClassIds.Awp ||
                ClassId == (int) ClassIds.Sawedoff ||
                ClassId == (int) ClassIds.G3Sg1 ||
                ClassId == (int) ClassIds.Scar20 ||
                ClassId == (int) ClassIds.DualBerettas ||
                ClassId == (int) ClassIds.Elite ||
                ClassId == (int) ClassIds.FiveSeven ||
                ClassId == (int) ClassIds.Glock ||
                ClassId == (int) ClassIds.Hkp2000 ||
                ClassId == (int) ClassIds.M4A1 ||
                ClassId == (int) ClassIds.Mp7 ||
                ClassId == (int) ClassIds.Mp9 ||
                ClassId == (int) ClassIds.P250 ||
                ClassId == (int) ClassIds.P90 ||
                ClassId == (int) ClassIds.Sg556 ||
                ClassId == (int) ClassIds.Ssg08 ||
                ClassId == (int) ClassIds.Taser ||
                ClassId == (int) ClassIds.Tec9 ||
                ClassId == (int) ClassIds.Ump45 ||
                ClassId == (int) ClassIds.Ssg08 ||
                ClassId == (int) ClassIds.G3Sg1 ||
                ClassId == (int) ClassIds.Scar20 ||
                ClassId == (int) ClassIds.Knife ||
                ClassId == (int) ClassIds.DecoyGrenade ||
                ClassId == (int) ClassIds.HeGrenade ||
                ClassId == (int) ClassIds.IncendiaryGrenade ||
                ClassId == (int) ClassIds.MolotovGrenade ||
                ClassId == (int) ClassIds.SmokeGrenade ||
                ClassId == (int) ClassIds.Flashbang ||
                ClassId == (int) ClassIds.Famas ||
                ClassId == (int) ClassIds.Mac10 ||
                ClassId == (int) ClassIds.Galil ||
                ClassId == (int) ClassIds.M249 ||
                ClassId == (int) ClassIds.M249X ||
                ClassId == (int) ClassIds.Mag7 ||
                ClassId == (int) ClassIds.Nova ||
                ClassId == (int) ClassIds.Negev ||
                ClassId == (int) ClassIds.Ump45X ||
                ClassId == (int) ClassIds.Xm1014 ||
                ClassId == (int) ClassIds.Xm1014X ||
                ClassId == (int) ClassIds.M4 ||
                ClassId == (int) ClassIds.Mag ||
                ClassId == (int) ClassIds.G3Sg1 ||
                ClassId == (int) ClassIds.G3Sg1X ||
                ClassId == (int) ClassIds.Tec9X ||
                ClassId == (int) ClassIds.Bizon ||
                ClassId == (int) ClassIds.P90X ||
                ClassId == (int) ClassIds.Usp ||
                ClassId == (int) ClassIds.Scar20;
        }

        public bool IsPlayer()
        {
            return ClassId == (int) ClassIds.CsPlayer;
        }

        private uint GetClassId()
        {
            try
            {
                var clientClass = GetClientClass();
                return clientClass != 0 ? Smurf.Memory.Read<uint>((IntPtr) ((long) clientClass + 20)) : clientClass;
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
                var function = Smurf.Memory.Read<uint>((IntPtr) (VirtualTable + 2*0x04));
                return function != 0xFFFFFFFF ? Smurf.Memory.Read<uint>((IntPtr) (function + 0x01)) : 0;
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
                    var ptr = Smurf.Memory.Read<int>((IntPtr) (clientClass + 8));
                    var name = Smurf.Memory.ReadString((IntPtr) ptr, Encoding.ASCII, 32);
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