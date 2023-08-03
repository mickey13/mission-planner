using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;
using static MissionPlanner.AFTGround;

namespace MissionPlanner
{
    public partial class AFTVehicleConnecting : Form
    {
        public AFTVehicleConnecting()
        {
            InitializeComponent();
        }

        private void AFTVehicleConnecting_Load(object sender, EventArgs e)
        {
            //Connect drone
            Connect();


            // Show pre-flight checklist
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }

            checklist.Show();
            checklist.BringToFront();
            this.Close();
        }
    }
}
