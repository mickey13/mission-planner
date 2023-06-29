using System;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class AFTSettingsMore : Form
    {
        public AFTSettingsMore()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
