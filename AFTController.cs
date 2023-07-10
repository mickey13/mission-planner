using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MissionPlanner
{
    internal class AFTController
    {
        #region Form declarations and constants

        // Declaring main forms
        public static MainAFT aftMain = null;
        public static AFTGround aftGround = null;
        public static AFTAir aftAir = null;
        public static MainV2 custom = null;

        // Declaring aftGround sub-forms
        public static AFTNewMission aftNewMission = null;
        public static AFTChecklist checklist = null;
        public static AFTSettingsAdv aftSetAdv = null;
        public static AFTReturnHome aftReturnHome = null;

        // Declaring custom sub-forms
        public static AFTWarning warning = null;
        public static AFTLoadingScreen aftLoad = null;

        // Declaring settings forms (aftSetMore___ use same class type as placeholder)
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

        // Pictures and colors for color modes
        public static Bitmap aftLogoLight = MissionPlanner.Properties.Resources.AFT_logo_black;
        public static Bitmap aftLogoDark = MissionPlanner.Properties.Resources.AFT_logo_white;
        public static Bitmap togPicLight = MissionPlanner.Properties.Resources.tog_img_for_light_mode;
        public static Bitmap togPicDark = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;
        public static Bitmap lineLight = MissionPlanner.Properties.Resources.line_black;
        public static Bitmap lineDark = MissionPlanner.Properties.Resources.line_white;

        public static Color lightColor = System.Drawing.SystemColors.Control;
        public static Color darkColor = System.Drawing.SystemColors.ControlText;

        // Pictures for selection buttons
        public static Bitmap emptyButton = MissionPlanner.Properties.Resources.circle_hollow;
        public static Bitmap filledButton = MissionPlanner.Properties.Resources.circle_selected;

        // Picture and variable for checkboxes
        public static Bitmap boxChecked = MissionPlanner.Properties.Resources.checkbox_checkmark;
        public static bool checklistConfirmed = false;

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

        #endregion

        #region Funtions that instantiate & show forms

        /// <summary>
        /// Instantiate and show camera settings
        /// </summary>
        public static void ShowCamSettings()
        {
            if ((aftSetCam == null) || aftSetCam.IsDisposed)
            {
                aftSetCam = new AFTSettingsCam();
            }
            // Add to open forms list
            //openSubForms.Add(aftSetCam);

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
            // Add to open forms list
            //openSubForms.Add(aftSetAlt);

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
            // Add to open forms list
            //openSubForms.Add(aftSetOri);

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
            // Add to open forms list
            //openSubForms.Add(aftSetSpeed);

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
            // Add to open forms list
            //openSubForms.Add(aftSetBat);

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
            // Add to open forms list
            //openSubForms.Add(aftSetGrid);

            aftSetGrid.Show();
            aftSetGrid.BringToFront();
        }

        /// <summary>
        /// Instantiate and show advanced settings
        /// </summary>
        /// <param name="saveMission"></Set to true if calling from settings window, false otherwise>
        public static void ShowAdvSettings(bool saveMission)
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
            // Add to open forms list
            //openSubForms.Add(aftSetAdv);

            aftSetAdv.Show();
            aftSetAdv.BringToFront();
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
            // Add to open forms list
            //openSubForms.Add(aftSaveMission);

            aftSaveMission.Show();
            aftSaveMission.BringToFront();
        }

        #endregion
    }
}
