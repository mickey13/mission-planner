using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTAir : Form
    {
        // Vector from mouse to selected pushpin
        Vector _mouseToMarker;

        // Private field to track if current pushpin is being dragged or not
        private bool _IsPinDragging;

        /// <summary>
        /// Custom EventArgs for updating polygon coords
        /// </summary>
        public class PolygonEventArgs : EventArgs
        {
            public LocationCollection PolygonCoordinates { get; }

            public PolygonEventArgs(LocationCollection coordinates)
            {
                PolygonCoordinates = coordinates;
            }
        }

        /// <summary>
        /// Set up map polygon, pushpin layer, and pushpin list
        /// </summary>
        void _initializeMapChildren()
        {
            bingMapsUserControl1.myMap.Children.Clear();
            SetUpNewPolygon();
            polygonPointLayer = new MapLayer();
            pushPinList = new List<Pushpin>();
            bingMapsUserControl1.myMap.Children.Add(polygonPointLayer);
        }

        /// <summary>
        /// Create mission boundary from given coordinates
        /// </summary>
        /// <param name="locationToBeAdded"></Location to add to the boundary>
        void _updatePolygonBoundary(Location locationToBeAdded)
        {
            // Visual representation of polygon point/vertex
            Pushpin polygonPushPin = new Pushpin();
            //polygonPt.Stroke = new SolidColorBrush(missionBoundaryColor);
            //polygonPt.StrokeThickness = 3;
            polygonPushPin.Width = 16;
            polygonPushPin.Height = 16;
            polygonPushPin.Background = new SolidColorBrush(missionBoundaryColor);

            // Add handlers to the pushpin
            polygonPushPin.MouseRightButtonDown += new MouseButtonEventHandler(pin_MouseRightButtonDown);
            polygonPushPin.MouseRightButtonUp += new MouseButtonEventHandler(pin_MouseRightButtonUp);

            // If polygon already being showed
            if (bingMapsUserControl1.myMap.Children.Contains(newPolygon))
            {
                // Create/reset mission boundary
                missionBounds = new LocationCollection();

                // Save locations before creating new polygon
                foreach (Location location in newPolygon.Locations)
                {
                    missionBounds.Add(location);
                }

                // Create new polygon
                bingMapsUserControl1.myMap.Children.Remove(newPolygon);
                SetUpNewPolygon();

                // Add saved locations to new polygon
                newPolygon.Locations = missionBounds;
            }

            newPolygon.Locations.Add(locationToBeAdded);
            polygonPushPin.Location = locationToBeAdded;

            // Add pushpin to map
            polygonPointLayer.AddChild(polygonPushPin, locationToBeAdded);
            pushPinList.Add(polygonPushPin);

            // Set focus back to the map so that +/- work for zoom in/out
            bingMapsUserControl1.myMap.Focus();

            // If there are two or more points, add the polygon layer to the map
            if (newPolygon.Locations.Count >= 2)
            {
                bingMapsUserControl1.myMap.Children.Add(newPolygon);
            }
        }

        public AFTAir()
        {
            InitializeComponent();

            // Send menu panel and compass button to correct starting location
            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();
            btnFlightLines.Location = new System.Drawing.Point(12, 654);

            // Initialize map and polygon
            bingMapsUserControl1.myMap.CredentialsProvider = new ApplicationIdCredentialsProvider(bingMapsKey);
            _initializeMapChildren();
            bingMapsUserControl1.myMap.Focus();

            // Subscribe to events for mouse double click, mouse move, map loading, and for initiating a polygon edit
            bingMapsUserControl1.myMap.MouseDoubleClick += new MouseButtonEventHandler(MyMap_MouseDoubleClick);
            bingMapsUserControl1.myMap.MouseMove += new System.Windows.Input.MouseEventHandler(myMap_MouseMove);
            bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;
            aftNewMission.AirPolygonEditRequested += aftNewMission_AirPolygonEditRequested;
        }

        private void AFTAir_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map
            this.bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;
        }

        private void MyMap_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Set starting map location and zoom level of map
            this.bingMapsUserControl1.myMap.SetView(locationStart, zoomStart);
        }

        private void aftNewMission_AirPolygonEditRequested(object sender, PolygonEventArgs e)
        {
            // Handle event when it's raised in aftNewMission
            // Clear map before loading boundary from saved file
            _initializeMapChildren();

            // Update map with new polygon coordinates
            foreach (Location loc in e.PolygonCoordinates)
            {
                _updatePolygonBoundary(loc);
            }
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disable double-click zoom feature
            e.Handled = true;

            // Capture mouse screen coords, convert to lat/long
            Point mousePosition = e.GetPosition(null);
            Location polygonPointLocation = bingMapsUserControl1.myMap.ViewportPointToLocation(mousePosition);

            // Update polygon with new point
            _updatePolygonBoundary(polygonPointLocation);
        }

        void pin_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            // Update pushpin fields/properties
            SelectedPushpin = (Pushpin)sender;
            _IsPinDragging = true;
            _mouseToMarker = Point.Subtract(
            bingMapsUserControl1.myMap.LocationToViewportPoint(SelectedPushpin.Location),
            e.GetPosition(bingMapsUserControl1.myMap));
        }

        void pin_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Save updated pushpin locations
            LocationCollection locCol = new LocationCollection();
            foreach (Pushpin p in pushPinList)
            {
                locCol.Add(p.Location);
            }

            // Create new polygon and set focus back to the map so that +/- work for zoom in/out
            bingMapsUserControl1.myMap.Children.Remove(newPolygon);
            SetUpNewPolygon();
            bingMapsUserControl1.myMap.Focus();

            // Add updated locations to new polygon
            newPolygon.Locations = locCol;
            bingMapsUserControl1.myMap.Children.Add(newPolygon);

            // Update fields/properties
            _IsPinDragging = false;
            SelectedPushpin = null;
        }

        private void myMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (_IsPinDragging && SelectedPushpin != null)
                {
                    SelectedPushpin.Location = bingMapsUserControl1.myMap.ViewportPointToLocation(Point.Add(e.GetPosition(bingMapsUserControl1.myMap), _mouseToMarker));
                    e.Handled = true;
                }
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
            if (aftNewMission == null || aftNewMission.IsDisposed)
            {
                aftNewMission = new AFTNewMission();
            }
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
            ShowAdvSettings(false, true);
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
            if (aftNewMission == null || aftNewMission.IsDisposed)
            {
                aftNewMission = new AFTNewMission();
            }
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
