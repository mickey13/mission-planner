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
    public partial class AFTSettingsAlt : Form
    {
        public AFTSettingsAlt()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ShowCamSettings();
            //this.Hide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowOriSettings();
            //this.Hide();
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            ShowCamSettings();
            //this.Hide();
        }

        private void btnOrientation_Click(object sender, EventArgs e)
        {
            ShowOriSettings();
            //this.Hide();
        }

        private void btnSpeed_Click(object sender, EventArgs e)
        {
            ShowSpeedSettings();
            //this.Hide();
        }

        private void btnBattery_Click(object sender, EventArgs e)
        {
            ShowBatSettings();
            //this.Hide();
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            ShowGridSettings();
            //this.Hide();
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings();
        }
    }
}
