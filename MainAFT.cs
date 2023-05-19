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

        public bool ToggleColorMode(List<Button> buttons = null, List<Label> labels = null, List<PictureBox> pictureBoxes = null)
        {
            // If in light mode
            if (BackColor == System.Drawing.SystemColors.Control)
            {
                // Toggle background color
                BackColor = System.Drawing.SystemColors.ControlText;

                // If toggling buttons
                if (buttons != null)
                {
                    foreach (var button in buttons)
                    {
                        if (button != null)
                        {
                            button.BackColor = System.Drawing.SystemColors.ControlText;
                            button.ForeColor = System.Drawing.SystemColors.Control;
                        }
                    }
                }
                // If toggling labels
                if (labels != null)
                {
                    foreach (var label in labels)
                    {
                        if (label != null)
                        {
                            label.BackColor = System.Drawing.SystemColors.ControlText;
                            label.ForeColor = System.Drawing.SystemColors.Control;
                        }
                    }
                }
                // If toggling pictures
                if (pictureBoxes != null)
                {
                    foreach (var pictureBox in pictureBoxes)
                    {
                        if (pictureBox != null)
                        {
                            pictureBox.BackColor = System.Drawing.SystemColors.ControlText;
                        }
                    }
                }
                return true;
            }
            // If in dark mode
            else
            {
                // Toggle background color
                BackColor = System.Drawing.SystemColors.Control;

                // If toggling buttons
                if (buttons != null)
                {
                    foreach (var button in buttons)
                    {
                        if (button != null)
                        {
                            button.BackColor = System.Drawing.SystemColors.Control;
                            button.ForeColor = System.Drawing.SystemColors.ControlText;
                        }
                    }
                }
                // If toggling labels
                if (labels != null)
                {
                    foreach (var label in labels)
                    {
                        if (label != null)
                        {
                            label.BackColor = System.Drawing.SystemColors.Control;
                            label.ForeColor = System.Drawing.SystemColors.ControlText;
                        }
                    }
                }
                // If toggling pictures
                if (pictureBoxes != null)
                {
                    foreach (var pictureBox in pictureBoxes)
                    {
                        if (pictureBox != null)
                        {
                            pictureBox.BackColor = System.Drawing.SystemColors.Control;
                        }
                    }
                }
                return false;
            }
        }

        private void toggleButton_Click(object sender, EventArgs e)
        {
            // Lists holding all components of MainAFT form
            List<Button> buttonList = new List<Button> { groundButton, airButton, customButton, toggleButton };
            List<Label> labelList = new List<Label> { groundLabel, airLabel, customLabel, label1, label2 };
            List<PictureBox> pictureBoxList = new List<PictureBox> { pictureBox1, line1, line2 };

            // Toggle between light and dark mode
            if (ToggleColorMode(buttonList, labelList, pictureBoxList))
            {
                toggleButton.Image = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;
                pictureBox1.Image = MissionPlanner.Properties.Resources.AFT_logo_white;
                line1.Image = MissionPlanner.Properties.Resources.line_white;
                line2.Image = MissionPlanner.Properties.Resources.line_white;
            }
            // If in dark mode
            else
            {
                toggleButton.Image = MissionPlanner.Properties.Resources.tog_img_for_light_mode;
                pictureBox1.Image = MissionPlanner.Properties.Resources.AFT_logo_black;
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
