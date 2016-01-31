using Smurf.GlobalOffensive.Patchables;

namespace Smurf.GlobalOffensive.Updaters
{
    public class Radar
    {
        private bool _radar;

        public void Update()
        {
            if (!Smurf.Objects.ShouldUpdate())
                return;


            ReadSettings();

            if (!_radar)
                return;

            foreach (var player in Smurf.Objects.Players)
            {
                if (player.Team == Smurf.LocalPlayer.Team)
                    continue;
                if (!player.IsAlive)
                    continue;

                if (!player.IsDormant && !player.IsSpotted)
                    Smurf.Memory.Write(player.BaseAddress + Offsets.BaseEntity.Spotted, 1);
            }
        }

        private void ReadSettings()
        {
            _radar = Smurf.Settings.GetBool("Misc", "Radar");
        }
    }
}