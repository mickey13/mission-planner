using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTSaveMission : Form
    {
        public AFTSaveMission()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowGridSettings();
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

        private void btnGrid_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowGridSettings();
        }

        private void btnSaveMission_Click(object sender, EventArgs e)
        {
            if ((aftSaveMissionAs == null) || aftSaveMissionAs.IsDisposed)
            {
                aftSaveMissionAs = new AFTSaveMissionAs();
            }
            aftSaveMissionAs.Show();
            aftSaveMissionAs.BringToFront();
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings(true, true);
        }
    }
}
