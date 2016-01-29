using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smurf.GlobalOffensive.Updaters
{
    public class SoundESP
    {
        private int _sound;
        private long _lastBeep;
        private bool _enabled;
        private float _range;
        private float _interval;
        private float _volume;


        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_enabled)
                return;
            Smurf.SoundManager.SetVolume(_volume / 100f);

            var span = new TimeSpan(DateTime.Now.Ticks - _lastBeep);
            if (span.TotalMilliseconds > _interval)
            {
                _lastBeep = DateTime.Now.Ticks;
            }

            var minRange = _range / _interval * (float)span.TotalMilliseconds;

            var leastDist = float.MaxValue;

            foreach (var player in Smurf.Objects.Players)
            {
                if (player.Id == Smurf.LocalPlayer.Id)
                    continue;
                if (!player.IsAlive)
                    continue;
                if (player.Team == Smurf.LocalPlayer.Team)
                    continue;
                if (player.IsDormant)
                    continue;

                var distance = player.DistanceMeters;

                if (!(distance <= minRange))
                    continue;
                leastDist = distance;
                break;
            }
            if (leastDist == float.MaxValue)
                return;

            //Smurf.SoundManager.Play(0);
            Smurf.SoundManager.Play(_sound - 1);
            Thread.Sleep(50);
            _lastBeep = DateTime.Now.Ticks;
        }

        private void ReadSettings()
        {
            _enabled = Smurf.Settings.GetBool("Sound ESP", "Sound ESP");
            _range = Smurf.Settings.GetInt("Sound ESP", "Sound Range");
            _interval = Smurf.Settings.GetInt("Sound ESP", "Sound Interval");
            _volume = Smurf.Settings.GetInt("Sound ESP", "Sound Volume");
            _sound = Smurf.Settings.GetInt("Sound ESP", "Sound Track");
        }
    }
}
