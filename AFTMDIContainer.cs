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

            // Adding an invisible menustrip to the parent container hides all menu bars on the child forms.
            // But, child forms need to have WindowState = Maximized
            menuStrip1.Visible = false;
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
