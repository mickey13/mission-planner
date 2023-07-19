using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTVehiclePowerUp : Form
    {
        public AFTVehiclePowerUp()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.Close();

            connectingVehicle = new AFTVehicleConnecting();
            connectingVehicle.Show();
            connectingVehicle.BringToFront();
        }
    }
}
