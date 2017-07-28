using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Smurf.GlobalOffensive.Objects;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class AimAssist
    {
        #region Fields

        private float _reTargetTime;//Not implemented.
        private bool _randomBone; //Not implemented.
        private List<int> _randomBones; //Not implemented. the random bones we'll use will be in this list (?)

        private bool _aimAssistEnabled;
        private WinAPI.VirtualKeyShort _aimKey;
        private bool _aimHumanized;
        private bool _aimSpotted;
        private bool _aimEnemies;
        private bool _aimAllies;
        private int _aimFov;
        private int _aimBone;
        private int _aimSpeed;

        public Player AimTarget;
        private readonly List<Player> _players = new List<Player>();

        #endregion

        #region Methos

        public void Update()
        {

            return;

            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_aimAssistEnabled)
                return;

            GetPlayers();

            if (Core.KeyUtils.KeyIsDown(_aimKey))
            {
                if (AimTarget == null)
                {
                    AimTarget = GetTarget();
                }
                else
                {
                    Aim();
                }
                if (AimTarget != null)
                {
                    if (AimTarget.IsDormant || !AimTarget.IsAlive)
                    {
                        AimTarget = null;
                    }
                    if (Core.KeyUtils.KeyWentUp(_aimKey))
                    {
                        AimTarget = null;
                    }
                }

            }
        }

        private void ReadSettings()
        {
            try
            {
                _aimAssistEnabled = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Enabled");
                _aimKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Core.Settings.GetString(Core.LocalPlayerWeapon.WeaponName, "Aim Key"), 16);
                _aimFov = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Fov");
                _aimHumanized = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Humanized");
                _aimSpotted = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Spotted");
                _aimEnemies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Enemies");
                _aimAllies = Core.Settings.GetBool(Core.LocalPlayerWeapon.WeaponName, "Aim Allies");
                _aimSpeed = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Speed");
                _aimBone = Core.Settings.GetInt(Core.LocalPlayerWeapon.WeaponName, "Aim Bone");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void GetPlayers()
        {
            for (var i = 0; i < 32; i++)
            {
                BaseEntity entity = new BaseEntity(Core.Objects.GetEntityPtr(i));

                if (!entity.IsValid)
                    continue;

                if (entity.IsPlayer())
                    _players.Add(new Player(Core.Objects.GetEntityPtr(i)));
            }
        }

        private void Aim()
        {
            if (!AimTarget.IsAlive || AimTarget.IsDormant)
                return;

            Vector3 destination = MathUtils.AngleToTarget(AimTarget, _aimBone);
            Vector3 currentViewAngles = Engine.GetViewAngles();


            if (_aimHumanized) //Pretty bad, needs to be imrpoved. 
            {
                Vector3 scale = new Vector3(1.5f, 5.7f, 0f);

                Vector3 viewAngleDelta = destination - currentViewAngles;

                viewAngleDelta += new Vector3(viewAngleDelta.Y / scale.Y, viewAngleDelta.X / scale.X, 0.0f);
                viewAngleDelta -= new Vector3(Core.LocalPlayer.VecPunch.X * 2f, Core.LocalPlayer.VecPunch.Y * 2f, 0);
                viewAngleDelta /= _aimSpeed;
                Vector3 vecViewAngles = currentViewAngles + viewAngleDelta;

                Engine.SetViewAngles(vecViewAngles);
            }
            else
            {
                destination -= new Vector3(Core.LocalPlayer.VecPunch.X *2f, Core.LocalPlayer.VecPunch.Y *2f, 0);
                Vector3 newDestination = MathUtils.SmoothAim(Engine.GetViewAngles(), destination, _aimSpeed);

                if (newDestination != Vector3.Zero)
                    Engine.SetViewAngles(newDestination);
            }
        }

        private Player GetTarget()
        {
            IEnumerable<Player> targetList = _players.Where(p => p.Id != Core.LocalPlayer.Id && p.IsAlive && !p.IsDormant);
            //if (_aimSpotted)
            //    targetList = targetList.Where(p => p.SeenBy(Core.LocalPlayer));
            if (_aimEnemies)
                targetList = targetList.Where(p => p.Team != Core.LocalPlayer.Team);
            if (_aimAllies)
                targetList = targetList.Where(p => p.Team == Core.LocalPlayer.Team);

            targetList = targetList.OrderBy(p => p.DistanceMeters);

            foreach (Player target in targetList)
            {
                Vector3 dst = MathUtils.AngleToTarget(target, _aimBone);
                float fov = MathUtils.Fov(Engine.GetViewAngles(), dst, Vector3.Distance(Core.LocalPlayer.Position, target.Position) / 10);

                if (fov <= _aimFov)
                {
                    return target;
                }
            }
            return null;
        }

        #endregion
    }
}
