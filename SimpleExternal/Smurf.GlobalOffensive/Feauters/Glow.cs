using System;
using System.Linq;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Glow
    {
        #region Fields

        private bool _glowActive, _glowFriendly, _glowEnemies;

        #endregion

        #region Properties

        public IntPtr GlowPointer { get; set; }

        #endregion

        #region Constructor

        public Glow()
        {
            GlowPointer = Core.Memory.Read<IntPtr>(Core.ClientBase + Offsets.Misc.GlowObject);
        }

        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate(false, false, false))
                return;

            ReadSettings();

            if (!_glowActive)
                return;

            DoGlow();
        }

        private void DoGlow()
        {

            #region Player Glow

            foreach (var player in Core.Objects.Players.Where(player => !player.IsDormant).Where(player => player.IsAlive))
            {
                if (_glowEnemies)
                {
                    if (player.Team != Core.LocalPlayer.Team)
                    {
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x4, 0.7f); //red
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x8, 0.1f); //green
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0xC, 0.0f); //blue
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x10, 0.8f);
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x24, true);
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x25, false);
                    }
                }
                if (_glowFriendly)
                {
                    if (player.Team == Core.LocalPlayer.Team)
                    {
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x4, 0.0f); //red
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x8, 0.3f); //green
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0xC, 0.5f); //blue
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x10, 0.8f);
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x24, true);
                        Core.Memory.Write(GlowPointer + player.GlowIndex * 0x38 + 0x25, false);
                    }
                }
            }

            #endregion
        }

        private void ReadSettings()
        {
            try
            {
                _glowActive = Core.Settings.GetBool("Glow ESP", "Glow ESP Enabled");
                _glowFriendly = Core.Settings.GetBool("Glow ESP", "Glow ESP Allies");
                _glowEnemies = Core.Settings.GetBool("Glow ESP", "Glow ESP Enemies");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}