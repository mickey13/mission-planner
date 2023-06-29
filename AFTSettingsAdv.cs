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
using static MissionPlanner.AFTSettingsAlt;
using static MissionPlanner.AFTSettingsOri;
using static MissionPlanner.AFTSettingsSpeed;
using static MissionPlanner.AFTSettingsBat;
using static MissionPlanner.AFTSettingsGrid;



namespace MissionPlanner
{
    public partial class AFTSettingsAdv : Form
    {
        public AFTSettingsAdv()
        {
            InitializeComponent();
        }

        private void AFTSettingsAdv_Load(object sender, EventArgs e)
        {
            //Sync with main settings if they're already open
            if (!((aftSetAlt == null) || aftSetAlt.IsDisposed))
            {
                trackAltAdv.Value = aftSetAlt.trackAlt.Value;
                lblAltDisplay.Text = trackAltAdv.Value.ToString();
            }

            if (!((aftSetSpeed == null) || aftSetSpeed.IsDisposed))
            {
                trackSpeedAdv.Value = aftSetSpeed.trackSpeed.Value;
                lblSpeedDisplay.Text = trackSpeedAdv.Value.ToString();
            }

            if (!((aftSetBat == null) || aftSetBat.IsDisposed))
            {
                btnNumFlightsAdv.Image = aftSetBat.btnNumFlights.Image;
            }

            if (!((aftSetGrid == null) || aftSetGrid.IsDisposed))
            {
                btnSegmentAdv.Image = aftSetGrid.btnSegment.Image;
            }
        }

        private void btnNumFlights_Click(object sender, EventArgs e)
        {
            if ((aftSetBat == null) || (aftSetBat.IsDisposed))
            {
                aftSetBat = new AFTSettingsBat();
            }

            ToggleSelection(btnNumFlightsAdv);
            ToggleSelection(aftSetBat.btnNumFlights);
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            if ((aftSetGrid == null) || aftSetGrid.IsDisposed)
            {
                aftSetGrid = new AFTSettingsGrid();
            }

            ToggleSelection(btnSegmentAdv);
            ToggleSelection(aftSetGrid.btnSegment);
        }

        private void btnOptn1_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreCam == null) || aftSetMoreCam.IsDisposed)
            {
                aftSetMoreCam = new AFTSettingsMore();
            }
            aftSetMoreCam.Show();
            aftSetMoreCam.BringToFront();
        }

        private void btnOptn2_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreAlt == null) || aftSetMoreAlt.IsDisposed)
            {
                aftSetMoreAlt = new AFTSettingsMore();
            }
            aftSetMoreAlt.Show();
            aftSetMoreAlt.BringToFront();
        }

        private void btnOptn3_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreOri == null) || aftSetMoreOri.IsDisposed)
            {
                aftSetMoreOri = new AFTSettingsMore();
            }
            aftSetMoreOri.Show();
            aftSetMoreOri.BringToFront();
        }

        private void btnOptn4_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreSpeed == null) || aftSetMoreSpeed.IsDisposed)
            {
                aftSetMoreSpeed = new AFTSettingsMore();
            }
            aftSetMoreSpeed.Show();
            aftSetMoreSpeed.BringToFront();
        }

        private void btnOptn5_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreBat == null) || aftSetMoreBat.IsDisposed)
            {
                aftSetMoreBat = new AFTSettingsMore();
            }
            aftSetMoreBat.Show();
            aftSetMoreBat.BringToFront();
        }

        private void btnOptn6_Click(object sender, EventArgs e)
        {
            if ((aftSetMoreGrid == null) || aftSetMoreGrid.IsDisposed)
            {
                aftSetMoreGrid = new AFTSettingsMore();
            }
            aftSetMoreGrid.Show();
            aftSetMoreGrid.BringToFront();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((aftSaveMissionAs == null) || aftSaveMissionAs.IsDisposed)
            {
                aftSaveMissionAs = new AFTSaveMissionAs();
            }
            aftSaveMissionAs.Show();
            aftSaveMissionAs.BringToFront();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void trackAltAdv_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackAltAdv, trackAltAdv.Value.ToString());
            lblAltDisplay.Text = trackAltAdv.Value.ToString();

            // Write to main altitude settings
            if (!((aftSetAlt == null) || aftSetAlt.IsDisposed))
            {
                aftSetAlt.trackAlt.Value = trackAltAdv.Value;
            }
        }

        private void trackOriAdv_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackOriAdv, trackOriAdv.Value.ToString());
            lblOriDisplay.Text = trackOriAdv.Value.ToString();
        }

        private void trackSpeedAdv_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackSpeedAdv, trackSpeedAdv.Value.ToString());
            lblSpeedDisplay.Text = trackSpeedAdv.Value.ToString();

            // Write to main speed settings
            if (!((aftSetSpeed == null) || aftSetSpeed.IsDisposed))
            {
                aftSetSpeed.trackSpeed.Value = trackSpeedAdv.Value;
            }
        }
    }
}
