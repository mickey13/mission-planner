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

            // Initialize map and polygon
            bingMapsUserControl1.myMap.CredentialsProvider = new ApplicationIdCredentialsProvider(bingMapsKey);
            SetUpNewPolygon();
            this.bingMapsUserControl1.myMap.Focus();

            // Create handlers for right mouse button click and map load
            this.bingMapsUserControl1.myMap.MouseDoubleClick += new MouseButtonEventHandler(MyMap_MouseDoubleClick);
            this.bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;
            this.bingMapsUserControl1.myMap.MouseMove += new System.Windows.Input.MouseEventHandler(myMap_MouseMove);

            //this.bingMapsUserControl1.myMap.ManipulationStarting += MyMap_ManipulationStarting;
            //this.bingMapsUserControl1.myMap.ManipulationDelta += MyMap_ManipulationDelta;

            //newPolygon.MouseRightButtonDown += new MouseButtonEventHandler(NewPolygon_MouseRightButtonDownResize);

            // Adds the layer that contains the polygon points/vertices
            polygonPointLayer = new MapLayer();
            this.bingMapsUserControl1.myMap.Children.Add(polygonPointLayer);

            pushPinVertices = new List<Pushpin>();
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


        //private bool _shapeEdit;
        //public MapPolygon selectedPoly { get; set; }
        public List<Pushpin> pushPinVertices { get; set; }


        /*void NewPolygon_MouseRightButtonDownResize(object sender, MouseButtonEventArgs e)
        {

            if (!_shapeEdit && selectedPoly == null)
            {
                _shapeEdit = true;
                selectedPoly = sender as MapPolygon;
                pushPinVertices = new List<Pushpin>();
                int i = 0;
                foreach (Microsoft.Maps.MapControl.WPF.Location vertice in selectedPoly.Locations)
                {
                    Pushpin verticeBlock = new Pushpin();
                    // I use a template to place a 'vertice marker' instead of a pushpin, il provide resource below
                    verticeBlock.Template = (ControlTemplate)System.Windows.Application.Current.Resources["PushPinTemplate"];
                    verticeBlock.Content = "vertice";
                    verticeBlock.Location = vertice;
                    verticeBlock.MouseRightButtonDown += new MouseButtonEventHandler(pin_MouseRightButtonDown);
                    verticeBlock.MouseRightButtonUp += new MouseButtonEventHandler(pin_MouseRightButtonUp);
                    bingMapsUserControl1.myMap.Children.Add(verticeBlock);
                    pushPinVertices.Add(verticeBlock);
                    i++;

                }
            }

        }*/



        /*private void myMap_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                if (_shapeEdit && selectedPoly != null)
                {

                    foreach (Pushpin p in pushPinVertices)
                    {
                        bingMapsUserControl1.myMap.Children.Remove(p);
                    }

                    _shapeEdit = false;
                    selectedPoly = null;

                }

            }
        }
        // Note: I needed my window to pass bing maps the keydown event
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            myMap_KeyDown(sender, e);
        }*/


        // === Editable polygon Events ===

        // ==== Draggable pushpin events =====
        Vector _mouseToMarker;
        private bool _IsPinDragging;
        public Pushpin SelectedPushpin { get; set; }

        void pin_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            LocationCollection locCol = new LocationCollection();
            foreach (Pushpin p in pushPinVertices)
            {
                locCol.Add(p.Location);

            }

            // Create new polygon and set focus back to the map so that +/- work for zoom in/out
            this.bingMapsUserControl1.myMap.Children.Remove(newPolygon);
            SetUpNewPolygon();
            this.bingMapsUserControl1.myMap.Focus();


            // Add saved locations to new polygon
            newPolygon.Locations = locCol;
            this.bingMapsUserControl1.myMap.Children.Add(newPolygon);
            //bingMapRefresh();

            _IsPinDragging = false;
            SelectedPushpin = null;
        }

        void pin_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SelectedPushpin = (Pushpin)sender;
            _IsPinDragging = true;
            _mouseToMarker = Point.Subtract(
            bingMapsUserControl1.myMap.LocationToViewportPoint(SelectedPushpin.Location),
            e.GetPosition(bingMapsUserControl1.myMap));

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
        // ==== Draggable pushpin events =====
        // Nice little maprefresh I found online since the bingmap WPF doesnt always seem to update elements after certain event orders
        /*private void bingMapRefresh()
        {
            //myMap.UpdateLayout();
            var mapCenter = bingMapsUserControl1.myMap.Center;
            mapCenter.Latitude += 0.00001;
            bingMapsUserControl1.myMap.SetView(mapCenter, bingMapsUserControl1.myMap.ZoomLevel);
        }*/

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disable double-click zoom feature
            e.Handled = true;

            // Visual representation of polygon point/vertex
            Pushpin polygonPt = new Pushpin();
            //polygonPt.Stroke = new SolidColorBrush(missionBoundaryColor);
            //polygonPt.StrokeThickness = 3;
            polygonPt.Width = 16;
            polygonPt.Height = 16;
            polygonPt.Background = new SolidColorBrush(missionBoundaryColor);

            polygonPt.MouseRightButtonDown += new MouseButtonEventHandler(pin_MouseRightButtonDown);
            polygonPt.MouseRightButtonUp += new MouseButtonEventHandler(pin_MouseRightButtonUp);

            // Capture mouse screen coords, convert to lat/long
            Point mousePosition = e.GetPosition(null);
            Location polygonPointLocation = bingMapsUserControl1.myMap.ViewportPointToLocation(mousePosition);

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
                this.bingMapsUserControl1.myMap.Children.Remove(newPolygon);


                // Create new polygon and set focus back to the map so that +/- work for zoom in/out
                SetUpNewPolygon();
                this.bingMapsUserControl1.myMap.Focus();

                // Add saved locations to new polygon
                newPolygon.Locations = missionBounds;
            }

            // Add clicked lat/long position to mission boundary
            newPolygon.Locations.Add(polygonPointLocation);

            polygonPt.Location = polygonPointLocation;

            // Add point/vertice marker to map
            polygonPointLayer.AddChild(polygonPt, polygonPointLocation);

            pushPinVertices.Add(polygonPt);

            // Set focus back to the map so that +/- work for zoom in/out
            this.bingMapsUserControl1.myMap.Focus();

            // If there are two or more points, add the polygon layer to the map
            if (newPolygon.Locations.Count >= 2)
            {
                // Removes the polygon points layer.
                //polygonPointLayer.Children.Clear();

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
