using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.Objects;

namespace Smurf.GlobalOffensive
{
    public class ObjectManager : NativeObject
    {

        private readonly List<Player> _players = new List<Player>();
        //private readonly List<BaseEntity> _entities = new List<BaseEntity>();
        //private readonly List<Weapon> _weapons = new List<Weapon>();

        private readonly int _ticksPerSecond;
        private TimeSpan _lastUpdate = TimeSpan.Zero;

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
            WindowTitle = Utils.MiscUtils.GetActiveWindowTitle();
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
            //_weapons.Clear();
            //_entities.Clear();

            var localPlayerPtr = Core.Memory.Read<IntPtr>(Core.ClientBase + Offsets.Misc.LocalPlayer);

            LocalPlayer = new LocalPlayer(localPlayerPtr);
            LocalPlayerWeapon = LocalPlayer.GetCurrentWeapon(localPlayerPtr);

            var capacity = Core.Memory.Read<int>(Core.ClientBase + Offsets.Misc.EntityList + 0x4);
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
            return Core.Memory.Read<IntPtr>(BaseAddress + index*Offsets.BaseEntity.EntitySize);
        }

        public Player GetPlayerById(int id)
        {
            //   if (_players.Count < id)
            //       return null;

            return Players.FirstOrDefault(p => p.Id == id);
        }

    }
}