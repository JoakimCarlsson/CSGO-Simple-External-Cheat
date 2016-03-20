using System;
using System.Numerics;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Rcs
    {
        #region Fields

        public float MaxYaw, MaxPitch;
        public bool RcsEnabled;
        private int _rcsStart;
        private float _pixelsX, _pixelsY;

        #endregion

        #region Properties

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

            if (!RcsEnabled)
                return;

            ControlRecoil();
            LastPunch = Smurf.LocalPlayer.VecPunch;
        }

        public void ControlRecoil(bool aimbot = false)
        {
            if (!Smurf.TriggerBot.AimOntarget)
                if (Smurf.LocalPlayer.ShotsFired <= _rcsStart)
                    return;

            var punch = Smurf.LocalPlayer.VecPunch - LastPunch;
            Vector3 pixels = punch / (float)(0.22 * 0.28 * 1) * 20;
            WinAPI.mouse_event((uint)0, (uint)pixels.Y, (uint)-pixels.X, 0, 0);

        }

        private void ReadSettïngs()
        {
            RcsEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            MaxYaw = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Yaw");
            MaxPitch = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Pitch");
            _rcsStart = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Start");
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            Smurf.Memory.Write((IntPtr) (Smurf.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        #endregion
    }
}