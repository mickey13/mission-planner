using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

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
            //this.Dispose();
            ShowBatSettings();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowSaveScreen();
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

        private void btnSegment_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnSegment);

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                ToggleSelection(aftSetAdv.btnSegmentAdv);
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings(true, true);
        }
    }
}
