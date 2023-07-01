using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTMDIContainer : Form
    {
        public AFTMDIContainer()
        {
            InitializeComponent();
        }

        private void AFTMDIContainer_Load(object sender, EventArgs e)
        {
            // Initialize and show main screen
            aftMain = new MainAFT();
            aftMain.MdiParent = this;
            aftMain.Show();
        }
    }
}
