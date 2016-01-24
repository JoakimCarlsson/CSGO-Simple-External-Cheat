using System.Numerics;

namespace Smurf.GlobalOffensive.Math
{
    static class MathUtils
    {
        #region Fields

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

        #endregion
    }
}
