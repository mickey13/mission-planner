using System;
using System.IO;
using System.Text.Json;
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
            // Instantiate, but hide, advanced settings screen
            ShowAdvSettings(true, false);

            // Save mission settings
            var missionSettings = new MissionSettings
            {
                Altitude = new MissionSettings.AltitudeSettings
                {
                    Altitude = aftSetAdv.trackAltAdv.Value
                },
                Orientation = new MissionSettings.OrientationSettings
                {
                    Angle = aftSetAdv.trackOriAdv.Value
                },
                Speed = new MissionSettings.SpeedSettings
                {
                    Speed = aftSetAdv.trackSpeedAdv.Value
                },
                Battery = new MissionSettings.BatterySettings
                {
                    ChooseNumFlightsForMe = IsSelected(aftSetAdv.btnNumFlightsAdv)
                },
                Grid = new MissionSettings.GridSettings
                {
                    Segmented = IsSelected(aftSetAdv.btnSegmentAdv)
                },
                MissionBoundary = new MissionSettings.MissionBoundarySettings
                {
                    MissionBoundary = missionBounds
                }
            };

            // Options that edit how json file is saved
            var saveOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            // Convert mission settings to json
            string jsonsettings = JsonSerializer.Serialize(missionSettings, saveOptions);

            // Get path to user's Downloads folder
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(downloadsFolder, "Downloads", textBox1.Text + ".json");

            // Create and save file
            try
            {
                File.WriteAllText(filePath, jsonsettings);
                Console.WriteLine("Mission settings have been saved to the Downloads folder.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving to Downloads folder: {ex.Message}");
            }

            // Close all settings & free up memory
            if (!((aftSetCam == null) || aftSetCam.IsDisposed))
            {
                //aftSetCam.Close();
                //aftSetCam = null;
                aftSetCam.Hide();
            }

            if (!((aftSetAlt == null) || aftSetAlt.IsDisposed))
            {
                //aftSetAlt.Close();
                //aftSetAlt = null;
                aftSetAlt.Hide();
            }

            if (!((aftSetOri == null) || aftSetOri.IsDisposed))
            {
                //aftSetOri.Close();
                //aftSetOri = null;
                aftSetOri.Hide();
            }

            if (!((aftSetSpeed == null) || aftSetSpeed.IsDisposed))
            {
                //aftSetSpeed.Close();
                //aftSetSpeed = null;
                aftSetSpeed.Hide();
            }

            if (!((aftSetBat == null) || aftSetBat.IsDisposed))
            {
                //aftSetBat.Close();
                //aftSetBat = null;
                aftSetBat.Hide();
            }

            if (!((aftSetGrid == null) || aftSetGrid.IsDisposed))
            {
                //aftSetGrid.Close();
                //aftSetGrid = null;
                aftSetGrid.Hide();
            }

            if (!((aftSetAdv == null) || aftSetAdv.IsDisposed))
            {
                // Dispose of advanced settings because it syncs whenever it is created
                aftSetAdv.Close();
                aftSetAdv = null;

            }

            if (!((aftSaveMission == null) || aftSaveMission.IsDisposed))
            {
                aftSaveMission.Close();
                aftSaveMission = null;
            }

            // Close and free up memory
            this.Close();
            aftSaveMissionAs = null;
        }
    }
}
