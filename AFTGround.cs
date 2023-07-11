using System;
using System.Drawing;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {
        public AFTGround()
        {
            InitializeComponent();

            // Send menu panel to correct starting location
            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();

            // Send compass button to correct starting location
            btnFlightLines.Location = new Point(12, 654);
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            // Show menu panel
            if (this.Controls.GetChildIndex(sideMenuPanel) == 0)
            {
                sideMenuPanel.Dock = DockStyle.None;
                sideMenuPanel.SendToBack();
            }
            // Hide menu panel
            else
            {
                sideMenuPanel.Dock = DockStyle.Left;
                this.Controls.SetChildIndex(sideMenuPanel, 0);
            }
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            // Instantiate and show new mission screen
            aftNewMission = new AFTNewMission();
            aftNewMission.Show();
            aftNewMission.BringToFront();
        }

        private void btnPreFlightCheck_Click(object sender, EventArgs e)
        {
            // If checklist hasn't been instantiated yet, instantiate it
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }

            // Show checklist
            checklist.Show();
            checklist.BringToFront();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowAdvSettings(false);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            if ((aftReturnHome == null) || aftReturnHome.IsDisposed)
            {
                aftReturnHome = new AFTReturnHome();
            }
            aftReturnHome.Show();
            aftReturnHome.BringToFront();
        }

        private void btnFly_Click(object sender, EventArgs e)
        {
            AFTVehiclePowerUp powerUp = new AFTVehiclePowerUp();
            powerUp.Show();
            powerUp.BringToFront();
        }

        private void btnCreateMission_Click(object sender, EventArgs e)
        {
            aftNewMission = new AFTNewMission();
            aftNewMission.Show();
            aftNewMission.BringToFront();
        }

        private void btnFlightLines_Click(object sender, EventArgs e)
        {
            /*Sow a low res bmap with flight lines showing the quickest safe route home*/
        }

        private void btnVidDownlink_Click(object sender, EventArgs e)
        {
            /*Switch to video downlink*/
        }
    }
}
