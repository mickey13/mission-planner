using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTNewMission;
using static MissionPlanner.AFTSettingsCam;

namespace MissionPlanner
{
    public partial class AFTSettingsOri : Form
    {
        public AFTSettingsOri()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowAltSettings();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowSpeedSettings();
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowCamSettings();
        }

        private void btnAltitude_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowAltSettings();
        }

        private void btnSpeed_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowSpeedSettings();
        }

        private void btnBattery_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowBatSettings();
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowGridSettings();
        }
    }
}
