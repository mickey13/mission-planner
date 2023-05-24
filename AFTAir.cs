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
    public partial class AFTAir : Form
    {
        MainAFT MainAFT = new MainAFT();

        public AFTAir()
        {
            InitializeComponent();
        }

        private void AFTAir_Load(object sender, EventArgs e)
        {

        }

        void OpenNextForm(States next_state)
        {

            ApplicationState.nextState = next_state;

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void airToggleButton_Click(object sender, EventArgs e)
        {
            // Toggle between light and dark mode
            if (MainAFT.ToggleColorMode(this)) // If in light mode
            {
                airToggleButton.Image = togPicDark;

                // Toggle aftMain images
                aftMain.toggleButton.Image = togPicDark;
                aftMain.pictureBox1.Image = aftLogoDark;
                aftMain.line1.Image = lineDark;
                aftMain.line2.Image = lineDark;
            }
            else // If in dark mode
            {
                airToggleButton.Image = togPicLight;

                // Toggle aftMain images
                aftMain.toggleButton.Image = togPicLight;
                aftMain.pictureBox1.Image = aftLogoLight;
                aftMain.line1.Image = lineLight;
                aftMain.line2.Image = lineLight;
            }

            // Sync color modes across forms
            MainAFT.SyncColorModes(aftMain, this);
            aftMain.toggleButton.Image = airToggleButton.Image;

            MainAFT.SyncColorModes(aftGround, this);
            aftGround.groundToggleButton.Image = airToggleButton.Image;
        }

        private void AFTAir_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenNextForm(States.AFTMAIN);
        }
    }
}
