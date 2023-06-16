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

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            AFTNewMission aftNewMission = new AFTNewMission();
            aftNewMission.ShowDialog();
        }
    }
}
