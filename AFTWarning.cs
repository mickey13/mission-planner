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
using static MissionPlanner.MainAFT;
/*using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;*/

namespace MissionPlanner
{
    public partial class AFTWarning : Form
    {

        public AFTWarning()
        {
            InitializeComponent();

            // Apply drop shadow effect
            (new MissionPlanner.DropShadow()).ApplyShadows(this);
        }

        private void confirmationButton_Click(object sender, EventArgs e)
        {
            //confirmationButton.BackColor = Color.LimeGreen;
            //confirmationButton.Text = "Loading Firmware";
            this.Dispose();

            AFTLoadingScreen aftLoad = new AFTLoadingScreen();
            aftLoad.MdiParent = aftMain.MdiParent;
            SyncColorsAndInitialize(new List<Form> { aftLoad }, aftMain, aftMain.MdiParent);
            aftLoad.Show();

            InitializeForm(custom, aftMain.MdiParent);
            custom.Show();
            aftLoad.Close();
        }
        //getting "slide to confirm" to work
        /*private bool _isSwiped;

        private void SwipeableTextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.IsInertial && !_isSwiped)
            {
                var swipedDistance = e.Cumulative.Translation.X;

                if (Math.Abs(swipedDistance) <= 2) return;

                if (swipedDistance > 0)
                {
                    SwipeableTextBlock.Text = "Right Swiped";
                }
                else
                {
                    SwipeableTextBlock.Text = "Left Swiped";
                }
                _isSwiped = true;
            }
        }

        private void SwipeableTextBlock_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _isSwiped = false;
        }*/
    }
}
