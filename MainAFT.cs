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
using static MissionPlanner.Swarm.Sequence.LayoutEditor;

namespace MissionPlanner
{
    public partial class MainAFT : Form
    {
        // Pictures and colors for color modes
        public Bitmap aftLogoLight = MissionPlanner.Properties.Resources.AFT_logo_black;
        public Bitmap aftLogoDark = MissionPlanner.Properties.Resources.AFT_logo_white;
        public Bitmap togPicLight = MissionPlanner.Properties.Resources.tog_img_for_light_mode;
        public Bitmap togPicDark = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;
        public Bitmap lineLight = MissionPlanner.Properties.Resources.line_black;
        public Bitmap lineDark = MissionPlanner.Properties.Resources.line_white;

        public Color lightColor = System.Drawing.SystemColors.Control;
        public Color darkColor = System.Drawing.SystemColors.ControlText;

        // Declaring lower-level forms
        private static AFTGround aftGround = null;
        private static AFTAir aftAir = null;

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

        // Toggle a form between light and dark mode
        public bool ToggleColorMode(Form form)
        {
            if (form.BackColor == lightColor) // If in light mode
            {
                // Toggle background color
                form.BackColor = darkColor;

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
                // Toggle background color
                form.BackColor = lightColor;

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

        // Sync a lower-level form's color mode with MainAFT
        private void SyncColorModes(Form form)
        {
            form.BackColor = this.BackColor;
            foreach (Control c in form.Controls)
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
        }

        private void InstantiateForm(Form form)
        {
            if (form == aftGround & (aftGround == null || aftGround.IsDisposed))
            {
                aftGround = new AFTGround();
            }
            else if (form == aftAir & (aftAir == null || aftAir.IsDisposed))
            {
                aftAir = new AFTAir();
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
            // Sync lower-level form color mode with MainAFT
            InstantiateForm(aftGround);
            SyncColorModes(aftGround);
            aftGround.groundToggleButton.Image = toggleButton.Image;

            InstantiateForm(aftAir);
            SyncColorModes(aftAir);
            aftAir.airToggleButton.Image = toggleButton.Image;
        }

        private void groundButton_Click(object sender, EventArgs e)
        {
            InstantiateForm(aftGround);
            SyncColorModes(aftGround);
            aftGround.groundToggleButton.Image = toggleButton.Image;

            aftGround.Show();
        }

        private void airButton_Click(object sender, EventArgs e)
        {
            InstantiateForm(aftAir);
            SyncColorModes(aftAir);
            aftAir.airToggleButton.Image = toggleButton.Image;

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
