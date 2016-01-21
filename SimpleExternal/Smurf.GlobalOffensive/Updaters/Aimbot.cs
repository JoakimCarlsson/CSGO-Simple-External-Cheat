using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Aimbot
    {
        private static bool _enabled;
        private static WinAPI.VirtualKeyShort _aimKey;
        private static int _fov;
        private static int _bones;
        private static int _aimSmooth;
        private static bool _aimSpotted;
        private static bool _aimAllies;
        private static bool _aimEnemies;
        private static Vector3 _newViewAngles;
        private static Vector3 _viewAngels;
        private static Player activeTarget;

        public IEnumerable<Player> ValidTargets;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            if (Smurf.LocalPlayerWeapon.Clip1 <= 0)
                return;

            ReadSettings();

            if (!_enabled)
                return;

            if (Smurf.KeyUtils.KeyIsDown(_aimKey))
            {

            }
        }

        private void ReadSettings()
        {
            _enabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aimbot Enabled");
            _aimKey = Smurf.Settings.GetKey(Smurf.LocalPlayerWeapon.WeaponName, "Aim Key");
            _fov = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim FOV");
            _bones = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Bone");
            _aimSmooth = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Aim Smooth");
            _aimSpotted = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Spotted");
            _aimEnemies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Enemies");
            _aimAllies = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Aim Allies");
        }
    }
}
