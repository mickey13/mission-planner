using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.Swarm.Sequence.LayoutEditor;

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

        //public bool ToggleColorMode(List<Button> buttons = null, List<Label> labels = null, List<PictureBox> pictureBoxes = null)
        public bool ToggleColorMode(Form form)
        {
            // If in light mode
            if (form.BackColor == System.Drawing.SystemColors.Control)
            {
                // Toggle background color
                form.BackColor = System.Drawing.SystemColors.ControlText;

                foreach (Control c in form.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.BackColor = System.Drawing.SystemColors.ControlText;
                        c.ForeColor = System.Drawing.SystemColors.Control;
                    }
                    else
                    {
                        c.BackColor = System.Drawing.SystemColors.ControlText;
                    }
                }
                return true;
            }
            // If in dark mode
            else
            {
                // Toggle background color
                form.BackColor = System.Drawing.SystemColors.Control;

                foreach (Control c in form.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.BackColor = System.Drawing.SystemColors.Control;
                        c.ForeColor = System.Drawing.SystemColors.ControlText;
                    }
                    else
                    {
                        c.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
                return false;
            }
        }

        private void toggleButton_Click(object sender, EventArgs e)
        {
            // Toggle between light and dark mode
            if (ToggleColorMode(this))
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

            // Syncing color mode
            aftGround.BackColor = this.BackColor;
            foreach(Control c in aftGround.Controls)
            {
                if (c is Button || c is Label)
                {
                    c.BackColor = groundButton.BackColor;
                    c.ForeColor = groundButton.ForeColor;
                }
                else
                {
                    c.BackColor = groundButton.BackColor;
                }
            }

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

        private void MainAFT_Load(object sender, EventArgs e)
        {

        }
    }
}
