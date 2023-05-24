using BrightIdeasSoftware;
using IronPython.Runtime;
using org.mariuszgromada.math.mxparser.mathcollection;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.MainAFT;
using static MissionPlanner.AFTStateMachine.ApplicationState;
using static MissionPlanner.AFTStateMachine;
using static MissionPlanner.Swarm.Sequence.LayoutEditor;

namespace MissionPlanner
{
    public partial class MainAFT : Form
    {

        public MainAFT()
        {
            InitializeComponent();
            //Program.Splash?.Refresh();
        }

        private void MainAFT_Load(object sender, EventArgs e)
        {

        }
        
        void OpenNextForm(States next_state)
        {

            ApplicationState.nextState = next_state;

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        // Toggle a form between light and dark mode
        public bool ToggleColorMode(Form form)
        {
            if (form.BackColor == lightColor) // If in light mode
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
            else // If in dark mode
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

        // Sync color modes across forms
        public void SyncColorModes(Form formToSync, Form formToSyncWith)
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

        private void toggleButton_Click(object sender, EventArgs e)
        {

            // Toggle between light and dark mode
            if (ToggleColorMode(this)) // If in light mode
            {
                toggleButton.Image = togPicDark;
                pictureBox1.Image = aftLogoDark;
                line1.Image = lineDark;
                line2.Image = lineDark;
            }
            else // If in dark mode
            {
                toggleButton.Image = togPicLight;
                pictureBox1.Image = aftLogoLight;
                line1.Image = lineLight;
                line2.Image = lineLight;
            }
            
            SyncColorModes(aftGround, this);
            aftGround.groundToggleButton.Image = toggleButton.Image;

            SyncColorModes(aftAir, this);
            aftAir.airToggleButton.Image = toggleButton.Image;
        }

        private void groundButton_Click(object sender, EventArgs e)
        {
            OpenNextForm(States.AFTGROUND);
        }

        private void airButton_Click(object sender, EventArgs e)
        {
            OpenNextForm(States.AFTAIR);
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            OpenNextForm(States.CUSTOM);
        }
    }
}
