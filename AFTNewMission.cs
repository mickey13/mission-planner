using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class AFTNewMission : Form
    {
        public static AFTSettingsCam aftSetCam = null;

        public static Bitmap emptyButton = MissionPlanner.Properties.Resources.circle_hollow;
        public static Bitmap filledButton = MissionPlanner.Properties.Resources.circle_hollow;
        public AFTNewMission()
        {
            InitializeComponent();
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            if (btnNewMission.Image == emptyButton)
            {
                btnNewMission.Image = filledButton;
                btnLoadMission.Image = emptyButton;
            }
            else
            {
                btnNewMission.Image = emptyButton;
            }
        }

        private void btnLoadMission_Click(object sender, EventArgs e)
        {
            if (btnLoadMission.Image == emptyButton)
            {
                btnLoadMission.Image = filledButton;
                btnNewMission.Image = emptyButton;
            }
            else
            {
                btnLoadMission.Image = emptyButton;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (btnNewMission.Image == filledButton)
            {
                if (aftSetCam == null)
                {
                    aftSetCam = new AFTSettingsCam();
                }
                this.Dispose();
                aftSetCam.ShowDialog();
            }
            else if (btnLoadMission.Image == filledButton)
            {

            }
            else { }
        }
    }
}
