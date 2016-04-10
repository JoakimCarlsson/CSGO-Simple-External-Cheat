using System;
using ExternalUtilsCSharp.MathObjects;

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

        public static Vector2[] WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, params Vector3[] points)
        {
            Vector2[] worlds = new Vector2[points.Length];
            for (int i = 0; i < worlds.Length; i++)
                worlds[i] = viewMatrix.WorldToScreen(screenSize, points[i]);
            return worlds;
        }
        /// <summary>
        /// Translates a 3d-coordinate to a screen-coodinate
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation</param>
        /// <param name="screenSize">The size of the screen which is translated to</param>
        /// <param name="point3D">3d-coordinate of the point to translate</param>
        /// <returns>Translated screen-coodinate</returns>
        public static Vector2 WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, Vector3 point3D)
        {
            Vector2 returnVector = Vector2.Zero;
            float w = viewMatrix[3, 0] * point3D.X + viewMatrix[3, 1] * point3D.Y + viewMatrix[3, 2] * point3D.Z + viewMatrix[3, 3];
            if (w >= 0.01f)
            {
                float inverseX = 1f / w;
                returnVector.X =
                    (screenSize.X / 2f) +
                    (0.5f * (
                    (viewMatrix[0, 0] * point3D.X + viewMatrix[0, 1] * point3D.Y + viewMatrix[0, 2] * point3D.Z + viewMatrix[0, 3])
                    * inverseX)
                    * screenSize.X + 0.5f);
                returnVector.Y =
                    (screenSize.Y / 2f) -
                    (0.5f * (
                    (viewMatrix[1, 0] * point3D.X + viewMatrix[1, 1] * point3D.Y + viewMatrix[1, 2] * point3D.Z + viewMatrix[1, 3])
                    * inverseX)
                    * screenSize.Y + 0.5f);
            }
            return returnVector;
        }
        #endregion
    }
}