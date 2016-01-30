using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class AutoPistol
    {
        private bool _autoPistol;
        private int _delay;
        private long _triggerLastShot;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;
            if (Smurf.LocalPlayerWeapon.WeaponGroup != "Pistol")
                return;

            ReadSettigns();

            if (!_autoPistol)
                return;

            //TODO Fix so we only shoot if we are active in the csgo window.
            if (Smurf.KeyUtils.KeyIsDown(0x02))
            {
                if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _delay))
                    return;

                _triggerLastShot = DateTime.Now.Ticks;

                Shoot();
            }
        }

        private void ReadSettigns()
        {
            _autoPistol = Smurf.Settings.GetBool(Smurf.LocalPlayerWeapon.WeaponName, "Auto Pistol");
            _delay = Smurf.Settings.GetInt(Smurf.LocalPlayerWeapon.WeaponName, "Auto Pistol Delay");
        }
        public void Shoot()
        {
            Thread.Sleep(10);
            Smurf.Memory.Write(Smurf.ClientBase + Offsets.Misc.ForceAttack, 5);
            Thread.Sleep(10);
            Smurf.Memory.Write(Smurf.ClientBase + Offsets.Misc.ForceAttack, 4);
        }
    }
}
