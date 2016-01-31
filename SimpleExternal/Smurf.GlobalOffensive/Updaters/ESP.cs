using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class ESP
    {
        private bool _glowActive, _glowFriendly, _glowEnemies;
        private WinAPI.VirtualKeyShort _glowKey;
        public IntPtr GlowPointer { get; set; }



        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (Smurf.KeyUtils.KeyWentUp(_glowKey))
            {
                _glowActive = !_glowActive;
            }

            if (!_glowActive)
                return;

            Glow();
        }

        private void Glow()
        {
            GlowPointer = Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.GlowObject);

            #region Player Glow
            foreach (var player in Smurf.Objects.Players.Where(player => !player.IsDormant).Where(player => player.IsAlive))
            {
                if (_glowEnemies)
                {
                    if (player.Team != Smurf.LocalPlayer.Team)
                    {
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x4, 0.7f); //red
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x8, 0.1f); //green
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0xC, 0.0f); //blue
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x10, 0.8f);
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x24, true);
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x25, false);
                    }
                }
                if (_glowFriendly)
                {
                    if (player.Team == Smurf.LocalPlayer.Team)
                    {
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x4, 0.0f); //red
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x8, 0.3f); //green
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0xC, 0.5f); //blue
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x10, 0.8f);
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x24, true);
                        Smurf.Memory.Write(GlowPointer + (player.GlowIndex * 0x38) + 0x25, false);
                    }
                }
            }
            #endregion
        }

        private void ReadSettings()
        {
            try
            {
                if (_glowKey == 0)
                {
                    _glowActive = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Enabled");
                    _glowFriendly = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Allies");
                    _glowEnemies = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Enemies");
                    _glowKey = (WinAPI.VirtualKeyShort)Convert.ToInt32(Smurf.Settings.GetString("Glow ESP", "Glow ESP Key"), 16);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
