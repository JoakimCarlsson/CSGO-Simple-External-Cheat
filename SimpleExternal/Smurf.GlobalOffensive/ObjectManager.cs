using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Smurf.Common;
using Smurf.GlobalOffensive.Data.Enums;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive
{
    /// <summary>
    ///     Manages entities within the game world.
    /// </summary>
    public class ObjectManager : NativeObject
    {
        private readonly List<BaseEntity> _entities = new List<BaseEntity>();
        // Obtain this dynamically from the game at a later stage.
        //private readonly int _capacity;
        // Exposed through a read-only list, users of the API won't be able to change what's going on in game anyway.
        private readonly List<Player> _players = new List<Player>();

        private readonly int _ticksPerSecond;
        private readonly List<Weapon> _weapons = new List<Weapon>();
        private TimeSpan _lastUpdate = TimeSpan.Zero;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectManager" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="ticksPerSecond">The ticks per second.</param>
        public ObjectManager(IntPtr baseAddress, int ticksPerSecond = 10) : base(baseAddress)
        {
            _ticksPerSecond = ticksPerSecond;
        }

        public IReadOnlyList<Player> Players => _players;
        //public IReadOnlyList<Weapon> Weapons => _weapons;
        //public IReadOnlyList<BaseEntity> Entities => _entities;
        internal LocalPlayer LocalPlayer { get; private set; }
        internal Weapon LocalPlayerWeapon { get; private set; }
        public string WindowTitle { get; set; }

        public void Update()
        {
            WindowTitle = GetActiveWindowTitle();
            if (!IsValid)
                throw new InvalidOperationException(
                    "Can not update the ObjectManager when it's not properly initialized! Are you sure BaseAddress is valid?");

            var timeStamp = MonotonicTimer.GetTimeStamp();
            // Throttle the updates a little - entities won't be changing that frequently.
            // Realistically we don't need to call this very often at all, as we only keep references to the actual
            // entities in the game, and only resolve their members when they're actually required.
            if (timeStamp - _lastUpdate < TimeSpan.FromMilliseconds(1000/_ticksPerSecond))
                return;

            if (!Smurf.Client.InGame)
            {
                // No point in updating if we're not in game - we'd end up reading garbage.
                // Do set the last update time though, we especially don't want to tick too often in menu.
                _lastUpdate = timeStamp;
                return;
            }

            _players.Clear();
            //_weapons.Clear();
            //_entities.Clear();

            var localPlayerPtr = Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.LocalPlayer);

            LocalPlayer = new LocalPlayer(localPlayerPtr);
            LocalPlayerWeapon = LocalPlayer.GetCurrentWeapon(localPlayerPtr);

            var capacity = Smurf.Memory.Read<int>(Smurf.ClientBase + Offsets.Misc.EntityList + 0x4);
            for (var i = 0; i < capacity; i++)
            {
                var entity = new BaseEntity(GetEntityPtr(i));

                if (!entity.IsValid)
                    continue;

                if (entity.IsPlayer())
                    _players.Add(new Player(GetEntityPtr(i)));
                //else if (entity.IsWeapon())
                //    _weapons.Add(new Weapon(GetEntityPtr(i)));
                //else
                //    _entities.Add(new BaseEntity(GetEntityPtr(i)));
            }
            _lastUpdate = timeStamp;
        }

        private IntPtr GetEntityPtr(int index)
        {
            return Smurf.Memory.Read<IntPtr>(BaseAddress + index*Offsets.BaseEntity.EntitySize);
        }

        public BaseEntity GetPlayerById(int id)
        {
            //   if (_players.Count < id)
            //       return null;

            return Players.FirstOrDefault(p => p.Id == id);
        }

        public bool ShouldUpdate(bool checkKnife = true, bool checkGrenades = true, bool checkMisc = true)
        {
            if (WindowTitle != Smurf.GameTitle)
                return false;

            if (Smurf.LocalPlayer == null)
                return false;

            if (Smurf.LocalPlayerWeapon == null)
                return false;

            if (!Smurf.LocalPlayer.IsAlive)
                return false;

            if (Smurf.Client.State != SignonState.Full)
                return false;

            if (checkMisc)
                if (Smurf.LocalPlayerWeapon.ClassName == "none" ||
                    Smurf.LocalPlayerWeapon.ClassName == "BaseEntity" ||
                    Smurf.LocalPlayerWeapon.ClassName == "CC4" ||
                    Smurf.LocalPlayerWeapon.ClassName == "CBreakableProp")
                    return false;

            if (checkGrenades)
                if (Smurf.LocalPlayerWeapon.ClassName == "CDecoyGrenade" ||
                    Smurf.LocalPlayerWeapon.ClassName == "CHEGrenade" ||
                    Smurf.LocalPlayerWeapon.ClassName == "CFlashbang" ||
                    Smurf.LocalPlayerWeapon.ClassName == "CSmokeGrenade")
                    return false;

            if (checkKnife)
                if (Smurf.LocalPlayerWeapon.ClassName == "CKnife")
                    return false;

            return true;
        }
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var builder = new StringBuilder(nChars);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, builder, nChars) > 0)
            {
                return builder.ToString();
            }
            return null;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    }
}