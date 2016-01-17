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
        private float Yaw, Pitch;
        private bool _rcsEnabled;
        private int RcsStart;
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

        private void ControlRecoil()
        {
            if (!Smurf.TriggerBot.AimOntarget)
                if (Smurf.LocalPlayer.ShotsFired <= RcsStart)
                    return;

            ViewAngels = Smurf.Memory.Read<Vector3>((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles));
            NewViewAngels = ViewAngels;

            var punch = Smurf.LocalPlayer.VecPunch - LastPunch;

            if (punch.X != 0 || punch.Y != 0)
            {
                //Yaw
                NewViewAngels.X -= punch.X * Yaw;

                //Pitch
                NewViewAngels.Y -= punch.Y * Pitch;

                SetViewAngles(NewViewAngels);
            }
        }

        private void ReadSettïngs()
        {
            _rcsEnabled = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            Yaw = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Yaw");
            Pitch = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Pitch");
            RcsStart = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Start");
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles.ClampAngle();
            Smurf.Memory.Write((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }
        #endregion
    }
}
