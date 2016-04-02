using System;
using System.Numerics;
using Smurf.GlobalOffensive.MathUtils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Rcs
    {
        #region Fields

        private Vector3 _newViewAngels;
        public float MaxYaw, MaxPitch;
        public bool RcsEnabled;
        private int _rcsStart;
        private float _pixelsX, _pixelsY;

        #endregion

        #region Properties

        public Vector3 ViewAngels { get; set; }
        public Vector3 LastPunch { get; set; }

        #endregion

        #region Methods

        public void Update()
        {
            if (!Core.Objects.ShouldUpdate())
                return;



            ReadSettïngs();

            if (!RcsEnabled)
                return;

            ControlRecoil();
            LastPunch = Core.LocalPlayer.VecPunch;
        }

        public void ControlRecoil(bool aimbot = false)
        {
            if (!Core.TriggerBot.AimOntarget)
                if (Core.LocalPlayer.ShotsFired <= _rcsStart)
                    return;

            if (Core.LocalPlayerWeapon.Clip1 == 0)
                return;

            ViewAngels = Core.Memory.Read<Vector3>((IntPtr) (Core.ClientState + Offsets.ClientState.ViewAngles));
            _newViewAngels = ViewAngels;

            var punch = Core.LocalPlayer.VecPunch - LastPunch;
            if (punch.X != 0 || punch.Y != 0)
            {
                _newViewAngels.X -= punch.X * MaxYaw;
                _newViewAngels.Y -= punch.Y * MaxPitch;
                SetViewAngles(_newViewAngels);
            }
        }

        private void ReadSettïngs()
        {
            RcsEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            MaxYaw = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Yaw");
            MaxPitch = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Pitch");
            _rcsStart = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Rcs Start");
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            Core.Memory.Write((IntPtr) (Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        #endregion
    }
}