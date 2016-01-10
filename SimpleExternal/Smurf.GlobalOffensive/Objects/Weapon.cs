using System;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Objects
{
    public class Weapon : BaseEntity
    {
        public int Clip1 => ReadField<int>(Offsets.Weapon.Clip1);
        public int ZoomLevel => ReadField<int>(Offsets.Weapon.ZoomLevel);
        public string WeaponName => FormateWeaponName(GetClassName());

        public Weapon(IntPtr baseAddress) : base(baseAddress)
        {

        }

        private string FormateWeaponName(string weaponClassId)
        {
            //todo temp fix
            if (weaponClassId == "none" || weaponClassId == "CKnife" || weaponClassId == "BaseEntity" || weaponClassId == "CC4" || weaponClassId == "CHEGrenade" || weaponClassId == "CFlashbang" || weaponClassId == "CSmokeGrenade" || weaponClassId  == "CBreakableProp")
                return "Default";

            //Will remove the C from all the weapons.
            var formatedWeaponName = weaponClassId.Remove(0, 1);

            //Will remove "Weapon" infront of the weapon name.
            if (formatedWeaponName.Contains("Weapon"))
                //Replaces Weapon with nothing.
                formatedWeaponName = formatedWeaponName.Replace("Weapon", "");

            return formatedWeaponName;
        }
    }
}
