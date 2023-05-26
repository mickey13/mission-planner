using Accord.Statistics.Kernels;
using netDxf;
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
    public partial class AFTGround : Form
    {
        MainAFT MainAFT = new MainAFT();

        public AFTGround()
        {
            InitializeComponent();
        }

        private void groundForm_Load(object sender, EventArgs e)
        {

        }

        private void groundToggleButton_Click(object sender, EventArgs e)
        {
            // Toggle between light and dark mode
            if (MainAFT.ToggleColorMode(this))
            {
                groundToggleButton.Image = MainAFT.togPicDark;
            }
            // If in dark mode
            else
            {
                groundToggleButton.Image = MainAFT.togPicLight;
            }
        }
    }
}
