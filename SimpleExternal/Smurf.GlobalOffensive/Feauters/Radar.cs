using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class Radar
    {
        private bool _radar;

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            ReadSettings();

            if (!_radar)
                return;

            foreach (var player in Core.Objects.Players)
            {
                if (player.Team == Core.LocalPlayer.Team)
                    continue;
                if (!player.IsAlive)
                    continue;

                if (!player.IsDormant && !player.IsSpotted)
                    Core.Memory.Write(player.BaseAddress + Offsets.BaseEntity.Spotted, 1);
            }
        }

        private void ReadSettings()
        {
            _radar = Core.Settings.GetBool("Misc", "Radar");
        }
    }
}