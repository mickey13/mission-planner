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
        // Mission boundary polygon
        MapPolygon missionBounds = new MapPolygon();

        // Most recent location of mouse click event
        private Location location = null;

        //
        // Custom polygon addition
        //

        // The user defined polygon to add to the map.
        MapPolygon newPolygon = null;
        // The map layer containing the polygon points defined by the user.
        MapLayer polygonPointLayer = new MapLayer();

        public AFTGround()
        {
            InitializeComponent();

            // Send menu panel to correct starting location
            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();

            // Set WebView to correct position
            webView21.Dock = DockStyle.Fill;

            // Send compass button to correct starting location
            btnFlightLines.Location = new System.Drawing.Point(12, 654);

            // Capture left mouse button release on map
            //bingMapsUserControl1.MouseLeftButtonUp +=
            //  new MouseButtonEventHandler(myMap_MouseLeftButtonUp);

            /*bingMapsUserControl1.MapTapped +=
                new MouseButtonEventHandler(myMap_MapTapped);*/

            // Customize mission boundary polygon
            missionBounds.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            missionBounds.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            missionBounds.StrokeThickness = 5;
            missionBounds.Opacity = 0.7;

            // Mission boundary polygon location collection
            missionBounds.Locations = new LocationCollection() { };

            //
            // Custom polygon addition
            //

            //Set focus to map
            this.bingMapsUserControl1.myMap.Focus();
            SetUpNewPolygon();
            // Adds location points to the polygon for every single mouse click
            this.bingMapsUserControl1.myMap.MouseLeftButtonUp += new MouseButtonEventHandler(
            MyMap_MouseLeftButtonUp);

            // Adds the layer that contains the polygon points
            this.bingMapsUserControl1.myMap.Children.Add(polygonPointLayer);
        }

        //
        // Custom polygon addition
        //

        private void SetUpNewPolygon()
        {
            newPolygon = new MapPolygon();

            // Defines the polygon fill details
            newPolygon.Locations = new LocationCollection();
            newPolygon.Fill = new SolidColorBrush(Colors.Blue);
            newPolygon.Stroke = new SolidColorBrush(Colors.Green);
            newPolygon.StrokeThickness = 3;
            newPolygon.Opacity = 0.8;

            //Set focus back to the map so that +/- work for zoom in/out
            this.bingMapsUserControl1.myMap.Focus();
        }
        private void MyMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            // Creates a location for a single polygon point and adds it to
            // the polygon's point location list.
            //Point mousePosition = e.GetPosition(this);
            Point mousePosition = e.GetPosition(null);
            //Point mousePosition = Control.MousePosition;

            //Convert the mouse coordinates to a location on the map
            Location polygonPointLocation = this.bingMapsUserControl1.myMap.ViewportPointToLocation(
                mousePosition);
            newPolygon.Locations.Add(polygonPointLocation);

            // A visual representation of a polygon point.
            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Colors.Red);
            r.Stroke = new SolidColorBrush(Colors.Yellow);
            r.StrokeThickness = 1;
            r.Width = 8;
            r.Height = 8;

            // Adds a small square where the user clicked, to mark the polygon point.
            polygonPointLayer.AddChild(r, polygonPointLocation);
            //Set focus back to the map so that +/- work for zoom in/out
            this.bingMapsUserControl1.myMap.Focus();

            //If there are two or more points, add the polygon layer to the map
            if (newPolygon.Locations.Count >= 2)
            {
                // Removes the polygon points layer.
                polygonPointLayer.Children.Clear();

                // Adds the filled polygon layer to the map.
                this.bingMapsUserControl1.myMap.Children.Add(newPolygon);
                SetUpNewPolygon();
            }
        }

        private void AFTGround_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map, create map load handler
            this.bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;

            // Create handlers for loading and clicking map
            this.bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;
            //this.bingMapsUserControl1.myMap.MouseLeftButtonUp += MyMap_MouseLeftButtonUp;
        }

        // Set starting location of Bing map
        private void MyMap_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var location = new Location(47.604, -122.329);
            this.bingMapsUserControl1.myMap.SetView(location, 12);
        }

        //
        //Stuff from MainV2
        //

        /*public void LoadGDALImages(object nothing)
        {
            if (Settings.Instance.ContainsKey("GDALImageDir"))
            {
                try
                {
                    Utilities.GDAL.ScanDirectory(Settings.Instance["GDALImageDir"]);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        private void BGCreateMaps(object state)
        {
            // sort logs
            try
            {
                MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));

                MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.rlog"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                // create maps
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog", SearchOption.AllDirectories));
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.bin", SearchOption.AllDirectories));
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.log", SearchOption.AllDirectories));

                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }*/

        //
        // Prototype prior to custom polygon addition
        //

        /*void MyMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //CoordPoint pt = this.bingMapsUserControl1.myMap.ScreenPointToCoordPoint(e.Location);

            Point mousePosition = e.GetPosition(null);
            location = bingMapsUserControl1.myMap.ViewportPointToLocation(mousePosition);

            // Add clicked position to mission boundary locations
            missionBounds.Locations.Add(location);

            // If enough locations to make a shape, show the shape
            if (missionBounds.Locations.Count > 2)
            {
                this.bingMapsUserControl1.myMap.Children.Add(missionBounds);
            }
        }*/

        /*private void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Geopoint location = args.Location;
        }*/

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
            ShowAdvSettings(false);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            if ((aftReturnHome == null) || aftReturnHome.IsDisposed)
            {
                aftReturnHome = new AFTReturnHome();
            }
            aftReturnHome.Show();
            aftReturnHome.BringToFront();
        }

        private void btnFly_Click(object sender, EventArgs e)
        {
            AFTVehiclePowerUp powerUp = new AFTVehiclePowerUp();
            powerUp.Show();
            powerUp.BringToFront();
        }

        private void btnCreateMission_Click(object sender, EventArgs e)
        {
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

        private void webView21_Click(object sender, EventArgs e)
        {

        }
    }
}
