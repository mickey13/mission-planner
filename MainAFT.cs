using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class MainAFT : Form
    {
        public MainAFT()
        {
            InitializeComponent();
            Program.Splash?.Refresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Program.Splash?.Close();
        }

        private void toggleButton_Click(object sender, EventArgs e)
        {
            // If in light mode
            if (BackColor == System.Drawing.SystemColors.Control)
            {
                // Toggle background color
                BackColor = System.Drawing.SystemColors.ControlText;

                // Toggle colors of AFT Ground button and label
                groundButton.BackColor = System.Drawing.SystemColors.ControlText;
                groundLabel.BackColor = System.Drawing.SystemColors.ControlText;
                groundLabel.ForeColor = System.Drawing.SystemColors.Control;

                // Toggle colors of AFT Air button and label
                airButton.BackColor = System.Drawing.SystemColors.ControlText;
                airLabel.BackColor = System.Drawing.SystemColors.ControlText;
                airLabel.ForeColor = System.Drawing.SystemColors.Control;

                // Toggle colors of Custom button and label
                customButton.BackColor = System.Drawing.SystemColors.ControlText;
                customLabel.BackColor = System.Drawing.SystemColors.ControlText;
                customLabel.ForeColor = System.Drawing.SystemColors.Control;

                // Toggle colors and image of toggleButton
                toggleButton.BackColor = System.Drawing.SystemColors.ControlText;
                toggleButton.Image = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;

                // Toggle colors and image of AFT Logo
                pictureBox1.BackColor = System.Drawing.SystemColors.ControlText;
                pictureBox1.Image = MissionPlanner.Properties.Resources.AFT_logo_white;

                // Toggle color of text and lines
                label1.BackColor = System.Drawing.SystemColors.ControlText;
                label1.ForeColor = System.Drawing.SystemColors.Control;
                label2.BackColor = System.Drawing.SystemColors.ControlText;
                label2.ForeColor = System.Drawing.SystemColors.Control;
                line1.Image = MissionPlanner.Properties.Resources.line_white;
                line2.Image = MissionPlanner.Properties.Resources.line_white;
            }
            // If in dark mode
            else
            {
                // Toggle background color
                BackColor = System.Drawing.SystemColors.Control;

                // Toggle colors of AFT Ground button and label
                groundButton.BackColor = System.Drawing.SystemColors.Control;
                groundLabel.BackColor = System.Drawing.SystemColors.Control;
                groundLabel.ForeColor = System.Drawing.SystemColors.ControlText;

                // Toggle colors of AFT Air button and label
                airButton.BackColor = System.Drawing.SystemColors.Control;
                airLabel.BackColor = System.Drawing.SystemColors.Control;
                airLabel.ForeColor = System.Drawing.SystemColors.ControlText;

                // Toggle colors of Custom button and label
                customButton.BackColor = System.Drawing.SystemColors.Control;
                customLabel.BackColor = System.Drawing.SystemColors.Control;
                customLabel.ForeColor = System.Drawing.SystemColors.ControlText;

                // Toggle colors and image of toggleButton
                toggleButton.BackColor = System.Drawing.SystemColors.Control;
                toggleButton.Image = MissionPlanner.Properties.Resources.tog_img_for_light_mode;

                // Toggle colors and image of AFT Logo
                pictureBox1.BackColor = System.Drawing.SystemColors.Control;
                pictureBox1.Image = MissionPlanner.Properties.Resources.AFT_logo_black;

                // Toggle color of text and lines
                label1.BackColor = System.Drawing.SystemColors.Control;
                label1.ForeColor = System.Drawing.SystemColors.ControlText;
                label2.BackColor = System.Drawing.SystemColors.Control;
                label2.ForeColor = System.Drawing.SystemColors.ControlText;
                line1.Image = MissionPlanner.Properties.Resources.line_black;
                line2.Image = MissionPlanner.Properties.Resources.line_black;
            }
        }

        private void groundButton_Click(object sender, EventArgs e)
        {
            AFTGround aftGround = new AFTGround();
            aftGround.Show();
        }

        private void airButton_Click(object sender, EventArgs e)
        {
            AFTAir aftAir = new AFTAir();
            aftAir.Show();
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            MainV2 mainV2 = new MainV2();
            mainV2.Show();
        }
    }
}
