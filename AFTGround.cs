using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTMDIContainer;
using static MissionPlanner.MainAFT;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {
        public AFTGround()
        {
            InitializeComponent();

            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();
        }

        private void groundForm_Load(object sender, EventArgs e)
        {

        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            if (this.Controls.GetChildIndex(sideMenuPanel) == 0)
            {
                sideMenuPanel.Dock = DockStyle.None;
                sideMenuPanel.SendToBack();
            }
            else
            {
                sideMenuPanel.Dock = DockStyle.Left;
                this.Controls.SetChildIndex(sideMenuPanel, 0);
            }
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

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            AFTNewMission aftNewMission = new AFTNewMission();
            aftNewMission.ShowDialog();
        }

        private void btnPreFlightCheck_Click(object sender, EventArgs e)
        {
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }
            checklist.Show();
            checklist.BringToFront();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if ((aftSetAdv == null) || aftSetAdv.IsDisposed)
            {
                aftSetAdv = new AFTSettingsAdv();
            }

            // Hide the save mission button
            aftSetAdv.btnSave.Text = "";
            aftSetAdv.btnSave.Size = new Size(1, 1);
            aftSetAdv.btnSave.SendToBack();

            // Show the user the close menu button
            aftSetAdv.btnClose.Text = "CLOSE MENU";
            aftSetAdv.btnClose.Location = new Point(954, 881);
            aftSetAdv.btnClose.BringToFront();

            aftSetAdv.Show();
            aftSetAdv.BringToFront();
        }
    }
}
