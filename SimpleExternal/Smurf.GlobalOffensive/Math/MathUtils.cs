using System;
using System.Numerics;

namespace Smurf.GlobalOffensive.Math
{
    internal static class MathUtils
    {
        #region Fields

        public static float Deg2Rad = (float)(System.Math.PI / 180f);
        private static readonly float Rad2Deg = (float)(180f / System.Math.PI);

        #endregion

        #region Methods

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
        public static float Fov(Vector3 viewAngle, Vector3 destination)
        {
            float deltaX = (viewAngle.X - destination.X);
            float deltaY = (viewAngle.Y - destination.Y);
            //if (deltaX > 180)
            //    deltaX -= 180;
            //if (deltaY > 180)
            //    deltaY -= 180;
            //if (deltaX < 0.0)
            //    deltaX -= deltaX * 2;
            //if (deltaY < 0.0)
            //    deltaY -= deltaY * 2;
            float fov = deltaX + deltaY;
            Console.WriteLine(fov);
            return fov;
        }
        public static float Fov2(Vector3 viewangel, Vector3 dst)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(dst.X - viewangel.X, 2) + System.Math.Pow(dst.Y - viewangel.Y, 2));
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
            var radians = System.Math.PI / 180 * degrees;
            return radians;
        }

        public static Vector3 CalcAngle(this Vector3 src, Vector3 dst)
        {
            var ret = new Vector3();
            var vDelta = src - dst;
            var fHyp = (float)System.Math.Sqrt(vDelta.X * vDelta.X + vDelta.Y * vDelta.Y);

            ret.X = RadiansToDegrees((float)System.Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)System.Math.Atan(vDelta.Y / vDelta.X));

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

            double hyp = System.Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            angles.X = (float)(System.Math.Atan(delta.Z / hyp) * 57.295779513082f);
            angles.Y = (float)(System.Math.Atan(delta.Y / delta.X) * 57.295779513082f);


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