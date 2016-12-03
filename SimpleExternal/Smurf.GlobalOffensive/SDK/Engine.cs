using System;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.SDK
{
    internal static class Engine
    {
        #region Methods

        public static void ForceUpdate()
        {
            Core.Memory.Write((IntPtr)(Core.ClientState + 0x16c), -1);
        }

        public static void ForceAttack(int delay1, int delay2, int delay3)
        {
            Thread.Sleep(delay1);
            Core.Memory.Write(Core.ClientBase + Offsets.Misc.ForceAttack, 5);
            Thread.Sleep(delay2);
            Core.Memory.Write(Core.ClientBase + Offsets.Misc.ForceAttack, 4);
            Thread.Sleep(delay3);
        }

        public static void SetViewAngles(Vector3 viewAngles)
        {
            viewAngles = viewAngles.ClampAngle();
            viewAngles = viewAngles.NormalizeAngle();
            Core.Memory.Write((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles), viewAngles);
        }

        public static Vector3 GetViewAngles()
        {
            return Core.Memory.Read<Vector3>((IntPtr)(Core.ClientState + Offsets.ClientState.ViewAngles));
        }

        #endregion
    }
}
