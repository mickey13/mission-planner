﻿using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTNewMission : Form
    {
        // Declare an event to notify subscribers when the map polygon needs to be edited
        public event EventHandler<AFTGround.PolygonEventArgs> GroundPolygonEditRequested;
        public event EventHandler<AFTAir.PolygonEventArgs> AirPolygonEditRequested;

        public event EventHandler<AFTSettingsAdv.MissionSettingsEventArgs> MissionSettingsEditRequested;

        public AFTNewMission()
        {
            InitializeComponent();
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnNewMission, this);
        }

        private void btnLoadMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnLoadMission, this);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (btnNewMission.Image == filledButton)
            {
                // Show camera settings and free up memory
                this.Dispose();
                aftNewMission = null;
                ShowCamSettings();
            }
            else if (btnLoadMission.Image == filledButton)
            {
                // Instantiate advanced settings to update them with mission file
                ShowAdvSettings(false, false);

                // Show an OpenFileDialog to let the user choose a file
                var openFileDialog = new OpenFileDialog
                {
                    // Only show JSON files
                    Title = "Select a JSON file to load",
                    Filter = "JSON Files (*.json)|*.json",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Read the JSON data from the selected file
                    string jsonString = File.ReadAllText(selectedFilePath);

                    try
                    {
                        // Deserialize the JSON data back into the Settings object
                        missionSettings = JsonSerializer.Deserialize<MissionSettings>(jsonString);

                        // Now have access to deserialized data
                        Console.WriteLine($"Altitude: {missionSettings.AltitudeSet.Altitude}");
                        Console.WriteLine($"Orientation: {missionSettings.OrientationSet.FixedDirection}, " +
                            $"{missionSettings.OrientationSet.TargetPtDirection}, " +
                            $"{missionSettings.OrientationSet.DroneFacingDirection}, " +
                            $"{missionSettings.OrientationSet.Angle}");
                        Console.WriteLine($"Speed: {missionSettings.SpeedSet.Speed}");
                        Console.WriteLine($"Battery: {missionSettings.BatterySet.ChooseNumFlightsForMe}");
                        Console.WriteLine($"Grid: {missionSettings.GridSet.Segmented}");
                        Console.WriteLine($"MissionBoundary: {missionSettings.MissionBoundarySet.MissionBoundary}");
                    }
                    catch (JsonException)
                    {
                        Console.WriteLine("Error: Invalid JSON format in the selected file.");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Error: The selected file does not exist or cannot be found.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during deserialization: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File selection canceled by the user.");
                }

                // Send mission settings to aftSettingsAdv to update settings
                MissionSettingsEditRequested?.Invoke(this, new AFTSettingsAdv.MissionSettingsEventArgs(
                    missionSettings.AltitudeSet.Altitude,
                    missionSettings.OrientationSet.Angle,
                    missionSettings.SpeedSet.Speed,
                    missionSettings.BatterySet.ChooseNumFlightsForMe,
                    missionSettings.GridSet.Segmented));

                // If there is a mission boundary in the loaded file
                if (missionSettings.MissionBoundarySet.MissionBoundary != null)
                {
                    if (aftGround != null)
                    {
                        // Send mission boundary coords to aftGround to update polygon
                        GroundPolygonEditRequested?.Invoke(this, new AFTGround.PolygonEventArgs(missionSettings.MissionBoundarySet.MissionBoundary));
                    }
                    else if (aftAir != null)
                    {
                        // Send mission boundary coords to aftAir to update polygon
                        AirPolygonEditRequested?.Invoke(this, new AFTAir.PolygonEventArgs(missionSettings.MissionBoundarySet.MissionBoundary));
                    }
                }

                // Free up memory
                this.Dispose();
                aftNewMission = null;
            }
            else { }
        }
    }
}
