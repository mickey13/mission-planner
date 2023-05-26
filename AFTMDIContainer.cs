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
    public partial class AFTMDIContainer : Form
    {
        // Declaring forms
        public static MainAFT aftMain = null;
        public static AFTGround aftGround = null;
        public static AFTAir aftAir = null;
        public static MainV2 custom = null;

        // Pictures and colors for color modes
        public static Bitmap aftLogoLight = MissionPlanner.Properties.Resources.AFT_logo_black;
        public static Bitmap aftLogoDark = MissionPlanner.Properties.Resources.AFT_logo_white;
        public static Bitmap togPicLight = MissionPlanner.Properties.Resources.tog_img_for_light_mode;
        public static Bitmap togPicDark = MissionPlanner.Properties.Resources.tog_img_for_dark_mode;
        public static Bitmap lineLight = MissionPlanner.Properties.Resources.line_black;
        public static Bitmap lineDark = MissionPlanner.Properties.Resources.line_white;

        public static Color lightColor = System.Drawing.SystemColors.Control;
        public static Color darkColor = System.Drawing.SystemColors.ControlText;

        public AFTMDIContainer()
        {
            InitializeComponent();
        }

        private void AFTMDIContainer_Load(object sender, EventArgs e)
        {
            InitializeForm(aftMain, this);
            InitializeForm(aftGround, this);
            InitializeForm(aftAir, this);

            aftMain.Show();
        }

        public static Form InitializeForm(Form child, Form parent)
        {
            if (parent != null)
            {
                if ((child == null || child.IsDisposed) & child != parent)
                {
                    if (child == aftMain)
                    {
                        aftMain = new MainAFT();
                        aftMain.MdiParent = parent;

                        return aftMain;
                    }
                    else if (child == aftGround)
                    {
                        aftGround = new AFTGround();
                        aftGround.MdiParent = parent;

                        return aftGround;
                    }
                    else if (child == aftAir)
                    {
                        aftAir = new AFTAir();
                        aftAir.MdiParent = parent;

                        return aftAir;
                    }
                    else
                    {
                        custom = new MainV2();
                        custom.MdiParent = parent;

                        return custom;
                    }
                }
                return child;
            }
            return null;
        }
    }
}
