using System;
using System.Drawing;
using System.Windows.Forms;
using static MissionPlanner.AFTController;



namespace MissionPlanner
{
    public partial class AFTSettingsAdv : Form
    {
        /// <summary>
        /// Custom EventArgs for updating mission settings
        /// </summary>
        public class MissionSettingsEventArgs : EventArgs
        {
            public int Altitude { get; }
            public int Angle { get; }
            public int Speed { get; }
            public bool ChooseNumFlightsForMe { get; }
            public bool Segmented { get; }

            public MissionSettingsEventArgs(int altitude, int angle, int speed, bool chooseNumFlightsForMe, bool segmented)
            {
                Altitude = altitude;
                Angle = angle;
                Speed = speed;
                ChooseNumFlightsForMe = chooseNumFlightsForMe;
                Segmented = segmented;
            }
        }

        public AFTSettingsAdv()
        {
            InitializeComponent();

            aftNewMission.MissionSettingsEditRequested += aftNewMission_MissionSettingsEditRequested;


        }

        private void AFTSettingsAdv_Load(object sender, EventArgs e)
        {
            // Send close button to correct location
            aftSetAdv.btnClose.Location = new Point(7, 22);

            // Sync with main settings if they're already open
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

        private void aftNewMission_MissionSettingsEditRequested(object sender, MissionSettingsEventArgs e)
        {
            // Handle event when it's raised in aftNewMission
            // Update mission settings from loaded file
            trackAltAdv.Value = e.Altitude;
            lblAltDisplay.Text = e.Altitude.ToString();

            trackOriAdv.Value = e.Angle;
            lblOriDisplay.Text = e.Angle.ToString();

            trackSpeedAdv.Value = e.Speed;
            lblSpeedDisplay.Text = e.Speed.ToString();

            if (e.ChooseNumFlightsForMe)
            {
                btnNumFlightsAdv.Image = filledButton;
            }

            if (e.Segmented)
            {
                btnSegmentAdv.Image = filledButton;
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
                aftSetAlt.lblAltDisplay.Text = lblAltDisplay.Text;
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
                aftSetSpeed.lblSpeedDisplay.Text = lblSpeedDisplay.Text;
            }
        }
    }
}
