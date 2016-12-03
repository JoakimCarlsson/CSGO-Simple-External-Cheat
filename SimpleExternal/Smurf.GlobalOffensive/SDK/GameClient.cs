using System;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive.SDK
{
    /// <summary>
    ///     Manages the game client, and all stuff we require from it.
    /// </summary>
    public class GameClient : NativeObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GameClient" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        public GameClient(IntPtr baseAddress) : base(baseAddress)
        {
            Console.WriteLine("GameClient initialized.");
        }

        /// <summary>
        ///     Gets the index of the local player.
        /// </summary>
        /// <value>
        ///     The index of the local player.
        /// </value>
        public int LocalPlayerIndex => ReadField<int>(Offsets.ClientState.LocalPlayerIndex);

        /// <summary>
        ///     Gets the state of the game client.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public SignonState State => (SignonState) ReadField<int>(Offsets.ClientState.GameState);

        /// <summary>
        ///     Gets a value indicating whether [in game].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [in game]; otherwise, <c>false</c>.
        /// </value>
        public bool InGame => State == SignonState.Full;

        /// <summary>
        ///     Gets a value indicating whether [in menu].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [in menu]; otherwise, <c>false</c>.
        /// </value>
        public bool InMenu => State == SignonState.None;
    }
}