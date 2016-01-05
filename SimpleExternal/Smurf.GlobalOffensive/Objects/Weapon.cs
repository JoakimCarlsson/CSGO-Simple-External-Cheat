using System;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Objects
{
    public class Weapon : BaseEntity
    {
        public Weapon(IntPtr baseAddress) : base(baseAddress)
        {

        }
        public int Clip1 => ReadField<int>(Offsets.Weapon.Clip1);
        public int ZoomLevel => ReadField<int>(Offsets.Weapon.ZoomLevel);

    }
}
