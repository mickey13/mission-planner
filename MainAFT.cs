using System;
using System.Drawing;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class MainAFT : Form
    {
        public MainAFT()
        {
            InitializeComponent();
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

                toggleButton.BackColor = darkColor;
                groundButton.BackColor = darkColor;
                airButton.BackColor = darkColor;
                customButton.BackColor = darkColor;
            }
            else // If in dark mode
            {
                toggleButton.Image = togPicLight;
                pictureBox1.Image = aftLogoLight;
                line1.Image = lineLight;
                line2.Image = lineLight;

                toggleButton.BackColor = lightColor;
                groundButton.BackColor = lightColor;
                airButton.BackColor = lightColor;
                customButton.BackColor = lightColor;
            }

            // The below throws an exception. It should fix itself when updating the code to the current version
            //SyncColorsAndInitialize(new List<Form> { aftAir }, this, this.MdiParent);
            //aftAir.airToggleButton.Image = toggleButton.Image;
        }

        private void groundButton_Click(object sender, EventArgs e)
        {
            // Initialize and show ground screen
            aftGround = new AFTGround();
            aftGround.MdiParent = this.MdiParent;
            aftGround.Show();
        }

        private void airButton_Click(object sender, EventArgs e)
        {
            // Initialize and show air screen
            aftAir = new AFTAir();
            aftAir.MdiParent = this.MdiParent;
            aftAir.Show();
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            // Initialize, sync colors, and show warning screen
            warning = new AFTWarning();
            SyncColors(warning, this);
            warning.label1.ForeColor = Color.Red;
            warning.ShowDialog();
        }
    }
}
