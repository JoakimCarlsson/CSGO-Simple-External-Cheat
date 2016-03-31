using System;
using System.Numerics;

namespace Smurf.GlobalOffensive.Objects
{
    /// <summary>
    ///     Represents the local player - i.e. the guy who's eyes we're borrowing.
    /// </summary>
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
        public Matrix4x4 ViewMatrix => ReadField<Matrix4x4>(Offsets.Misc.ViewMatrix);

        /// <summary>
        ///     Gets the player ID for the player currently under the player's crosshair, and 0 if none.
        /// </summary>
        public int CrosshairId => ReadField<int>(Offsets.LocalPlayer.CrosshairId);

        public int Velocity => GetVelocity();

        /// <summary>
        ///     Gets the target the local player is currently aiming at, or null if none.
        /// </summary>
        public BaseEntity Target
        {
            get
            {
                // Store this in a local variable - the crosshair ID will get updated *very* frequently, 
                // to the point where we can't be sure that by the time we make a call to FirstOrDefault, it'll
                // still be "valid" according to the check before it. (Value can change on a per-frame basis)
                // This way, at least we'll be sure that for the execution of this function, we maintain the same value.
                var id = CrosshairId;

                if (CrosshairId <= 0)
                    return null;

                return Smurf.Objects.GetPlayerById(id);
            }
        }

        private int GetVelocity()
        {
            var vector2 = new Vector2(Smurf.LocalPlayer.VecVelocity.X, Smurf.LocalPlayer.VecVelocity.Y);
            var length = vector2.Length();
            var velocity = length;

            return (int) velocity;
        }
    }
}