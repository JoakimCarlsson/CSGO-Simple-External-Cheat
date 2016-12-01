using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smurf.GlobalOffensive.SDK;
using Smurf.GlobalOffensive.Utils;

namespace Smurf.GlobalOffensive.Feauters
{
    public class SkinChanger
    {
        #region Fields

        private WinAPI.VirtualKeyShort _forceUpdate = (WinAPI.VirtualKeyShort) 0x24; //Home Key


        #endregion

        #region Methods

        public void Update()
        {
            if (!MiscUtils.ShouldUpdate())
                return;

            //ReadSettings();

            if (Core.KeyUtils.KeyWentUp(_forceUpdate))
                Engine.ForceUpdate();

        }



        private void ReadSettings()
        {
            
        }

        #endregion
    }
}
