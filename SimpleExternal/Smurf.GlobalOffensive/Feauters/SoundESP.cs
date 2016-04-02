using System;
using System.Threading;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class SoundEsp
    {
        #region Fields
        private bool _enabled;
        private float _interval;
        private long _lastBeep;
        private float _range;
        private int _volume;
        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_enabled)
                return;

            var span = new TimeSpan(DateTime.Now.Ticks - _lastBeep);
            if (span.TotalMilliseconds > _interval)
            {
                _lastBeep = DateTime.Now.Ticks;
            }

            var minRange = _range / _interval * (float)span.TotalMilliseconds;

            var leastDist = float.MaxValue;

            foreach (var player in Core.Objects.Players)
            {
                if (player.Id == Core.LocalPlayer.Id)
                    continue;
                if (!player.IsAlive)
                    continue;
                if (player.Team == Core.LocalPlayer.Team)
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

            Beep(_volume, 800, 500);
            Thread.Sleep(5);
            _lastBeep = DateTime.Now.Ticks;
        }
        private bool Beep(int volume, int frequency, int duration)
        {
            try
            {
                double amplitude = volume * 1.27;
                double a = ((amplitude * (System.Math.Pow(2, 15))) / 1000) - 1;
                double deltaFt = 2 * System.Math.PI * frequency / 8000;

                double samples = 441 * (duration / 100);
                int bytes = Convert.ToInt32(samples) * 4;
                int[] hdr = {
                                0x46464952,
                                36 + bytes,
                                0x45564157,
                                0x20746d66,
                                16,
                                0x20001,
                                8000,
                                176400,
                                0x100004,
                                0x61746164,
                                bytes
                            };
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(44 + bytes))
                {
                    using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(memoryStream))
                    {
                        for (int i = 0; i <= hdr.Length - 1; i++)
                        {
                            binaryWriter.Write(hdr[i]);
                        }
                        for (int T = 0; T <= Convert.ToInt32(samples) - 1; T++)
                        {
                            short sample = Convert.ToInt16(a * System.Math.Sin(deltaFt * T));
                            binaryWriter.Write(sample);
                            binaryWriter.Write(sample);
                        }
                        binaryWriter.Flush();
                        memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                        using (System.Media.SoundPlayer sp = new System.Media.SoundPlayer(memoryStream))
                        {
                            sp.PlaySync();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private void ReadSettings()
        {
            _enabled = Core.Settings.GetBool("Sound ESP", "Sound ESP");
            _range = Core.Settings.GetInt("Sound ESP", "Sound Range");
            _interval = Core.Settings.GetInt("Sound ESP", "Sound Interval");
            _volume = Core.Settings.GetInt("Sound ESP", "Sound Volume");
        }
        #endregion
    }
}