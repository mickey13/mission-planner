using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using static MissionPlanner.AFTStateMachine.ApplicationState;

namespace MissionPlanner
{
    public partial class AFTStateMachine : Form
    {
        public AFTStateMachine()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RunProgram();
        }

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

        public class ApplicationState
        {
            public enum States
            {
                NOT_SPECIFIED,              // No state yet specified

                AFTMAIN,                    // Main screen
                AFTGROUND,                  // AFT ground interface
                AFTAIR,                     // AFT air interface
                CUSTOM,                     // Launches traditional Mission Planner interface


                ERROR,                      // Perform error processing
                FINISHED,                   // Perform wrap-up processing

                TERMINATE,                  // Forces application exit
            }

            // Start at the main screen
            public static States nextState = States.AFTMAIN;
        }

        public static void DisposeAllForms()
        {
            aftMain.Dispose();
            aftGround.Dispose();
            aftAir.Dispose();

            if (custom != null)
            {
                custom.Dispose();
            }
        }

        public static void RunProgram()
        {
            // Form to open
            States currentState = ApplicationState.nextState;

            while (currentState != States.TERMINATE)
            {
                bool successful = false;
                switch (currentState)
                {
                    // Open main screen
                    case States.AFTMAIN:
                        if (aftMain == null)
                        {
                            aftMain = new MainAFT();
                            aftGround = new AFTGround();
                            aftAir = new AFTAir();
                            successful = true;
                        }
                        if (aftMain != null || successful)
                        {
                            aftMain.ShowDialog();
                        }
                        else
                        {
                            ApplicationState.nextState = States.ERROR;
                        }
                        break;

                    // Open ground interface
                    case States.AFTGROUND:
                        if (aftGround != null || successful)
                        {
                            aftGround.ShowDialog();
                        }
                        else
                        {
                            ApplicationState.nextState = States.ERROR;
                        }
                        break;

                    // Open air interface
                    case States.AFTAIR:
                        if (aftAir != null || successful)
                        {
                            aftAir.ShowDialog();
                        }
                        else
                        {
                            ApplicationState.nextState = States.ERROR;
                        }
                        break;

                    // Open custom interface
                    case States.CUSTOM:
                        if (custom == null)
                        {
                            custom = new MainV2();
                            successful = true;
                        }
                        if (custom != null || successful)
                        {
                            custom.ShowDialog();
                        }
                        else
                        {
                            ApplicationState.nextState = States.ERROR;
                        }
                        break;

                    // Display error message
                    case States.ERROR:
                        MessageBox.Show(
                            String.Format(
                                @"Error in {0}",
                                currentState.ToString()));
                        ApplicationState.nextState = States.FINISHED;
                        break;

                    // Terminate the program
                    case States.TERMINATE:

                        break;

                    // Terminate the program
                    case States.FINISHED:
                        ApplicationState.nextState = States.TERMINATE;
                        break;

                    default:

                        break;
                }
                currentState = ApplicationState.nextState;
            }

            DisposeAllForms();
            Application.Exit();
        }
    }
}
