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
    public partial class AFTSettingsAdv : Form
    {
        public AFTSettingsAdv()
        {
            InitializeComponent();
        }

        private void AFTSettingsAdv_Load(object sender, EventArgs e)
        {
            
        }

        private void btnNumFlights_Click(object sender, EventArgs e)
        {
            if ((aftSetBat == null) || (aftSetBat.IsDisposed))
            {
                aftSetBat = new AFTSettingsBat();
            }

            ToggleSelection(btnNumFlights);
            ToggleSelection(btnNumFlights, aftSetBat);
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            if ((aftSetGrid == null) || aftSetGrid.IsDisposed)
            {
                aftSetGrid = new AFTSettingsGrid();
            }

            ToggleSelection(btnSegment);
            ToggleSelection(btnSegment, aftSetGrid);
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
    }
}
