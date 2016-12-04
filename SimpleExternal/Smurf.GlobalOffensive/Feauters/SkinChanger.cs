using System;
using Smurf.GlobalOffensive.Enums;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class SkinChanger
    {
        #region Fields
        private WinAPI.VirtualKeyShort _forceUpdateKey;
        private bool _skinChanger;
        private bool _knifeChanger; //todo implement this.
        private bool _statTrak = false; //todo implement this.
        private int _statTrakCount = 0; //todo implement this.
        private bool _nameTag;
        private string _nameTagText;

        private readonly int _deagle = 328;
        private readonly int _duals = 28;
        private readonly int _fiveseven = 427;
        private readonly int _glock = 353;
        private readonly int _ak47 = 180;
        private readonly int _aug = 39;
        private readonly int _awp = 344;
        private readonly int _famas = 492;
        private readonly int _g3Sg1 = 39;
        private readonly int _galilar = 39;
        private readonly int _m249 = 39;
        private readonly int _m4A4 = 309;
        private readonly int _mac10 = 433;
        private readonly int _p90 = 39;
        private readonly int _ump45 = 39;
        private readonly int _xm1014 = 39;
        private readonly int _bizon = 39;
        private readonly int _mag7 = 39;
        private readonly int _negev = 39;
        private readonly int _sawedoff = 39;
        private readonly int _tec9 = 39;
        private readonly int _p2000 = 39;
        private readonly int _mp7 = 39;
        private readonly int _mp9 = 39;
        private readonly int _nova = 39;
        private readonly int _p250 = 39;
        private readonly int _scar20 = 39;
        private readonly int _sg556 = 39;
        private readonly int _ssg08 = 39;
        private readonly int _m4A1 = 326;
        private readonly int _usp = 313;
        private readonly int _cz75A = 270;
        private readonly int _revolver = 39;
        private readonly int _knife = (int)WeaponSkin.Tiger_Tooth;

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_skinChanger)
                return;

            if (Core.KeyUtils.KeyIsDown(_forceUpdateKey))
                Engine.ForceUpdate();

            ChangeSkins();
        }

        private void ChangeSkins()
        {
            for (int i = 1; i <= 9; i++)
            {
                int curWeaponIndex = Core.Memory.Read<int>(Core.Objects.LocalPlayer.BaseAddress + NetVars.Misc.MyWeapons + ((i - 1) * 0x4)) & 0xfff;
                int curWeaponEnt = Core.Memory.Read<int>(Core.ClientBase + Offsets.Misc.EntityList + (curWeaponIndex - 1) * 0x10);

                Weapon weapon = new Weapon((IntPtr)curWeaponEnt);
                int weaponSkinId = GetWeaponSkin(weapon.ItemDefinitionIndex);

                if (weapon.BaseAddress == IntPtr.Zero)
                    continue;

                int xuId = Core.Memory.Read<int>(weapon.BaseAddress + NetVars.Misc.OriginalOwnerXuidLow);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.ItemIdHigh, -1);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.OriginalOwnerXuidLow, 0);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.OriginalOwnerXuidHigh, 0);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.FallbackPaintKit, weaponSkinId);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.FallbackSeed, 0);

                if (_statTrak)
                    Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.FallbackStatTrak, _statTrakCount);

                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.FallbackWear, 0.001f);
                Core.Memory.Write(weapon.BaseAddress + NetVars.Misc.AccountId, xuId);
            }
        }

        private void ReadSettings()
        {
            _forceUpdateKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString("Skin Changer", "Force Update Key"), 16);
            _skinChanger = Core.Settings.GetBool("Skin Changer", "Skin Changer");
        }

        private int GetWeaponSkin(int weaponId)
        {
            switch (weaponId)
            {
                case (int)ItemDefinitionIndex.AK47:
                    return _ak47;
                case (int)ItemDefinitionIndex.AUG:
                    return _aug;
                case (int)ItemDefinitionIndex.AWP:
                    return _awp;
                case (int)ItemDefinitionIndex.BIZON:
                    return _bizon;
                case (int)ItemDefinitionIndex.CZ75A:
                    return _cz75A;
                case (int)ItemDefinitionIndex.DEAGLE:
                    return _deagle;
                case (int)ItemDefinitionIndex.ELITE:
                    return _duals;
                case (int)ItemDefinitionIndex.FAMAS:
                    return _famas;
                case (int)ItemDefinitionIndex.FIVESEVEN:
                    return _fiveseven;
                case (int)ItemDefinitionIndex.G3SG1:
                    return _g3Sg1;
                case (int)ItemDefinitionIndex.GALILAR:
                    return _galilar;
                case (int)ItemDefinitionIndex.GLOCK:
                    return _glock;
                case (int)ItemDefinitionIndex.M249:
                    return _m249;
                case (int)ItemDefinitionIndex.M4A1:
                    return _m4A1;
                case (int)ItemDefinitionIndex.M4A4:
                    return _m4A4;
                case (int)ItemDefinitionIndex.MAC10:
                    return _mac10;
                case (int)ItemDefinitionIndex.MAG7:
                    return _mag7;
                case (int)ItemDefinitionIndex.MP7:
                    return _mp7;
                case (int)ItemDefinitionIndex.MP9:
                    return _mp9;
                case (int)ItemDefinitionIndex.NEGEV:
                    return _negev;
                case (int)ItemDefinitionIndex.NOVA:
                    return _nova;
                case (int)ItemDefinitionIndex.P2000:
                    return _p2000;
                case (int)ItemDefinitionIndex.P250:
                    return _p250;
                case (int)ItemDefinitionIndex.P90:
                    return _p90;
                case (int)ItemDefinitionIndex.REVOLVER:
                    return _revolver;
                case (int)ItemDefinitionIndex.SAWEDOFF:
                    return _sawedoff;
                case (int)ItemDefinitionIndex.SCAR20:
                    return _scar20;
                case (int)ItemDefinitionIndex.SG556:
                    return _sg556;
                case (int)ItemDefinitionIndex.SSG08:
                    return _ssg08;
                case (int)ItemDefinitionIndex.TEC9:
                    return _tec9;
                case (int)ItemDefinitionIndex.UMP45:
                    return _ump45;
                case (int)ItemDefinitionIndex.USP:
                    return _usp;
                case (int)ItemDefinitionIndex.XM1014:
                    return _xm1014;
                case (int)ItemDefinitionIndex.KNIFE_BAYONET:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_BUTTERFLY:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_FALCHION:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_FLIP:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_GUT:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_KARAMBIT:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_M9_BAYONET:
                    return _knife;
                case (int)ItemDefinitionIndex.KNIFE_TACTICAL:
                    return _knife;
                default:
                    return -1;
            }
        }

        #endregion
    }
}
