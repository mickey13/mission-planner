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
            //this.Dispose();
            ShowAltSettings();
            //this.Hide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowSpeedSettings();
            //this.Hide();
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowCamSettings();
            //this.Hide();
        }

        private void btnAltitude_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowAltSettings();
        }

        private void btnSpeed_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowSpeedSettings();
        }

        private void btnBattery_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowBatSettings();
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowGridSettings();
        }

        // Selecting fixed direction
        private void btnFxdDir_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnFxdDir, this);
        }

        private void btnTgtPtOri_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnTgtPtOri, this);
        }

        private void btnSglDir_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnSglDir, this);
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings();
        }
    }
}
