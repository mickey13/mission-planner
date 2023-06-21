using System;
using System.Windows.Forms;

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

            AFTVehicleConnecting connectingVehicle = new AFTVehicleConnecting();
            connectingVehicle.Show();
            connectingVehicle.BringToFront();
        }
    }
}
