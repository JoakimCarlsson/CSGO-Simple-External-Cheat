using System;
using System.Numerics;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{

    public class Rcs
    {
        #region Fields
        public Vector3 NewViewAngels;
        private float _maxYaw, _maxPitch, _minYaw, _minPitch;
        private bool _rcsEnabled;
        private int _rcsStart;
        private string _currentWeapon;
        #endregion

        #region Properties

        public Vector3 ViewAngels { get; set; }
        public Vector3 LastPunch { get; set; }
        #endregion

        #region Methods

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;

            if (Smurf.LocalPlayerWeapon.Clip1 == 0)
                return;

            ReadSettïngs();

            if (!_rcsEnabled)
                return;

            ControlRecoil();
            LastPunch = Smurf.LocalPlayer.VecPunch;
        }

        public void ControlRecoil(bool aimbot = false)
        {
            if (!Smurf.TriggerBot.AimOntarget)
                if (Smurf.LocalPlayer.ShotsFired <= _rcsStart)
                    return;

            ViewAngels = Smurf.Memory.Read<Vector3>((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles));
            NewViewAngels = ViewAngels;

            var punch = Smurf.LocalPlayer.VecPunch - LastPunch;
            if ((punch.X != 0 || punch.Y != 0))
            {
                NewViewAngels.X -= punch.X * _maxYaw;
                NewViewAngels.Y -= punch.Y * _maxPitch;
                SetViewAngles(NewViewAngels);
            }
        }

        private void ReadSettïngs()
        {
            _rcsEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            _maxYaw = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Yaw");
            _maxPitch = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Pitch");
            _rcsStart = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Start");
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles.ClampAngle();
            viewAngles = NormalizeAngle(viewAngles);
            Smurf.Memory.Write((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        Vector3 NormalizeAngle(Vector3 angles)
        {
            if (angles.X > 89)
            {
                angles.X = 89;
            }
            else if (-89 > angles.X)
            {
                angles.X = -89;
            }

            if (angles.Y > 180)
            {
                angles.Y -= 360;
            }
            else if (-180 > angles.Y)
            {
                angles.Y += 360;
            }

            angles.Z = 0;

            return angles;
        }
        #endregion
    }
}
