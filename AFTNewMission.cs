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
using static MissionPlanner.AFTSettingsCam;

namespace MissionPlanner
{
    public partial class AFTNewMission : Form
    {
        public static AFTSettingsCam aftSetCam = null;

        public AFTNewMission()
        {
            InitializeComponent();
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(this, btnNewMission);
        }

        private void btnLoadMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(this, btnLoadMission);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (btnNewMission.Image == filledButton)
            {
                this.Dispose();
                //aftMain.MdiParent.Hide();
                ShowCamSettings();
            }
            else if (btnLoadMission.Image == filledButton)
            {

            }
            else { }
        }
    }
}
