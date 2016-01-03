using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive.Updaters
{
    public class TriggerBot
    {
        private bool _triggerAllies = false;
        private bool _triggerEnemies = false;
        //private bool _spawnProtection = false;

        public void Update()
        {
            if (Smurf.LocalPlayer == null)
                return;

            var target = Smurf.LocalPlayer.Target;
            if (target == null)
                return;

            if (Smurf.KeyUtils.KeyIsDown(18)) //ALT
            {
                if ((_triggerAllies && target.Team == Smurf.LocalPlayer.Team) || (_triggerEnemies && target.Team != Smurf.LocalPlayer.Team))
                {

                }
            }
        }

        private static void Shoot()
        {
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
    }
}
