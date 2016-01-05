using System;
using System.Numerics;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{

    public class Rcs
    {
        #region Fields
        private Vector3 NewViewAngels;
        private float Yaw, Pitch;
        private int RcsStart;
        #endregion

        #region Properties

        public Vector3 ViewAngels { get; set; }
        public Vector3 LastPunch { get; set; }
        #endregion

        #region Methods

        public void Update()
        {
            if (Smurf.LocalPlayer == null || Smurf.LocalPlayerWeapon == null)
                return;

            Yaw = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Yaw");
            Pitch = Smurf.Settings.GetFloat(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Force Pitch");
            RcsStart = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Rcs Start");

            if (Smurf.LocalPlayer.ShotsFired > RcsStart)
            {

                if (Smurf.LocalPlayerWeapon.Clip1 == 0)
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

            LastPunch = Smurf.LocalPlayer.VecPunch;
        }
        public static void SetViewAngles(Vector3 viewAngles)
        {
            //TODO Clamp VIewangels before we set them.
            Smurf.Memory.Write((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }
        #endregion
    }
}
