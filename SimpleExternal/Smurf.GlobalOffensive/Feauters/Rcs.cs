using System;
using System.Numerics;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Rcs
    {
        #region Constructor
        public Rcs()
        {
            _sensitivity = Core.Memory.Read<float>(Core.ClientBase + Offsets.Misc.Sensitivity);
        }
        #endregion

        #region Fields

        private Vector3 _newViewAngels;
        private float _maxYaw, _maxPitch, _minYaw, _minPitch;
        public float RandomYaw, RandomPitch;
        private bool _rcsEnabled;
        private int _rcsStart;
        private readonly float _sensitivity;
        private Vector3 _pixels;
        private bool _mouseMovement;

        #endregion

        #region Properties

        private Vector3 LastPunch { get; set; }

        #endregion

        #region Methods
        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_rcsEnabled)
                return;

            ControlRecoil();
            LastPunch = Core.LocalPlayer.VecPunch;
        }

        public void ControlRecoil()
        {
            RandomizeRecoilControl();

            if (!Core.TriggerBot.AimOntarget)
                if (Core.LocalPlayer.ShotsFired <= _rcsStart)
                    return;

            if (Core.LocalPlayerWeapon.Clip1 == 0)
                return;

            if (_mouseMovement)
            {
                Vector3 punch = Core.LocalPlayer.VecPunch - LastPunch;
                _pixels.X = punch.X / (float)(0.22 * _sensitivity * 1) * RandomYaw * 10;
                _pixels.Y = punch.Y / (float)(0.22 * _sensitivity * 1) * RandomPitch * 10;
                WinAPI.mouse_event((uint)0, (uint)_pixels.Y, (uint)-_pixels.X, 0, 0);
            }
            else
            {
                _newViewAngels = Engine.GetViewAngles();
                Vector3 punch = Core.LocalPlayer.VecPunch - LastPunch;

                if (punch.X != 0 || punch.Y != 0)
                {
                    _newViewAngels.X -= punch.X * RandomYaw;
                    _newViewAngels.Y -= punch.Y * RandomPitch;
                    _newViewAngels = _newViewAngels.NormalizeAngle();
                    Engine.SetViewAngles(_newViewAngels);
                }
            }

        }

        private void RandomizeRecoilControl()
        {
            float tempMinYaw = _minYaw * 10;
            float tempMinPitch = _minPitch * 10;
            float tempMaxYaw = _maxYaw * 10;
            float tempMaxPitch = _maxPitch * 10;

            float tempRandomYaw = new Random().Next((int)tempMinYaw, (int)tempMaxYaw) + 1;
            float tempRandomPitch = new Random().Next((int)tempMinPitch, (int)tempMaxPitch) + 1;

            RandomYaw = tempRandomYaw / 10;
            RandomPitch = tempRandomPitch / 10;
        }

        private void ReadSettings()
        {
            _rcsEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            _maxYaw = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Max Yaw");
            _maxPitch = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Max Pitch");
            _minYaw = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Min Yaw");
            _minPitch = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Min Pitch");
            _rcsStart = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Rcs Start");
            _mouseMovement = Core.Settings.GetBool("Misc", "Mouse Movement");
        }

        #endregion
    }
}