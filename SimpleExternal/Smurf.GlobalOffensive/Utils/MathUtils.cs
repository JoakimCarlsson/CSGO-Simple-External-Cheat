using System;
using System.Numerics;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.SDK;

namespace Smurf.GlobalOffensive.Utils
{
    internal static class MathUtils
    {
        #region Fields

        public static float Deg2Rad = (float)(Math.PI / 180f);
        private static readonly float Rad2Deg = (float)(180f / Math.PI);

        #endregion

        #region Methods
        public static Vector3 SmoothAim(Vector3 viewAngle, Vector3 destination, float smoothAmount)
        {
            var smoothAngle = destination - viewAngle;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            smoothAngle /= smoothAmount;
            smoothAngle += viewAngle;

            smoothAngle = smoothAngle.NormalizeAngle();
            smoothAngle = smoothAngle.ClampAngle();

            return smoothAngle;
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

        public static float Fov(Vector3 viewAngle, Vector3 destination, float distance)
        {
            float pitch = (float)(Math.Sin(DegreesToRadians(viewAngle.X - destination.X)) * distance);
            float yaw = (float)(Math.Sin(DegreesToRadians(viewAngle.Y - destination.Y)) * distance);

            return (float)Math.Sqrt(Math.Pow(pitch, 2) + Math.Pow(yaw, 2));
        }

        public static float Fov(Vector3 viewAngle, Vector3 destination)
        {
            return (float)Math.Sqrt(Math.Pow(destination.X - viewAngle.X, 2) + Math.Pow(destination.Y - viewAngle.Y, 2));
        }

        public static Vector3 SmoothAngle(this Vector3 source, Vector3 destination, float smoothAmount)
        {
            return source + (destination - source) * smoothAmount;
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

        public static Vector3 CalcAngle(this Vector3 source, Vector3 destination)
        {
            var ret = new Vector3();
            var vDelta = source - destination;
            var fHyp = (float)Math.Sqrt(vDelta.X * vDelta.X + vDelta.Y * vDelta.Y);

            ret.X = RadiansToDegrees((float)Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }

        public static Vector3 CalcAngle(Vector3 source, Vector3 destination, Vector3 angles)
        {

            Vector3 delta;
            delta.X = (source.X - destination.X);
            delta.Y = (source.Y - destination.Y);
            delta.Z = (source.Z - destination.Z);

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

        public static float Get3DDistance(Vector3 playerPosition, Vector3 enemyPosition)
        {
            return (float)Math.Sqrt(Math.Pow(enemyPosition.X - playerPosition.X, 2f) + Math.Pow(enemyPosition.Y - playerPosition.Y, 2f) + Math.Pow(enemyPosition.Z - playerPosition.Z, 2f));
        }

        public static Vector3 AngleToTarget(Player target, int bone)
        {
            Vector3 myView = Core.LocalPlayer.Position + Core.LocalPlayer.VecView;
            Vector3 aimView = target.GetBonePos(target, bone);
            Vector3 dst = myView.CalcAngle(aimView);
            dst = dst.NormalizeAngle();
            return dst;
        }

        public static Vector3 CalcAngle(Vector3 playerPosition, Vector3 enemyPosition, Vector3 punchAngle, Vector3 viewOffset, float yawRecoilReductionFactor, float pitchRecoilReductionFactor)
        {
            Vector3 aimAngle = new Vector3(0, 0, 0);
            Vector3 delta = new Vector3(playerPosition.X - enemyPosition.X, playerPosition.Y - enemyPosition.Y, (playerPosition.Z + viewOffset.Z) - enemyPosition.Z);
            float hyp = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);

            aimAngle.X = (float)Math.Atan(delta.Z / hyp) * 57.29578f - punchAngle.X * yawRecoilReductionFactor;
            aimAngle.Y = (float)Math.Atan(delta.Y / delta.X) * 57.29578f - punchAngle.Y * pitchRecoilReductionFactor;
            aimAngle.Z = 0;

            if (delta.X >= 0.0)
                aimAngle.Y += 180f;
            return aimAngle;
        }
        #endregion
    }
}