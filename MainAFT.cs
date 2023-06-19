﻿using BrightIdeasSoftware;
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
using static MissionPlanner.Swarm.Sequence.LayoutEditor;
using static MissionPlanner.AFTMDIContainer;
using Accord.Statistics.Kernels;
using netDxf;
using System.Drawing.Imaging;

namespace MissionPlanner
{
    public partial class MainAFT : Form
    {
        public MainAFT()
        {
            InitializeComponent();
        }

        private void MainAFT_Load(object sender, EventArgs e)
        {

        }

        // Toggle a form between light and dark mode
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

        // Sync color modes across forms
        public static void SyncColorsAndInitialize(List<Form> formsToSync, Form formToSyncWith, Form parent)
        {
            if (formsToSync != null && formToSyncWith != null)
            {
                foreach (Form form in formsToSync)
                {
                    // If form is null or disposed, re-initialize it
                    Form newForm = AFTMDIContainer.InitializeForm(form, parent);

                    if (newForm != null)
                    {
                        newForm.BackColor = formToSyncWith.BackColor;
                        foreach (Control c in newForm.Controls)
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

                toggleButton .BackColor = lightColor;
                groundButton .BackColor = lightColor;
                airButton .BackColor = lightColor;
                customButton.BackColor = lightColor;
            }

            SyncColorsAndInitialize(new List<Form> { aftAir }, this, this.MdiParent);
            aftAir.airToggleButton.Image = toggleButton.Image;
        }

        private void groundButton_Click(object sender, EventArgs e)
        {
            InitializeForm(aftGround, this.MdiParent);
            aftGround.Show();
        }

        private void airButton_Click(object sender, EventArgs e)
        {
            SyncColorsAndInitialize(new List<Form> { aftAir }, this, this.MdiParent);
            aftAir.airToggleButton.Image = toggleButton.Image;

            aftAir.Show();
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            AFTWarning warning = new AFTWarning();
            SyncColorsAndInitialize(new List<Form> { warning }, this, this.MdiParent);
            warning.label1.ForeColor = Color.Red;

            warning.ShowDialog();
        }
    }
}
