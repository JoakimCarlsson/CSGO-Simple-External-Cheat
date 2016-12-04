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
        private bool _statTrak; //todo implement this.
        private int _statTrakCount; //todo implement this.
        private bool _nameTag;
        private string _nameTagText;

        private int _deagle;
        private int _duals;
        private int _fiveseven;
        private int _glock;
        private int _ak47;
        private int _aug;
        private int _awp;
        private int _famas;
        private int _g3Sg1;
        private int _galilar;
        private int _m249;
        private int _m4A4;
        private int _mac10;
        private int _p90;
        private int _ump45;
        private int _xm1014;
        private int _bizon;
        private int _mag7;
        private int _negev;
        private int _sawedoff;
        private int _tec9;
        private int _p2000;
        private int _mp7;
        private int _mp9;
        private int _nova;
        private int _p250;
        private int _scar20;
        private int _sg556;
        private int _ssg08;
        private int _m4A1;
        private int _usp;
        private int _cz75A;
        private int _revolver;
        private int _knife;

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
            _statTrak = Core.Settings.GetBool("Skin Changer", "Skin Changer StatTrak");
            _statTrakCount = Core.Settings.GetInt("Skin Changer", "Skin Changer StatTrak Count");

            _deagle = Core.Settings.GetInt("Skin Changer", "Deagle");
            _duals = Core.Settings.GetInt("Skin Changer", "Duals");
            _fiveseven = Core.Settings.GetInt("Skin Changer", "Fiveseven");
            _glock = Core.Settings.GetInt("Skin Changer", "Glock");
            _ak47 = Core.Settings.GetInt("Skin Changer", "Ak47");
            _aug = Core.Settings.GetInt("Skin Changer", "Aug");
            _awp = Core.Settings.GetInt("Skin Changer", "Awp");
            _famas = Core.Settings.GetInt("Skin Changer", "Famas");
            _g3Sg1 = Core.Settings.GetInt("Skin Changer", "G3Sg1");
            _galilar = Core.Settings.GetInt("Skin Changer", "Galilar");
            _m249 = Core.Settings.GetInt("Skin Changer", "M249");
            _m4A4 = Core.Settings.GetInt("Skin Changer", "M4A4");
            _mac10 = Core.Settings.GetInt("Skin Changer", "Mac10");
            _p90 = Core.Settings.GetInt("Skin Changer", "P90");
            _ump45 = Core.Settings.GetInt("Skin Changer", "Ump45");
            _xm1014 = Core.Settings.GetInt("Skin Changer", "Xm1014");
            _bizon = Core.Settings.GetInt("Skin Changer", "Bizon");
            _mag7 = Core.Settings.GetInt("Skin Changer", "Mag7");
            _negev = Core.Settings.GetInt("Skin Changer", "Negev");
            _sawedoff = Core.Settings.GetInt("Skin Changer", "Sawedoff");
            _p2000 = Core.Settings.GetInt("Skin Changer", "P2000");
            _tec9 = Core.Settings.GetInt("Skin Changer", "Tec9");
            _mp7 = Core.Settings.GetInt("Skin Changer", "Mp7");
            _mp9 = Core.Settings.GetInt("Skin Changer", "Mp9");
            _nova = Core.Settings.GetInt("Skin Changer", "Nova");
            _p250 = Core.Settings.GetInt("Skin Changer", "P250");
            _scar20 = Core.Settings.GetInt("Skin Changer", "Scar20");
            _sg556 = Core.Settings.GetInt("Skin Changer", "Sg556");
            _ssg08 = Core.Settings.GetInt("Skin Changer", "Ssg08");
            _m4A1 = Core.Settings.GetInt("Skin Changer", "M4A1");
            _usp = Core.Settings.GetInt("Skin Changer", "Usp");
            _cz75A = Core.Settings.GetInt("Skin Changer", "Cz75A");
            _revolver = Core.Settings.GetInt("Skin Changer", "Revolver");
            _knife = Core.Settings.GetInt("Skin Changer", "Knife");
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
