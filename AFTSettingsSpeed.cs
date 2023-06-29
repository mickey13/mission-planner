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
using static MissionPlanner.AFTMDIContainer;

namespace MissionPlanner
{
    public partial class AFTSettingsSpeed : Form
    {
        public AFTSettingsSpeed()
        {
            InitializeComponent();

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                trackSpeed.Value = aftSetAdv.trackSpeedAdv.Value;
                lblSpeedDisplay.Text = trackSpeed.Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowOriSettings();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowBatSettings();
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowCamSettings();
        }

        private void btnAltitude_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowAltSettings();
        }

        private void btnOrientation_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowOriSettings();
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

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings();
        }

        private void trackSpeed_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackSpeed, trackSpeed.Value.ToString());
            lblSpeedDisplay.Text = trackSpeed.Value.ToString();
        }
    }
}
