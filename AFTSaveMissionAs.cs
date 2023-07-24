using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTSaveMissionAs : Form
    {
        public AFTSaveMissionAs()
        {
            InitializeComponent();
        }

        private void btnSaveMission_Click(object sender, EventArgs e)
        {
            // *** Save flight settings ***
            /*// Create file with user-inputted name (only works on Windows machines)
            using System.IO;
            using System.Text.Json;
            using System.Text.Json.Serialization;
            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                Id = 1,
                SSN = 2,
                Message = "A Message"
            });

            string settingsToSave = JsonSerializer.Serialize(_data);
            File.WriteAllText(@"C:\path" + textBox1.Text + ".json", settingsToSave);*/

            // Close all settings & free up memory
            if (!((aftSetCam == null) || aftSetCam.IsDisposed))
            {
                aftSetCam.Close();
                aftSetCam = null;
            }

            if (!((aftSetAlt == null) || aftSetAlt.IsDisposed))
            {
                aftSetAlt.Close();
                aftSetAlt = null;
            }

            if (!((aftSetOri == null) || aftSetOri.IsDisposed))
            {
                aftSetOri.Close();
                aftSetOri = null;
            }

            if (!((aftSetSpeed == null) || aftSetSpeed.IsDisposed))
            {
                aftSetSpeed.Close();
                aftSetSpeed = null;
            }

            if (!((aftSetBat == null) || aftSetBat.IsDisposed))
            {
                aftSetBat.Close();
                aftSetBat = null;
            }

            if (!((aftSetGrid == null) || aftSetGrid.IsDisposed))
            {
                aftSetGrid.Close();
                aftSetGrid = null;
            }

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                aftSetAdv.Close();
                aftSetAdv = null;
            }

            if (!((aftSaveMission == null) || aftSaveMission.IsDisposed))
            {
                aftSaveMission.Close();
                aftSaveMission = null;
            }

            this.Close();
        }
    }
}
