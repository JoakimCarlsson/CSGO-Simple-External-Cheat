using System;
using System.Numerics;
using Smurf.GlobalOffensive.Data.Enums;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Objects
{
	/// <summary>
	///     Base type for all entities in the game.
	/// </summary>
	public class BaseEntity : NativeObject
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BaseEntity" /> class.
		/// </summary>
		/// <param name="baseAddress">The base address.</param>
		internal BaseEntity(IntPtr baseAddress) : base(baseAddress)
		{
		}

		/// <summary>
		///     Gets the identifier.
		/// </summary>
		/// <value>
		///     The identifier.
		/// </value>
		public int Id => ReadField<int>(Offsets.BaseEntity.Index);
		public Vector3 Position => ReadField<Vector3>(Offsets.BaseEntity.Position);
		public int Health => ReadField<int>(Offsets.BaseEntity.Health);
		public int Armor => ReadField<int>(Offsets.BaseEntity.Armor);
		public int Flags => ReadField<int>(Offsets.Player.Flags);
		public bool IsDormant => ReadField<int>(Offsets.BaseEntity.Dormant) == 1;
		public bool IsAlive => ReadField<byte>(Offsets.Player.LifeState) == 0;
		public bool IsFriendly => Team == Smurf.LocalPlayer.Team;
		public PlayerTeam Team => (PlayerTeam) ReadField<int>(Offsets.BaseEntity.Team);
		public float DistanceSqr => Vector3.DistanceSquared(Smurf.LocalPlayer.Position, Position);
		public float Distance => Vector3.Distance(Smurf.LocalPlayer.Position, Position);

        public int ShotsFired => ReadField<int>(Offsets.LocalPlayer.ShotsFired);
	    public Vector3 VecPunch => ReadField<Vector3>(Offsets.LocalPlayer.VecPunch);
	}
}