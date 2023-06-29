using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTMDIContainer;
using static MissionPlanner.AFTNewMission;
using static MissionPlanner.AFTSettingsCam;

namespace MissionPlanner
{
    public partial class AFTSaveMissionAs : Form
    {
        public AFTSaveMissionAs()
        {
            InitializeComponent();
        }

        private void btnSaveMission_Click(object sender, EventArgs e)
        {
            /*Save flight settings*/

            // Close all settings
            if (!((aftSetCam == null) || aftSetCam.IsDisposed))
            {
                aftSetCam.Close();
            }

            if (!((aftSetAlt == null) || aftSetAlt.IsDisposed))
            {
                aftSetAlt.Close();
            }

            if (!((aftSetOri == null) || aftSetOri.IsDisposed))
            {
                aftSetOri.Close();
            }

            if (!((aftSetSpeed == null) || aftSetSpeed.IsDisposed))
            {
                aftSetSpeed.Close();
            }

            if (!((aftSetBat == null) || aftSetBat.IsDisposed))
            {
                aftSetBat.Close();
            }

            if (!((aftSetGrid == null) || aftSetGrid.IsDisposed))
            {
                aftSetGrid.Close();
            }

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                aftSetAdv.Close();
            }

            if (!((aftSaveMission == null) || aftSaveMission.IsDisposed))
            {
                aftSaveMission.Close();
            }

            this.Close();
        }
    }
}
