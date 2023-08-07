using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTSettingsCam : Form
    {
        public AFTSettingsCam()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowAltSettings();
        }

        private void btnAltitude_Click(object sender, EventArgs e)
        {
            ShowAltSettings();
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
            ShowAdvSettings(true, true);
        }
    }
}
