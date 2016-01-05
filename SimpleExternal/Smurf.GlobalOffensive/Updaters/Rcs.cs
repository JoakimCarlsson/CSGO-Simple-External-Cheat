using System;
using System.Numerics;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{

    public class Rcs
    {
        #region Fields
        public Vector3 NewViewAngels;
        public float Yaw = 2f;
        public float Pitch = 2.3f;
        #endregion

        #region Properties

        public Vector3 ViewAngels { get; set; }
        public Vector3 LastPunch { get; set; }
        #endregion

        #region Methods

        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            if (Smurf.LocalPlayer.ShotsFired > 1)
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
