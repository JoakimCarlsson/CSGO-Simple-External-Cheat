using System.Numerics;

namespace Smurf.GlobalOffensive.Math
{
    internal static class MathUtils
    {
        #region Fields

        public static float _deg2Rad = (float) (System.Math.PI/180f);
        private static readonly float _rad2Deg = (float) (180f/System.Math.PI);

        #endregion

        #region Methods

        public static Vector3 ClampAngle(this Vector3 src)
        {
            if (src.X > 89f)
                src.X -= 360f;
            else if (src.X < -89f)
                src.X += 360f;
            if (src.Y > 180f)
                src.Y -= 360f;
            else if (src.Y < -180f)
                src.Y += 360f;

            src.Z = 0;
            return src;
        }

        public static float RadiansToDegrees(float rad)
        {
            return rad*_rad2Deg;
        }

        public static double DegreesToRadians(double degrees)
        {
            var radians = System.Math.PI/180*degrees;
            return radians;
        }

        public static Vector3 CalcAngle(this Vector3 src, Vector3 dst)
        {
            var ret = new Vector3();
            var vDelta = src - dst;
            var fHyp = (float) System.Math.Sqrt(vDelta.X*vDelta.X + vDelta.Y*vDelta.Y);

            ret.X = RadiansToDegrees((float) System.Math.Atan(vDelta.Z/fHyp));
            ret.Y = RadiansToDegrees((float) System.Math.Atan(vDelta.Y/vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }
        public static Vector3 NormalizeAngle(this Vector3 angles)
        {
            if (angles.X > 89)
            {
                angles.X = 89;
            }
            else if (-89 > angles.X)
            {
                angles.X = -89;
            }

            if (angles.Y > 180)
            {
                angles.Y -= 360;
            }
            else if (-180 > angles.Y)
            {
                angles.Y += 360;
            }

            angles.Z = 0;

            return angles;
        }

        #endregion
    }
}