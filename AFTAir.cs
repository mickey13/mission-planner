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
    public partial class AFTAir : Form
    {
        MainAFT MainAFT = new MainAFT();

        public AFTAir()
        {
            InitializeComponent();
        }

        private void AFTAir_Load(object sender, EventArgs e)
        {

        }

        private void airToggleButton_Click(object sender, EventArgs e)
        {
            // Toggle between light and dark mode
            if (MainAFT.ToggleColorMode(this))
            {
                airToggleButton.Image = MainAFT.togPicDark;
            }
            // If in dark mode
            else
            {
                airToggleButton.Image = MainAFT.togPicLight;
            }
        }
    }
}
