using System.Threading;

namespace Smurf.GlobalOffensive.Updaters
{
    public class TriggerBot
    {
        private bool _triggerEnabled = true;
        private bool _triggerAllies = true;
        private bool _triggerEnemies = true;
        private bool _spawnProtection = false;
        private bool _triggerBurst = true;
        private bool _triggerBurstRandom = true;
        private int _burstMin = 0;
        private int _burstMax = 1;
        private int _delayFirstShotMax = 32;
        private int _delayFirstShotMin = 32;
        private int _delayShotsMax = 32;
        private int _delayShotsMin = 32;

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
                    //If our targets is immune in gungame / deathmatch.
                    if (_spawnProtection)
                        if (target.GunGameImmune)
                            return;

                    Shoot();
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
