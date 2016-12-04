using System;
using System.Collections.Generic;
using System.Linq;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive.SDK
{
    public class ObjectManager : NativeObject
    {

        private readonly List<Player> _players = new List<Player>();

        private readonly int _ticksPerSecond;
        private TimeSpan _lastUpdate = TimeSpan.Zero;

        public ObjectManager(IntPtr baseAddress, int ticksPerSecond = 10) : base(baseAddress)
        {
            _ticksPerSecond = ticksPerSecond;
        }

        public IReadOnlyList<Player> Players => _players;
        internal LocalPlayer LocalPlayer { get; private set; }
        internal Weapon LocalPlayerWeapon { get; private set; }

        public void Update()
        {
            if (!IsValid)
                throw new InvalidOperationException(
                    "Can not update the ObjectManager when it's not properly initialized! Are you sure BaseAddress is valid?");

            var timeStamp = MonotonicTimer.GetTimeStamp();

            if (timeStamp - _lastUpdate < TimeSpan.FromMilliseconds(1000/_ticksPerSecond))
                return;

            if (!Core.Client.InGame)
            {
                _lastUpdate = timeStamp;
                return;
            }

            _players.Clear();

            IntPtr localPlayerPtr = Core.Memory.Read<IntPtr>(Core.ClientBase + Offsets.LocalPlayer.Base);

            LocalPlayer = new LocalPlayer(localPlayerPtr);
            LocalPlayerWeapon = LocalPlayer.GetCurrentWeapon(localPlayerPtr);

            //int capacity = Core.Memory.Read<int>(Core.ClientBase + Offsets.Misc.EntityList + 0x4);
            for (var i = 0; i < 32; i++) //All we really care about are the players, and they should be in the first 64 entries.
            {
                var entity = new BaseEntity(GetEntityPtr(i));

                if (!entity.IsValid)
                    continue;

                if (entity.IsPlayer())
                    _players.Add(new Player(GetEntityPtr(i)));
            }
            _lastUpdate = timeStamp;
        }

        public IntPtr GetEntityPtr(int index)
        {
            return Core.Memory.Read<IntPtr>(BaseAddress + index*Offsets.BaseEntity.EntitySize);
        }

        public Player GetPlayerById(int id)
        {
            return Players.FirstOrDefault(p => p.Id == id);
        }

    }
}