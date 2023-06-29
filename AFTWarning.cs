using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTWarning : Form
    {
        public AFTWarning()
        {
            InitializeComponent();

            // Apply drop shadow effect
            (new MissionPlanner.DropShadow()).ApplyShadows(this);
        }

        private void confirmationButton_Click(object sender, EventArgs e)
        {
            // Close form and free up memory
            this.Dispose();
            warning = null;

            // Instantiate loading screen
            aftLoad = new AFTLoadingScreen();
            aftLoad.MdiParent = aftMain.MdiParent;
            SyncColorsAndInitialize(new List<Form> { aftLoad }, aftMain, aftMain.MdiParent);
            aftLoad.Show();

            // Instantiate custom form and free up memory
            InitializeForm(custom, aftMain.MdiParent);
            custom.Show();
            aftLoad.Close();
            aftLoad = null;
        }
    }
}
