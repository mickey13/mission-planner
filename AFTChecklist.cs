using System;
using System.Windows.Forms;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTChecklist : Form
    {
        public AFTChecklist()
        {
            InitializeComponent();

            // Initialize with rounded corners
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 90, 90));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Image = boxChecked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Image = boxChecked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Image = boxChecked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Image = boxChecked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Image = boxChecked;
        }

        private void confirmationButton_Click(object sender, EventArgs e)
        {
            // If checklist complete
            if ((button1.Image == boxChecked) && (button2.Image == boxChecked) && (button3.Image == boxChecked) && (button4.Image == boxChecked) && (button5.Image == boxChecked))
            {
                checklistConfirmed = true;
                /*Initiate flight?*/

                this.Hide();
            }
        }
    }
}
