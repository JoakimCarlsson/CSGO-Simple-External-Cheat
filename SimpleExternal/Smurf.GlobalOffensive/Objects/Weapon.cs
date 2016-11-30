using System;

namespace Smurf.GlobalOffensive.Objects
{
    public class Weapon : BaseEntity
    {
        public Weapon(IntPtr baseAddress) : base(baseAddress)
        {
        }

        public int Clip1 => ReadField<int>(Offsets.Weapon.Clip1);
        public int ZoomLevel => ReadField<int>(Offsets.Weapon.ZoomLevel);
        public string WeaponName => FormateWeaponName(GetClassName());
        public string WeaponGroup => GetGroup();

        private string GetGroup()
        {
            if (WeaponName == "DEagle" 
                || WeaponName == "Elite" 
                || WeaponName == "FiveSeven" 
                || WeaponName == "Glock" 
                || WeaponName == "P228" 
                || WeaponName == "P250" 
                || WeaponName == "HKP2000" 
                || WeaponName == "Tec9")
            {
                return "Pistol";
            }
            return "Default";
        }

        private string FormateWeaponName(string weaponClassId)
        {
            string formatedWeaponName = weaponClassId.Remove(0, 1);
            if (formatedWeaponName.Contains("Weapon"))
                formatedWeaponName = formatedWeaponName.Replace("Weapon", "");

            return formatedWeaponName;
        }
    }
}