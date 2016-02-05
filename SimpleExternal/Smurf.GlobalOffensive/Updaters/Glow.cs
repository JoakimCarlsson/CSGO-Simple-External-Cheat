//using System;
//using System.Linq;
//using Smurf.GlobalOffensive.Patchables;

//namespace Smurf.GlobalOffensive.Updaters
//{
//    public class Glow
//    {
//        private bool _glowActive, _glowFriendly, _glowEnemies;
//        public IntPtr GlowPointer { get; set; }

//        public void Update()
//        {
//            if (!Smurf.Objects.ShouldUpdate(false, false, false))
//                return;

//            ReadSettings();

//            if (!_glowActive)
//                return;

//            DoGlow();
//        }

//        private void DoGlow()
//        {
//            GlowPointer = Smurf.Memory.Read<IntPtr>(Smurf.ClientBase + Offsets.Misc.GlowObject);

//            #region Player Glow

//            foreach ( var player in Smurf.Objects.Players.Where(player => !player.IsDormant).Where(player => player.IsAlive))
//            {
//                if (_glowEnemies)
//                {
//                    if (player.Team != Smurf.LocalPlayer.Team)
//                    {
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x4, 0.7f); //red
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x8, 0.1f); //green
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0xC, 0.0f); //blue
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x10, 0.8f);
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x24, true);
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x25, false);
//                    }
//                }
//                if (_glowFriendly)
//                {
//                    if (player.Team == Smurf.LocalPlayer.Team)
//                    {
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x4, 0.0f); //red
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x8, 0.3f); //green
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0xC, 0.5f); //blue
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x10, 0.8f);
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x24, true);
//                        Smurf.Memory.Write(GlowPointer + player.GlowIndex*0x38 + 0x25, false);
//                    }
//                }
//            }

//            #endregion
//        }

//        private void ReadSettings()
//        {
//            try
//            {
//                    _glowActive = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Enabled");
//                    _glowFriendly = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Allies");
//                    _glowEnemies = Smurf.Settings.GetBool("Glow ESP", "Glow ESP Enemies");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }
//    }
//}