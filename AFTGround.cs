using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {
        public AFTGround()
        {
            InitializeComponent();

            // Send menu panel to correct starting location
            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();

            // Send compass button to correct starting location
            btnFlightLines.Location = new System.Drawing.Point(12, 654);

            // Set Credentials for map
            bingMapsUserControl1.myMap.CredentialsProvider = new ApplicationIdCredentialsProvider(bingMapsKey);

            // Set focus to map and create initial mission boundary polygon
            this.bingMapsUserControl1.myMap.Focus();
            SetUpNewPolygon();
            this.bingMapsUserControl1.myMap.Focus();

            // Create handlers for right mouse button click and map load
            this.bingMapsUserControl1.myMap.MouseRightButtonUp += new MouseButtonEventHandler(MyMap_MouseRightButtonUp);
            this.bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;

            // Adds the layer that contains the polygon points/vertices
            polygonPointLayer = new MapLayer();
            this.bingMapsUserControl1.myMap.Children.Add(polygonPointLayer);
        }

        private void AFTGround_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map
            this.bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;
        }

        private void MyMap_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Set starting map location and zoom level of map
            this.bingMapsUserControl1.myMap.SetView(locationStart, zoomStart);
        }

        private void MyMap_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Visual representation of polygon point/vertice
            Ellipse polygonPt = new Ellipse();
            polygonPt.Stroke = new SolidColorBrush(missionBoundaryColor);
            polygonPt.StrokeThickness = 3;
            polygonPt.Width = 16;
            polygonPt.Height = 16;

            // Capture mouse screen coords, convert to lat/long
            Point mousePosition = e.GetPosition(null);
            Location polygonPointLocation = this.bingMapsUserControl1.myMap.ViewportPointToLocation(mousePosition);

            // If polygon already being showed
            if (this.bingMapsUserControl1.myMap.Children.Contains(newPolygon))
            {
                // Create/reset mission boundary
                missionBounds = new LocationCollection();

                // Save locations before creating new polygon
                foreach (Location loc in newPolygon.Locations)
                {
                    missionBounds.Add(loc);
                }

                // Clear polygon
                this.bingMapsUserControl1.myMap.Children.Clear();

                // Create new polygon and set focus back to the map so that +/- work for zoom in/out
                SetUpNewPolygon();
                this.bingMapsUserControl1.myMap.Focus();

                // Add saved locations to new polygon
                foreach (Location loc in missionBounds)
                {
                    newPolygon.Locations.Add(loc);
                }
            }

            // Add clicked lat/long position to mission boundary
            newPolygon.Locations.Add(polygonPointLocation);

            // Add point/vertice marker to map
            polygonPointLayer.AddChild(polygonPt, polygonPointLocation);

            // Set focus back to the map so that +/- work for zoom in/out
            this.bingMapsUserControl1.myMap.Focus();

            // If there are two or more points, add the polygon layer to the map
            if (newPolygon.Locations.Count >= 2)
            {
                // Removes the polygon points layer.
                polygonPointLayer.Children.Clear();

                // Adds the filled polygon layer to the map.
                this.bingMapsUserControl1.myMap.Children.Add(newPolygon);
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            // Show menu panel
            if (this.Controls.GetChildIndex(sideMenuPanel) == 0)
            {
                sideMenuPanel.Dock = DockStyle.None;
                sideMenuPanel.SendToBack();
            }
            // Hide menu panel
            else
            {
                sideMenuPanel.Dock = DockStyle.Left;
                this.Controls.SetChildIndex(sideMenuPanel, 0);
            }
        }

        private void btnNewMission_Click(object sender, EventArgs e)
        {
            // Instantiate and show new mission screen
            aftNewMission = new AFTNewMission();
            aftNewMission.Show();
            aftNewMission.BringToFront();
        }

        private void btnPreFlightCheck_Click(object sender, EventArgs e)
        {
            // If checklist hasn't been instantiated yet, instantiate it
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }

            // Show checklist
            checklist.Show();
            checklist.BringToFront();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // Show advanced settings with close button visible
            ShowAdvSettings(false);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            // Instantiate and show return home screen
            aftReturnHome = new AFTReturnHome();
            aftReturnHome.Show();
            aftReturnHome.BringToFront();
        }

        private void btnFly_Click(object sender, EventArgs e)
        {
            // Instantiate and show power up vehicle screen
            powerUp = new AFTVehiclePowerUp();
            powerUp.Show();
            powerUp.BringToFront();
        }

        private void btnCreateMission_Click(object sender, EventArgs e)
        {
            // Instantiate and show new mission screen
            aftNewMission = new AFTNewMission();
            aftNewMission.Show();
            aftNewMission.BringToFront();
        }

        private void btnFlightLines_Click(object sender, EventArgs e)
        {
            /*Show a low res bmap with flight lines showing the quickest safe route home*/
        }

        private void btnVidDownlink_Click(object sender, EventArgs e)
        {
            /*Switch to video downlink*/
        }
    }
}
