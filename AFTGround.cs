using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTStateMachine;
using static MissionPlanner.AFTStateMachine.ApplicationState;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {
        MainAFT MainAFT = new MainAFT();

        public AFTGround()
        {
            InitializeComponent();
        }

        private void groundForm_Load(object sender, EventArgs e)
        {

        }

        void OpenNextForm(States next_state)
        {

            ApplicationState.nextState = next_state;

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void groundToggleButton_Click(object sender, EventArgs e)
        {
            // Toggle between light and dark mode
            if (MainAFT.ToggleColorMode(this)) // If in light mode
            {
                groundToggleButton.Image = togPicDark;

                // Toggle aftMain images
                aftMain.toggleButton.Image = togPicDark;
                aftMain.pictureBox1.Image = aftLogoDark;
                aftMain.line1.Image = lineDark;
                aftMain.line2.Image = lineDark;
            }
            else // If in dark mode
            {
                groundToggleButton.Image = togPicLight;

                // Toggle aftMain images
                aftMain.toggleButton.Image = togPicLight;
                aftMain.pictureBox1.Image = aftLogoLight;
                aftMain.line1.Image = lineLight;
                aftMain.line2.Image = lineLight;
            }

            // Sync color modes across forms
            MainAFT.SyncColorModes(aftMain, this);
            aftMain.toggleButton.Image = groundToggleButton.Image;

            MainAFT.SyncColorModes(aftAir, this);
            aftAir.airToggleButton.Image = groundToggleButton.Image;
        }

        private void groundForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenNextForm(States.AFTMAIN);
        }
    }
}
