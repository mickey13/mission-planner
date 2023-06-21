using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTNewMission : Form
    {
        public AFTNewMission()
        {
            InitializeComponent();
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnNewMission, this);
        }

        private void btnLoadMission_Click(object sender, EventArgs e)
        {
            ToggleSelection(btnLoadMission, this);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (btnNewMission.Image == filledButton)
            {
                this.Dispose();
                ShowCamSettings();
            }
            else if (btnLoadMission.Image == filledButton)
            {
                /*Load saved mission*/
            }
            else { }
        }
    }
}
