using System;
using System.Numerics;
using Smurf.GlobalOffensive.SDK;

namespace Smurf.GlobalOffensive.Objects
{
    public class LocalPlayer : Player
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LocalPlayer" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        internal LocalPlayer(IntPtr baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        ///     Gets the view matrix of the local player.
        /// </summary>
        /// <value>
        ///     The view matrix.
        /// </value>
        //public Matrix4x4 ViewMatrix => ReadField<Matrix4x4>(Offsets.Misc.ViewMatrix);

        /// <summary>
        ///     Gets the player ID for the player currently under the player's crosshair, and 0 if none.
        /// </summary>
        public int CrosshairId => ReadField<int>(Offsets.LocalPlayer.CrosshairId);

        public int Velocity => GetVelocity();

        /// <summary>
        ///     Gets the target the local player is currently aiming at, or null if none.
        /// </summary>
        public Player Target
        {
            get
            {
                var id = CrosshairId;

                return CrosshairId <= 0 ? null : Core.Objects.GetPlayerById(id);
            }
        }

        private int GetVelocity()
        {
            var vector2 = new Vector2(Core.LocalPlayer.VecVelocity.X, Core.LocalPlayer.VecVelocity.Y);
            var length = vector2.Length();
            var velocity = length;

            return (int) velocity;
        }
    }
}