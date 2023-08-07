using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTSettingsBat : Form
    {
        public AFTSettingsBat()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowSpeedSettings();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowGridSettings();
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

        private void btnGrid_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            ShowGridSettings();
        }

        private void btnNumFlights_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnNumFlights);

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                ToggleSelection(aftSetAdv.btnNumFlightsAdv);
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            ShowAdvSettings(true, true);
        }
    }
}
