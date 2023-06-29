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

namespace MissionPlanner
{
    public partial class AFTVehicleConnecting : Form
    {
        public AFTVehicleConnecting()
        {
            InitializeComponent();
        }

        private void AFTVehicleConnecting_Load(object sender, EventArgs e)
        {
            /*Connect drone*/

            // Show pre-flight checklist
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }

            checklist.Show();
            checklist.BringToFront();
            this.Close();
        }
    }
}
