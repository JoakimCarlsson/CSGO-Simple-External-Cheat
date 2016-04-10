using System;
using System.Numerics;

namespace Smurf.GlobalOffensive.Utils
{
    internal static class MathUtils
    {
        #region Fields

        public static float Deg2Rad = (float)(Math.PI / 180f);
        private static readonly float Rad2Deg = (float)(180f / Math.PI);

        #endregion

        #region Methods
        private static void SmoothAim(Vector3 _viewAngels, Vector3 dst, float smoothAmount)
        {
            var smoothAngle = dst - _viewAngels;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            smoothAngle /= smoothAmount;
            smoothAngle += _viewAngels;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            if (smoothAngle != Vector3.Zero)
                Core.ControlRecoil.SetViewAngles(smoothAngle);
        }

        public static Vector3 ClampAngle(this Vector3 angles)
        {
            if (angles.Y > 180.0f)
                angles.Y = 180.0f;

            if (angles.Y < -180.0f)
                angles.Y = -180.0f;

            if (angles.X > 89.0f)
                angles.X = 89.0f;

            if (angles.X < -89.0f)
                angles.X = -89.0f;

            angles.Z = 0;
            return angles;
        }
        public static float Fov(Vector3 viewAngle, Vector3 dst, float distance)
        {
            float pitch = (float)(Math.Sin(DegreesToRadians(viewAngle.X - dst.X)) * distance);
            float yaw = (float)(Math.Sin(DegreesToRadians(viewAngle.Y - dst.Y)) * distance);

            return (float)Math.Sqrt(Math.Pow(pitch, 2) + Math.Pow(yaw, 2));
        }
        public static float Fov(Vector3 viewangel, Vector3 dst)
        {
            return (float)Math.Sqrt(Math.Pow(dst.X - viewangel.X, 2) + Math.Pow(dst.Y - viewangel.Y, 2));
        }
        public static Vector3 SmoothAngle(this Vector3 src, Vector3 dest, float smoothAmount)
        {
            return src + (dest - src) * smoothAmount;
        }
        public static float RadiansToDegrees(float rad)
        {
            return rad * Rad2Deg;
        }

        public static double DegreesToRadians(double degrees)
        {
            var radians = Math.PI / 180 * degrees;
            return radians;
        }

        public static Vector3 CalcAngle(this Vector3 src, Vector3 dst)
        {
            var ret = new Vector3();
            var vDelta = src - dst;
            var fHyp = (float)Math.Sqrt(vDelta.X * vDelta.X + vDelta.Y * vDelta.Y);

            ret.X = RadiansToDegrees((float)Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }

        public static Vector3 CalcAngle(Vector3 src, Vector3 dst, Vector3 angles)
        {

            Vector3 delta;
            delta.X = (src.X - dst.X);
            delta.Y = (src.Y - dst.Y);
            delta.Z = (src.Z - dst.Z);

            double hyp = Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            angles.X = (float)(Math.Atan(delta.Z / hyp) * 57.295779513082f);
            angles.Y = (float)(Math.Atan(delta.Y / delta.X) * 57.295779513082f);


            angles.Z = 0.0f;
            if (delta.X >= 0.0) { angles.Y += 180.0f; }
            return angles;
        }

        public static Vector3 NormalizeAngle(this Vector3 angle)
        {
            //This is probably unnecessary, but its the way the engine does it so fuck it
            while (angle.X > 89.0f)
            {
                angle.X -= 180f;
            }

            while (angle.X < -89.0f)
            {
                angle.X += 180f;
            }

            while (angle.Y > 180f)
            {
                angle.Y -= 360f;
            }

            while (angle.Y < -180f)
            {
                angle.Y += 360f;
            }
            return angle;
        }
        #endregion
    }
}