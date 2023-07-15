using System;
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

            // Initialize, sync colors, and show loading screen
            aftLoad = new AFTLoadingScreen();
            aftLoad.MdiParent = aftMain.MdiParent;
            SyncColors(aftLoad, aftMain);
            aftLoad.Show();

            // Initialize and show custom form, free up memory
            custom = new MainV2();
            custom.MdiParent = aftMain.MdiParent;
            custom.Show();
            aftLoad.Close();
            aftLoad = null;
        }
    }
}
