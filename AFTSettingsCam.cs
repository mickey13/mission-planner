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
using static MissionPlanner.AFTMDIContainer;

namespace MissionPlanner
{
    public partial class AFTSettingsCam : Form
    {
        // Declaring forms (different "more options" for each form are the same form type for placeholders)
        public static AFTSettingsAlt aftSetAlt = null;
        public static AFTSettingsOri aftSetOri = null;
        public static AFTSettingsSpeed aftSetSpeed = null;
        public static AFTSettingsBat aftSetBat = null;
        public static AFTSettingsGrid aftSetGrid = null;

        public static AFTSettingsMore aftSetMoreCam = null;
        public static AFTSettingsMore aftSetMoreAlt = null;
        public static AFTSettingsMore aftSetMoreOri = null;
        public static AFTSettingsMore aftSetMoreSpeed = null;
        public static AFTSettingsMore aftSetMoreBat = null;
        public static AFTSettingsMore aftSetMoreGrid = null;

        public static AFTSaveMission aftSaveMission = null;
        public static AFTSaveMissionAs aftSaveMissionAs = null;

        public AFTSettingsCam()
        {
            InitializeComponent();
        }

        public static void ShowCamSettings()
        {
            if ((aftSetCam == null) || aftSetCam.IsDisposed)
            {
                aftSetCam = new AFTSettingsCam();
            }
            aftSetCam.Show();
            aftSetCam.BringToFront();
        }

        public static void ShowAltSettings()
        {
            if ((aftSetAlt == null) || aftSetAlt.IsDisposed)
            {
                aftSetAlt = new AFTSettingsAlt();
            }
            aftSetAlt.Show();
            aftSetAlt.BringToFront();
        }

        public static void ShowOriSettings()
        {
            if ((aftSetOri == null) || aftSetOri.IsDisposed)
            {
                aftSetOri = new AFTSettingsOri();
            }
            aftSetOri.Show();
            aftSetOri.BringToFront();
        }

        public static void ShowSpeedSettings()
        {
            if ((aftSetSpeed == null) || aftSetSpeed.IsDisposed)
            {
                aftSetSpeed = new AFTSettingsSpeed();
            }
            aftSetSpeed.Show();
            aftSetSpeed.BringToFront();
        }

        public static void ShowBatSettings()
        {
            if ((aftSetBat == null) || aftSetBat.IsDisposed)
            {
                aftSetBat = new AFTSettingsBat();
            }
            aftSetBat.Show();
            aftSetBat.BringToFront();
        }

        public static void ShowGridSettings()
        {
            if ((aftSetGrid == null) || aftSetGrid.IsDisposed)
            {
                aftSetGrid = new AFTSettingsGrid();
            }
            aftSetGrid.Show();
            aftSetGrid.BringToFront();
        }

        public static void ShowAdvSettings()
        {
            if ((aftSetAdv == null) || aftSetAdv.IsDisposed)
            {
                aftSetAdv = new AFTSettingsAdv();
            }

            // Show the user the save mission button
            aftSetAdv.btnSave.Text = "SAVE MISSION";
            aftSetAdv.btnSave.Size = new Size (166, 28);
            aftSetAdv.btnSave.Location = new Point(948, 881);
            aftSetAdv.btnSave.BringToFront();

            // Hide the close menu button
            aftSetAdv.btnClose.Text = "";
            aftSetAdv.btnClose.Location = new Point (954, 881);
            aftSetAdv.btnClose.SendToBack();

            aftSetAdv.Show();
            aftSetAdv.BringToFront();
        }

        public static void ShowSaveScreen()
        {
            if ((aftSaveMission == null) || aftSaveMission.IsDisposed)
            {
                aftSaveMission = new AFTSaveMission();
            }
            aftSaveMission.Show();
            aftSaveMission.BringToFront();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
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
            ShowAdvSettings();
        }
    }
}
