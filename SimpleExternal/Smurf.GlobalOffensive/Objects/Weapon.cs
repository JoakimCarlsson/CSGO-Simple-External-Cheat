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
        public string WeaponName => FormateWeaponName(GetClassName());
        public string WeaponGroup => GetGroup();

        //TODO Fix this so we get all the groups, pretty useless though.
        private string GetGroup()
        {
            if (WeaponName == "DEagle" || WeaponName == "Elite" || WeaponName == "FiveSeven" || WeaponName == "Glock" || WeaponName == "P228" || WeaponName == "P250" || WeaponName == "HKP2000" || WeaponName == "Tec9")
            {
                return "Pistol";
            }
            return "Default";
        }

        private string FormateWeaponName(string weaponClassId)
        {
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