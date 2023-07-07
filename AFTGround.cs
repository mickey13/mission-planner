//using SkiaSharp;
//using SkiaSharp.Views.Desktop;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {
        // Mission boundary polygon
        MapPolygon missionBounds = new MapPolygon();

        // Most recent location of mouse click event
        private Location location = null;

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
            bingMapsUserControl1.MouseLeftButtonUp +=
                new MouseButtonEventHandler(myMap_MouseLeftButtonUp);

            bingMapsUserControl1.MapTapped +=
                new MouseButtonEventHandler(myMap_MapTapped);

            // Customize mission boundary polygon
            missionBounds.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            missionBounds.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            missionBounds.StrokeThickness = 5;
            missionBounds.Opacity = 0.7;

            // Mission boundary polygon location collection
            missionBounds.Locations = new LocationCollection() { };

            /*// See if internet connection avaiable for map
            try
            {
                System.Net.IPHostEntry e =
                     System.Net.Dns.GetHostEntry("www.google.com");
            }
            catch
            {
                gMap.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.",
                      "GMap.NET - Demo.WindowsForms", MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
            }

            // config map
            gMap.MapProvider = GMapProviders.OpenStreetMap;
            gMap.Position = new PointLatLng(54.6961334816182, 25.2985095977783);
            gMap.MinZoom = 0;
            gMap.MaxZoom = 24;
            gMap.Zoom = 9;*/
        }

        /*private void SkCanvasView_OnPaintSurface
        (object sender, SKPaintSurfaceEventArgs e)
        {
            // Init skcanvas
            SKImageInfo skImageInfo = e.Info;
            SKSurface skSurface = e.Surface;
            SKCanvas skCanvas = skSurface.Canvas;

            // clear the canvas surface
            skCanvas.Clear(SKColors.SkyBlue);

            // retrieve the canvas info
            var skCanvasWidth = skImageInfo.Width;
            var skCanvasheight = skImageInfo.Height;

            // move canvas's X,Y to center of screen
            skCanvas.Translate((float)skCanvasWidth / 2,
                        (float)skCanvasheight / 2);

            // set the pixel scale of the canvas
            skCanvas.Scale(skCanvasWidth / 200f);

            SKPaint skPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = SKColors.Blue,
            };
        }*/

        //
        // Groundwork for getting lat/long from Google Maps while using an API
        //

        /*LatLng mapLocation;
        Point screenLocation;

        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;
        SKPath path = null;
        SKPaint thickLinePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Orange,
            StrokeWidth = 50
        };

        // Hold all corners of mission border
        List<LatLng> cornerCoords = new List<LatLng>();

        //@Override
        public void onMapReady(GoogleMaps map)
        {
            GoogleMaps mMap = map;
            mMap.setOnMapClickListener(this);
            mMap.setOnMapLongClickListener(this);
            mMap.setOnCameraIdleListener(this);
        }
        //'point' holds the value of LatLng coordinates
        //@Override
        public void onMapClick(LatLng point)
        {
            mapLocation = point;
            cornerCoords.Add(point);
            //Convert from x/y to lat/lng (reverse this somehow) (save this as screenLocation)
            yourGoogleMapInstance.Projection.FromScreenLocation(APointObject);

            // Convert from lat/lng to x/y and subtract with viewport's top left corner to get true pixel coordinates on screen
            var numTiles = 1 << map.getZoom();
            var projection = map.getProjection();
            var worldCoordinate = projection.fromLatLngToPoint(latLng);
            var pixelCoordinate = new google.maps.Point(
                    worldCoordinate.x * numTiles,
                    worldCoordinate.y * numTiles);

            var topLeft = new google.maps.LatLng(
                map.getBounds().getNorthEast().lat(),
                map.getBounds().getSouthWest().lng()
            );

            var topLeftWorldCoordinate = projection.fromLatLngToPoint(topLeft);
            var topLeftPixelCoordinate = new google.maps.Point(
                    topLeftWorldCoordinate.x * numTiles,
                    topLeftWorldCoordinate.y * numTiles);

            return new google.maps.Point(
                    pixelCoordinate.x - topLeftPixelCoordinate.x,
                    pixelCoordinate.y - topLeftPixelCoordinate.y
            )

            // Create path while simulatneously recording lat/lng
            if (path == null)
            {
                path = new SKPath();
                path.MoveTo(screenLocation.X, screenLocation.Y);
            }
            else
            {
                path.LineTo(screenLocation.X, screenLocation.Y);

                // Display thick line
                thickLinePaint.StrokeJoin = SKStrokeJoin.Round;
                canvas.DrawPath(path, thickLinePaint);
            }
        }*/

        //
        // Groundwork for allowing user to real-time draw mission boundary
        //

        /*void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 75,
                TextAlign = SKTextAlign.Right
            };

            SKPaint thickLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Orange,
                StrokeWidth = 50
            };

            SKPaint thinLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 2
            };
            //float xCoord1 = screenLocation.X;
            //float y = screenLocation.Y;
            float xCoord1 = 100;
            float xCoord2 = info.Width - xCoord1;
            float y = 2 * textPaint.FontSpacing;

            // Get stroke-join value
            SKStrokeJoin strokeJoin = SKStrokeJoin.Round;

            // Create path
            SKPath path = new SKPath();
            path.MoveTo(xCoord1, y - 80);
            path.LineTo(xCoord1, y + 80);
            path.LineTo(xCoord2, y + 80);

            // Display thick line
            thickLinePaint.StrokeJoin = strokeJoin;
            canvas.DrawPath(path, thickLinePaint);

            // Display thin line
            canvas.DrawPath(path, thinLinePaint);
            y += 3 * textPaint.FontSpacing;
        }*/

        private void AFTGround_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map, create map load handler
            this.bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;
            this.bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;

            // Create handler for clicking map
            MyMap.MouseLeftButtonUp += new EventHandler<MapEventArgs>(MyMap_MouseLeftButtonUp);
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

        void myMap_MouseLeftButtonUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*if (e.targetType == "map")
            {
                var point = new Microsoft.Maps.Point(e.getX(), e.getY());
                var loc = e.target.tryPixelToLocation(point);
                location = new Location(loc.latitude, loc.longitude);
            }*/
            CoordPoint pt = this.bingMapsUserControl1.ScreenPointToCoordPoint(e.Location);

            Point mousePosition = e.GetPosition(this);
            location = bingMapsUserControl1.ViewportPointToLocation(e.ViewportPoint);

            // Add clicked position to mission boundary locations
            missionBounds.Locations.Add(location);

            // If enough locations to make a shape, show the shape
            if (missionBounds.Locations.Count > 2)
            {
                this.bingMapsUserControl1.myMap.Children.Add(missionBounds);
            }
        }

        // Event from UWP; if can use in WinForms this would be the ideal solution (I think)
        /*private void myMap_MapTapped(object sender, TappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(bingMapsUserControl1);
            Location location;
            bingMapsUserControl1.TryPixelToLocation(pos, out location);
        }*/

        private void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Geopoint location = args.Location;
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
