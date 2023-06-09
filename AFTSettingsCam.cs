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

namespace MissionPlanner
{
    public partial class AFTSettingsCam : Form
    {
        public static AFTSettingsAlt aftSetAlt = null;
        public static AFTSettingsOri aftSetOri = null;
        public static AFTSettingsSpeed aftSetSpeed = null;
        public static AFTSettingsBat aftSetBat = null;
        public static AFTSettingsGrid aftSetGrid = null;

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
            aftSetCam.ShowDialog();
        }

        public static void ShowAltSettings()
        {
            if ((aftSetAlt == null) || aftSetAlt.IsDisposed)
            {
                aftSetAlt = new AFTSettingsAlt();
            }
            aftSetAlt.ShowDialog();
        }

        public static void ShowOriSettings()
        {
            if ((aftSetOri == null) || aftSetOri.IsDisposed)
            {
                aftSetOri = new AFTSettingsOri();
            }
            aftSetOri.ShowDialog();
        }

        public static void ShowSpeedSettings()
        {
            if ((aftSetSpeed == null) || aftSetSpeed.IsDisposed)
            {
                aftSetSpeed = new AFTSettingsSpeed();
            }
            aftSetSpeed.ShowDialog();
        }

        public static void ShowBatSettings()
        {
            if ((aftSetBat == null) || aftSetBat.IsDisposed)
            {
                aftSetBat = new AFTSettingsBat();
            }
            aftSetBat.ShowDialog();
        }

        public static void ShowGridSettings()
        {
            if ((aftSetGrid == null) || aftSetGrid.IsDisposed)
            {
                aftSetGrid = new AFTSettingsGrid();
            }
            aftSetGrid.ShowDialog();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Dispose();
            ShowAltSettings();
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
    }
}
