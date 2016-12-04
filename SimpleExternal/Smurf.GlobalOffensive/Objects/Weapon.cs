using System;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.SDK;

namespace Smurf.GlobalOffensive.Objects
{
    public class Weapon : BaseEntity
    {
        public Weapon(IntPtr baseAddress) : base(baseAddress)
        {
        }

        public int WeaponId => ReadField<int>(NetVars.Misc.ItemDefinitionIndex);
        public int Clip1 => ReadField<int>(Offsets.Weapon.Clip1);
        public int ZoomLevel => ReadField<int>(Offsets.Weapon.ZoomLevel);
        public int ItemDefinitionIndex => ReadField<int>(NetVars.Misc.ItemDefinitionIndex);
        public string WeaponName => FormateWeaponName(GetClassName());
        public WeaponType WeaponType => GetTypeById();

        public WeaponType GetTypeById()
        {
            switch (WeaponId)
            {
                case 1:
                    //Deagle"
                    return WeaponType.Pistol;
                case 2:
                    //Duals"
                    return WeaponType.Pistol;
                case 3:
                    //FiveSeven"
                    return WeaponType.Pistol;
                case 4:
                    //Glock"
                    return WeaponType.Pistol;
                case 7:
                    //AK47"
                    return WeaponType.Rifle;
                case 8:
                    //AUG"
                    return WeaponType.Rifle;
                case 9:
                    //AWP"
                    return WeaponType.Sniper;
                case 10:
                    //Famas"
                    return WeaponType.Rifle;
                case 11:
                    //G3SG1"
                    return WeaponType.Sniper;
                case 13:
                    //Galil"
                    return WeaponType.Rifle;
                case 14:
                    //M249"
                    return WeaponType.Heavy;
                case 16:
                    //M4A4"
                    return WeaponType.Rifle;
                case 17:
                    //Mac10"
                    return WeaponType.Smg;
                case 19:
                    //P90"
                    return WeaponType.Smg;
                case 24:
                    //UMP45"
                    return WeaponType.Smg;
                case 25:
                    //XM1014"
                    return WeaponType.Shotgun;
                case 26:
                    //Bizon"
                    return WeaponType.Smg;
                case 27:
                    //Mag7"
                    return WeaponType.Shotgun;
                case 28:
                    //Negev"
                    return WeaponType.Heavy;
                case 29:
                    //Sawedoff"
                    return WeaponType.Shotgun;
                case 30:
                    //Tec9"
                    return WeaponType.Pistol;
                case 31:
                    //Taser"
                    return WeaponType.Tazer;
                case 32:
                    //P2000"
                    return WeaponType.Pistol;
                case 33:
                    //MP7"
                    return WeaponType.Smg;
                case 34:
                    //MP9"
                    return WeaponType.Smg;
                case 35:
                    //Nova"
                    return WeaponType.Shotgun;
                case 36:
                    //P250"
                    return WeaponType.Pistol;
                case 38:
                    //Scar20"
                    return WeaponType.Sniper;
                case 39:
                    //SG556"
                    return WeaponType.Rifle;
                case 40:
                    //SSG08"
                    return WeaponType.Sniper;
                case 42:
                    //Knife"
                    return WeaponType.Knife;
                case 43:
                    //Flashbang"
                    return WeaponType.Grenade;
                case 44:
                    //Grenade"
                    return WeaponType.Grenade;
                case 45:
                    //Smoke"
                    return WeaponType.Grenade;
                case 46:
                    //Molotov"
                    return WeaponType.Grenade;
                case 47:
                    //Decoy"
                    return WeaponType.Grenade;
                case 48:
                    //Molotov"
                    return WeaponType.Grenade;
                case 49:
                    //C4"
                    return WeaponType.C4;
                case 59:
                    //Knife"
                    return WeaponType.Knife;
                case 60:
                    //M4A1"
                    return WeaponType.Rifle;
                case 61:
                    //USP"
                    return WeaponType.Pistol;
                case 63:
                    //CZ75"
                    return WeaponType.Pistol;
                case 64:
                    //Revolver"
                    return WeaponType.Pistol;
                case 500:
                    //Bayonet"
                    return WeaponType.Knife;
                case 505:
                    //Flipknife"
                    return WeaponType.Knife;
                case 506:
                    //Gutknife"
                    return WeaponType.Knife;
                case 507:
                    //Karambit"
                    return WeaponType.Knife;
                case 508:
                    //M9 Bayonet"
                    return WeaponType.Knife;
                case 509:
                    //Huntsman"
                    return WeaponType.Knife;
                case 512:
                    //Falchion"
                    return WeaponType.Knife;
                case 515:
                    //Butterfly
                    return WeaponType.Knife;
                case 516:
                    //Knife"
                    return WeaponType.Knife;
                default:
                    //Unknown Weapon"
                    return WeaponType.Unkown;
            }
        }

        private string FormateWeaponName(string weaponClassId)
        {
            string formatedWeaponName = weaponClassId.Remove(0, 1);
            if (formatedWeaponName.Contains("Weapon"))
                formatedWeaponName = formatedWeaponName.Replace("Weapon", "");

            return formatedWeaponName;
        }

        public int GetOwnerXuidLow()
        {
            return Core.Memory.Read<int>(BaseAddress + NetVars.Misc.OriginalOwnerXuidLow);
        }
    }
}