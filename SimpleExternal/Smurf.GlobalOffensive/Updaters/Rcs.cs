using System;
using System.Numerics;
using System.Security.AccessControl;
using Smurf.GlobalOffensive.Math;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{

    public class Rcs
    {
        #region Fields
        public Vector3 NewViewAngels;
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

            if (Smurf.LocalPlayer.ShotsFired >= 1)
            {
                ViewAngels = Smurf.Memory.Read<Vector3>((IntPtr)(Smurf.ClientState + Offsets.ClientState.ViewAngles));
                NewViewAngels = ViewAngels;

                var punch = Smurf.LocalPlayer.VecPunch - LastPunch;

                if (punch.X != 0 || punch.Y != 0)
                {
                    //Yaw
                    NewViewAngels.X -= punch.X * 2f;

                    //Pitch
                    NewViewAngels.Y -= punch.Y * 2.2f;

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
