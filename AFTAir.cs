using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTMDIContainer;
using static MissionPlanner.MainAFT;

namespace MissionPlanner
{
    public partial class AFTAir : Form
    {
        public AFTAir()
        {
            InitializeComponent();
        }

        private void AFTAir_Load(object sender, EventArgs e)
        {

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
            MainAFT.SyncColorsAndInitialize(new List<Form> { aftMain, aftGround }, this, aftMain.MdiParent);
            aftMain.toggleButton.Image = airToggleButton.Image;
            aftGround.groundToggleButton.Image = airToggleButton.Image;
        }
    }
}
