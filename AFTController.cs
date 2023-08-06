using Microsoft.Maps.MapControl.WPF;
using MissionPlanner.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace MissionPlanner
{
    internal class AFTController
    {
        #region Things that require user input or that may change based on the user

        // Bing Maps API key
        public static string bingMapsKey = "YOUR_MAPS_KEY";

        // Starting map location and zoom level
        public static Location locationStart = new Location(37.19912, -80.40813);
        public static int zoomStart = 16;

        #endregion

        #region Form declarations

        // Main forms
        //public static AFTMDIContainer aftContainer = null;
        public static MainAFT aftMain = null;
        public static AFTGround aftGround = null;
        public static AFTAir aftAir = null;
        public static MainV2 custom = null;

        // Sub-forms
        public static AFTNewMission aftNewMission = new AFTNewMission(); // Initialize early so that polygon edit event works
        public static AFTChecklist checklist = null;
        public static AFTSettingsAdv aftSetAdv = null;
        public static AFTReturnHome aftReturnHome = null;

        // custom sub-forms
        public static AFTWarning warning = null;
        public static AFTLoadingScreen aftLoad = null;

        // Settings forms (aftSetMore___ forms use same class type as placeholder)
        public static AFTSettingsCam aftSetCam = null;
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

        // Vehicle power up/connecting forms
        public static AFTVehiclePowerUp powerUp = null;
        public static AFTVehicleConnecting connectingVehicle = null;

        #endregion

        #region Constants for light/dark modes

        // Pictures and colors for color modes
        public static Bitmap aftLogoLight = MissionPlanner.Properties.Resources.AFT_logo_black;
        public static Bitmap aftLogoDark = MissionPlanner.Properties.Resources.AFT_logo_white;
        public static Bitmap togPicLight = MissionPlanner.Properties.Resources.tog_img_for_light_mode;
        public static Bitmap togPicDark = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;
        public static Bitmap lineLight = MissionPlanner.Properties.Resources.line_black;
        public static Bitmap lineDark = MissionPlanner.Properties.Resources.line_white;

        public static System.Drawing.Color lightColor = System.Drawing.SystemColors.Control;
        public static System.Drawing.Color darkColor = System.Drawing.SystemColors.ControlText;

        #endregion

        #region Constants and variables for form interactions

        // Pictures for selection buttons
        public static Bitmap emptyButton = MissionPlanner.Properties.Resources.circle_hollow;
        public static Bitmap filledButton = MissionPlanner.Properties.Resources.circle_selected;

        // Picture and variable for checkboxes
        public static Bitmap boxChecked = MissionPlanner.Properties.Resources.checkbox_checkmark;
        public static bool checklistConfirmed = false;

        #endregion

        #region Constants and objects for maps and mission boundary feature

        // Mission boundary color
        public static System.Windows.Media.Color missionBoundaryColor = System.Windows.Media.Colors.DeepSkyBlue;

        // User defined polygon to add to the map
        public static MapPolygon newPolygon = null;

        // Map layer containing the polygon points defined by the user
        public static MapLayer polygonPointLayer = null;

        // Create list to hold pushpins
        public static List<Pushpin> pushPinList { get; set; }

        // Pushpin that is currently selected
        public static Pushpin SelectedPushpin { get; set; }

        #endregion

        #region Constants and objects for mission settings

        // Mission boundary
        public static LocationCollection missionBounds = null;

        // Mission settings loaded from file
        public static MissionSettings missionSettings = null;

        #endregion

        #region Constants, variables, and objects for MAVLink and peripherals

        public static string inputtedPortName = "preset";
        public static string inputtedBaud = "57600";

        /// <summary>
        /// passive comports
        /// </summary>
        public static List<MAVLinkInterface> Comports = new List<MAVLinkInterface>();

        /// <summary>
        /// store the time we first connect
        /// </summary>
        public static DateTime connecttime = DateTime.Now;

        public static DateTime nodatawarning = DateTime.Now;

        /// <summary>
        /// track the last heartbeat sent
        /// </summary>
        public static DateTime heatbeatSend = DateTime.Now;

        public static System.Threading.Timer telemetryUpdateTimer;

        /// <summary>
        /// controls the main serial reader thread
        /// </summary>
        public static bool serialThread = false;

        public static bool pluginthreadrun = false;

        public static Thread pluginthread;

        public static ManualResetEvent SerialThreadrunner = new ManualResetEvent(false);

        /// <summary>
        /// speech engine enable
        /// </summary>
        public static bool speechEnable
        {
            get { return speechEngine == null ? false : speechEngine.speechEnable; }
            set
            {
                if (speechEngine != null) speechEngine.speechEnable = value;
            }
        }

        public static bool speech_armed_only = false;

        static MAVLinkInterface _comPort = new MAVLinkInterface();

        public static ManualResetEvent PluginThreadrunner = new ManualResetEvent(false);

        #endregion

        #region Classes

        /// <summary>
        /// File format for saving mission settings
        /// </summary>
        public class MissionSettings
        {
            public AltitudeSettings AltitudeSet { get; set; }
            public OrientationSettings OrientationSet { get; set; }
            public SpeedSettings SpeedSet { get; set; }
            public BatterySettings BatterySet { get; set; }
            public GridSettings GridSet { get; set; }
            public MissionBoundarySettings MissionBoundarySet { get; set; }

            public class AltitudeSettings
            {
                public int Altitude { get; set; } = 0;
            }

            public class OrientationSettings
            {
                public bool FixedDirection { get; set; } = false;
                public bool TargetPtDirection { get; set; } = false;
                public bool DroneFacingDirection { get; set; } = false;
                public int Angle { get; set; } = 0;

                public OrientationSettings()
                {
                    // Assign values if orientation settings screen has been created
                    if (aftSetOri != null)
                    {
                        FixedDirection = IsSelected(aftSetOri.btnFxdDir);
                        TargetPtDirection = IsSelected(aftSetOri.btnTgtPtOri);
                        DroneFacingDirection = IsSelected(aftSetOri.btnSglDir);
                    }
                }
            }

            public class SpeedSettings
            {
                public int Speed { get; set; } = 10;
            }

            public class BatterySettings
            {
                public bool ChooseNumFlightsForMe { get; set; } = false;
            }

            public class GridSettings
            {
                public bool Segmented { get; set; } = false;
            }
            public class MissionBoundarySettings
            {
                public LocationCollection MissionBoundary { get; set; } = null;
            }
        }

        #endregion

        #region Classes for MAVLink and peripherals
        public static MAVLinkInterface comPort
        {
            get { return _comPort; }
            set
            {
                if (_comPort == value)
                    return;
                _comPort = value;
                if (aftGround == null)
                    return;
                _comPort.MavChanged -= aftGround.comPort_MavChanged;
                _comPort.MavChanged += aftGround.comPort_MavChanged;
                aftGround.comPort_MavChanged(null, null);
            }
        }

        public static bool speechEnabled()
        {
            if (!speechEnable)
            {
                return false;
            }
            if (speech_armed_only)
            {
                return AFTController.comPort.MAV.cs.armed;
            }
            return true;
        }

        /// <summary>
        /// spech engine static class
        /// </summary>
        public static ISpeech speechEngine { get; set; }

        #endregion

        #region Functions that instantiate & show forms

        /// <summary>
        /// Instantiate and show camera settings
        /// </summary>
        public static void ShowCamSettings()
        {
            if ((aftSetCam == null) || aftSetCam.IsDisposed)
            {
                aftSetCam = new AFTSettingsCam();
            }

            aftSetCam.Show();
            aftSetCam.BringToFront();
        }

        /// <summary>
        /// Instantiate and show altitude settings
        /// </summary>
        public static void ShowAltSettings()
        {
            if ((aftSetAlt == null) || aftSetAlt.IsDisposed)
            {
                aftSetAlt = new AFTSettingsAlt();
            }

            aftSetAlt.Show();
            aftSetAlt.BringToFront();
        }

        /// <summary>
        /// Instantiate and show orientation settings
        /// </summary>
        public static void ShowOriSettings()
        {
            if ((aftSetOri == null) || aftSetOri.IsDisposed)
            {
                aftSetOri = new AFTSettingsOri();
            }

            aftSetOri.Show();
            aftSetOri.BringToFront();
        }

        /// <summary>
        /// Instantiate and show speed settings
        /// </summary>
        public static void ShowSpeedSettings()
        {
            if ((aftSetSpeed == null) || aftSetSpeed.IsDisposed)
            {
                aftSetSpeed = new AFTSettingsSpeed();
            }

            aftSetSpeed.Show();
            aftSetSpeed.BringToFront();
        }

        /// <summary>
        /// Instantiate and show battery settings
        /// </summary>
        public static void ShowBatSettings()
        {
            if ((aftSetBat == null) || aftSetBat.IsDisposed)
            {
                aftSetBat = new AFTSettingsBat();
            }

            aftSetBat.Show();
            aftSetBat.BringToFront();
        }

        /// <summary>
        /// Instantiate and show grid settings
        /// </summary>
        public static void ShowGridSettings()
        {
            if ((aftSetGrid == null) || aftSetGrid.IsDisposed)
            {
                aftSetGrid = new AFTSettingsGrid();
            }

            aftSetGrid.Show();
            aftSetGrid.BringToFront();
        }

        /// <summary>
        /// Instantiate and show advanced settings
        /// </summary>
        /// <param name="saveMission"></Set to true if calling from settings window, false if calling from side menu panel>
        /// <param name="showForm"></Set to true if showing the form immediately, false if otherwise>
        public static void ShowAdvSettings(bool saveMission, bool showForm)
        {
            if ((aftSetAdv == null) || aftSetAdv.IsDisposed)
            {
                aftSetAdv = new AFTSettingsAdv();
            }

            // Show save button
            if (saveMission)
            {
                aftSetAdv.btnSave.BringToFront();
                aftSetAdv.btnClose.SendToBack();
            }
            //Show close button
            else
            {
                aftSetAdv.btnSave.SendToBack();
                aftSetAdv.btnClose.BringToFront();
            }

            if (showForm)
            {
                aftSetAdv.BringToFront();
                aftSetAdv.Show();
            }
            else
            {
                aftSetAdv.SendToBack();
            }
        }

        /// <summary>
        /// Instantiate and show mission save screen
        /// </summary>
        public static void ShowSaveScreen()
        {
            if ((aftSaveMission == null) || aftSaveMission.IsDisposed)
            {
                aftSaveMission = new AFTSaveMission();
            }

            aftSaveMission.Show();
            aftSaveMission.BringToFront();
        }

        #endregion

        #region Functions for syncing colors

        /// <summary>
        /// Toggle a form between light and dark mode
        /// </summary>
        /// <param name="form"></Form to toggle color modes>
        /// <returns></True if going to dark mode, false if going to light>
        public static bool ToggleColorMode(Form form)
        {
            // If in light mode
            if (form.BackColor == lightColor)
            {
                // Toggle back and foreground color
                form.BackColor = darkColor;
                form.ForeColor = lightColor;

                foreach (Control c in form.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.BackColor = darkColor;
                        c.ForeColor = lightColor;
                    }
                    else
                    {
                        c.BackColor = darkColor;
                    }
                }
                return true;
            }
            // If in dark mode
            else
            {
                // Toggle back and foreground color
                form.BackColor = lightColor;
                form.ForeColor = darkColor;

                foreach (Control c in form.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.BackColor = lightColor;
                        c.ForeColor = darkColor;
                    }
                    else
                    {
                        c.BackColor = lightColor;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Sync color modes across forms
        /// </summary>
        /// <param name="formToSync"></Form to change color mode>
        /// <param name="formToSyncWith"></Form to sync colors with>
        public static void SyncColors(Form formToSync, Form formToSyncWith)
        {
            if (formToSync != null && formToSyncWith != null)
            {
                if (formToSync != null)
                {
                    formToSync.BackColor = formToSyncWith.BackColor;
                    foreach (Control c in formToSync.Controls)
                    {
                        if (c is Button || c is Label)
                        {
                            c.BackColor = formToSyncWith.BackColor;
                            c.ForeColor = formToSyncWith.ForeColor;
                        }
                        else
                        {
                            c.BackColor = formToSyncWith.BackColor;
                        }
                    }
                }
            }
        }

        #endregion

        #region General functions

        /// <summary>
        /// Initialize with a form for rounded corners
        /// </summary>
        /// <param name="nLeftRect"></X-coordinate of upper-left corner>
        /// <param name="nTopRect"></Y-coordinate of upper-left corner>
        /// <param name="nRightRect"></X-coordinate of lower-right corner>
        /// <param name="nBottomRect"></Y-coordinate of lower-right corner>
        /// <param name="nWidthEllipse"></Width of ellipse>
        /// <param name="nHeightEllipse"></Height of ellipse>
        /// <returns></Rectangular region that defines the edge of the form>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        /// <summary>
        /// Provide function for selection buttons
        /// </summary>
        /// <param name="btn"></Button to select/deselect>
        /// <param name="form"></Form that has multiple selection buttons>
        public static void ToggleSelection(Button btn, Form form = null)
        {
            // If multiple buttons on form
            if (form != null)
            {
                // List to hold all buttons in given form
                List<Button> btnList = new List<Button>();

                // If not selected
                if (btn.Image == emptyButton)
                {
                    btn.Image = filledButton;

                    // Add each control to btnList if it is a button
                    foreach (Control control in form.Controls)
                    {
                        if (control is Button)
                        {
                            btnList.Add(control as Button);
                        }
                    }

                    // Change selection status of all other selected buttons
                    foreach (Button button in btnList)
                    {
                        if ((button != btn) && (button.Image == filledButton))
                        {
                            button.Image = emptyButton;
                        }
                    }
                }
                // If selected
                else
                {
                    btn.Image = emptyButton;
                }
            }
            // If single button on form
            else
            {
                // If not selected
                if (btn.Image == emptyButton)
                {
                    btn.Image = filledButton;
                }
                // If selected
                else
                {
                    btn.Image = emptyButton;
                }
            }
        }

        /// <summary>
        /// Test if button is selected or not
        /// </summary>
        /// <param name="button"></Button to test>
        /// <returns></True if button is selected, false if button is not selected>
        public static bool IsSelected(Button button)
        {
            return button.Image == filledButton;
        }

        /// <summary>
        /// Overwrite existing mission boundary polygon with a new one
        /// </summary>
        public static void SetUpNewPolygon()
        {
            newPolygon = new MapPolygon();

            // Defines the polygon fill details
            newPolygon.Locations = new LocationCollection();
            newPolygon.Stroke = new SolidColorBrush(missionBoundaryColor);
            newPolygon.StrokeThickness = 4;
            newPolygon.Opacity = 0.8;
        }

        #endregion

        #region Functions for MAVLink and peripherals

        public static void doDisconnect(MAVLinkInterface comPort)
        {
            try
            {
                if (speechEngine != null) // cancel all pending speech
                    speechEngine.SpeakAsyncCancelAll();

                comPort.BaseStream.DtrEnable = false;
                comPort.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    try
                    {
                        MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                );
            }
            catch
            {
            }
        }

        public static void Connect()
        {
            comPort.giveComport = false;

            Console.WriteLine("Starting to connect");

            // Sanity check
            if (comPort.BaseStream.IsOpen && comPort.MAV.cs.groundspeed > 4)
            {
                if ((int)DialogResult.No ==
                    CustomMessageBox.Show(Strings.Stillmoving, Strings.Disconnect, MessageBoxButtons.YesNo))
                {
                    return;
                }
            }

            // Decide if this is a connect or disconnect
            if (comPort.BaseStream.IsOpen)
            {
                doDisconnect(comPort);
            }
            else
            {
                // Decide if aftGround or aftAir
                if (aftGround != null)
                {
                    AFTGround._doConnect(comPort, inputtedPortName, inputtedBaud);
                }
                else if (aftAir != null)
                {// Enable after updating aftAir; delete custom message box
                    //AFTAir._doConnect(comPort, inputtedPortName, inputtedBaud);
                    CustomMessageBox.Show("See Connect() in AFTController.cs");
                }
            }

            if (comPort.BaseStream.IsOpen)
                loadph_serial();
        }

        public static void loadph_serial()
        {
            try
            {
                if (comPort.MAV.SerialString == "")
                    return;

                if (comPort.MAV.SerialString.Contains("CubeBlack") &&
                    !comPort.MAV.SerialString.Contains("CubeBlack+") &&
                    comPort.MAV.param.ContainsKey("INS_ACC3_ID") && comPort.MAV.param["INS_ACC3_ID"].Value == 0 &&
                    comPort.MAV.param.ContainsKey("INS_GYR3_ID") && comPort.MAV.param["INS_GYR3_ID"].Value == 0 &&
                    comPort.MAV.param.ContainsKey("INS_ENABLE_MASK") && comPort.MAV.param["INS_ENABLE_MASK"].Value >= 7)
                {
                    MissionPlanner.Controls.SB.Show("Param Scan");
                }
            }
            catch
            {
            }

            try
            {
                if (comPort.MAV.SerialString == "")
                    return;

                // brd type should be 3
                // devids show which sensor is not detected
                // baro does not list a devid

                //devop read spi lsm9ds0_ext_am 0 0 0x8f 1
                if (comPort.MAV.SerialString.Contains("CubeBlack") && !comPort.MAV.SerialString.Contains("CubeBlack+"))
                {
                    Task.Run(() =>
                    {
                        bool bad1 = false;
                        byte[] data = new byte[0];

                        comPort.device_op(comPort.MAV.sysid, comPort.MAV.compid, out data,
                            MAVLink.DEVICE_OP_BUSTYPE.SPI,
                            "lsm9ds0_ext_g", 0, 0, 0x8f, 1);
                        if (data.Length != 0 && (data[0] != 0xd4 && data[0] != 0xd7))
                            bad1 = true;

                        comPort.device_op(comPort.MAV.sysid, comPort.MAV.compid, out data,
                            MAVLink.DEVICE_OP_BUSTYPE.SPI,
                            "lsm9ds0_ext_am", 0, 0, 0x8f, 1);
                        if (data.Length != 0 && data[0] != 0x49)
                            bad1 = true;

                        if (bad1)
                            aftGround.BeginInvoke(method: (Action)delegate
                            {
                                MissionPlanner.Controls.SB.Show("SPI Scan");
                            });
                    });
                }

            }
            catch
            {
            }
        }

        public static void PluginThread()
        {
            Hashtable nextrun = new Hashtable();

            pluginthreadrun = true;

            PluginThreadrunner.Reset();

            while (pluginthreadrun)
            {
                DateTime minnextrun = DateTime.Now.AddMilliseconds(1000);
                try
                {
                    foreach (var plugin in Plugin.PluginLoader.Plugins.ToArray())
                    {
                        if (!nextrun.ContainsKey(plugin))
                            nextrun[plugin] = DateTime.MinValue;

                        if ((DateTime.Now > plugin.NextRun) && (plugin.loopratehz > 0))
                        {
                            // get ms till next run
                            int msnext = (int)(1000 / plugin.loopratehz);

                            // allow the plug to modify this, if needed
                            plugin.NextRun = DateTime.Now.AddMilliseconds(msnext);

                            if (plugin.NextRun < minnextrun)
                                minnextrun = plugin.NextRun;

                            try
                            {
                                bool ans = plugin.Loop();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                    }
                }
                catch
                {
                }

                var sleepms = (int)((minnextrun - DateTime.Now).TotalMilliseconds);
                // max rate is 100 hz - prevent massive cpu usage
                if (sleepms > 0)
                    System.Threading.Thread.Sleep(sleepms);
            }

            while (Plugin.PluginLoader.Plugins.Count > 0)
            {
                var plugin = Plugin.PluginLoader.Plugins[0];
                try
                {
                    plugin.Exit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Plugin.PluginLoader.Plugins.Remove(plugin);
            }

            try
            {
                PluginThreadrunner.Set();
            }
            catch
            {
            }

            return;
        }

        #endregion
    }
}
