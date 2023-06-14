using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.AFTMDIContainer;

namespace MissionPlanner
{
    public partial class AFTChecklist : Form
    {
        // Initialize with rounded corners
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        List<Control> shadowControls = new List<Control>();
        Bitmap shadowBmp = null;

        public AFTChecklist()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 90, 90));

            //shadowControls.Add(panel1);
            //this.Refresh();
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
            if ((button1.Image == boxChecked) && (button2.Image == boxChecked) && (button3.Image ==  boxChecked) && (button4.Image == boxChecked) && (button5.Image == boxChecked))
            {

            }
        }

        /*private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (shadowBmp == null || shadowBmp.Size != this.Size)
            {
                shadowBmp?.Dispose();
                shadowBmp = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            }
            foreach (Control control in shadowControls)
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddRectangle(new Rectangle(control.Location.X, control.Location.Y, control.Size.Width, control.Size.Height));
                    DrawShadowSmooth(gp, 100, 60, shadowBmp);
                }
                e.Graphics.DrawImage(shadowBmp, new Point(0, 0));
            }
        }

        private static void DrawShadowSmooth(GraphicsPath gp, int intensity, int radius, Bitmap dest)
        {
            using (Graphics g = Graphics.FromImage(dest))
            {
                g.Clear(Color.Transparent);
                g.CompositingMode = CompositingMode.SourceCopy;
                double alpha = 0;
                double astep = 0;
                double astepstep = (double)intensity / radius / (radius / 2D);
                for (int thickness = radius; thickness > 0; thickness--)
                {
                    using (Pen p = new Pen(Color.FromArgb((int)alpha, 0, 0, 0), thickness))
                    {
                        p.LineJoin = LineJoin.Round;
                        g.DrawPath(p, gp);
                    }
                    alpha += astep;
                    astep += astepstep;
                }
            }
        }*/
    }
}
