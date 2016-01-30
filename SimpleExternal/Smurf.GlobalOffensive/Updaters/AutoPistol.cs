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

            ReadSettigns();

            if (!_autoPistol)
                return;

            //TODO Fix so we only shoot if we are active in the csgo window.
            if (Smurf.KeyUtils.KeyIsDown(0x02) && Smurf.LocalPlayerWeapon.WeaponGroup == "Pistol")
            {
                if (!(new TimeSpan(DateTime.Now.Ticks - _triggerLastShot).TotalMilliseconds >= _delay))
                    return;

                _triggerLastShot = DateTime.Now.Ticks;

               Shoot();
            }
        }

        private void ReadSettigns()
        {
            _autoPistol = Smurf.Settings.GetBool("Misc", "Auto Pistol");
            _delay = Smurf.Settings.GetInt("Misc", "Auto Pistol Delay");
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
