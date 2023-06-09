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
    public partial class AFTSettingsGrid : Form
    {
        public AFTSettingsGrid()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowBatSettings();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

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

        private void btnOrientation_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowOriSettings();
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
    }
}
