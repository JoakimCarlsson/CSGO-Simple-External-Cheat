using System;
using System.Numerics;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Rcs
    {
        #region Fields

        private Vector3 _newViewAngels;
        public float MaxYaw, MaxPitch, MinYaw, MinPitch, RandomYaw, RandomPitch;
        public bool RcsEnabled;
        private int _rcsStart;
        #endregion

        #region Properties

        public Vector3 ViewAngels { get; set; }
        public Vector3 LastPunch { get; set; }

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettïngs();

            if (!RcsEnabled)
                return;

            ControlRecoil();
            LastPunch = Core.LocalPlayer.VecPunch;
        }

        public void ControlRecoil(bool aimbot = false)
        {
            RandomizeRecoilControl();

            if (!Core.TriggerBot.AimOntarget)
                if (Core.LocalPlayer.ShotsFired <= _rcsStart)
                    return;

            if (Core.LocalPlayerWeapon.Clip1 == 0)
                return;

            ViewAngels = Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));
            _newViewAngels = ViewAngels;

            Vector3 punch = Core.LocalPlayer.VecPunch - LastPunch;
            if (punch.X != 0 || punch.Y != 0)
            {
                _newViewAngels.X -= punch.X * RandomYaw;
                _newViewAngels.Y -= punch.Y * RandomPitch;
                SetViewAngles(_newViewAngels);
            }
        }

        private void RandomizeRecoilControl()
        {
            if (Core.LocalPlayer.ShotsFired == 1)
            {
                float tempMinYaw = MinYaw * 10;
                float tempMinPitch = MinPitch * 10;
                float tempMaxYaw = MaxYaw * 10;
                float tempMaxPitch = MaxPitch * 10;

                float tempRandomYaw = new Random().Next((int)tempMinYaw, (int)tempMaxYaw) + 1;
                float tempRandomPitch = new Random().Next((int)tempMinPitch, (int)tempMaxPitch) + 1;

                RandomYaw = tempRandomYaw / 10;
                RandomPitch = tempRandomPitch / 10;
            }

        }

        private void ReadSettïngs()
        {
            RcsEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Rcs Enabled");
            MaxYaw = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Max Yaw");
            MaxPitch = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Max Pitch");
            MinYaw = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Min Yaw");
            MinPitch = Core.Settings.GetFloat(Core.LocalPlayerWeapon.WeaponName, "Rcs Force Min Pitch");
            _rcsStart = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Rcs Start");
        }

        public void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            Core.Memory.Write((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        #endregion
    }
}