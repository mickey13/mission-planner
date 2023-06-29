using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTSettingsAlt : Form
    {
        public AFTSettingsAlt()
        {
            InitializeComponent();

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                trackAlt.Value = aftSetAdv.trackAltAdv.Value;
                lblAltDisplay.Text = trackAlt.Value.ToString();
            }
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
            ShowAdvSettings(true);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackAlt, trackAlt.Value.ToString());
            lblAltDisplay.Text = trackAlt.Value.ToString();

        }
    }
}
