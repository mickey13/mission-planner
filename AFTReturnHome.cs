using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTReturnHome : Form
    {
        public AFTReturnHome()
        {
            InitializeComponent();

            // Initialize with rounded corners
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 142, 142));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Pause flight and return home*/
        }
    }
}
