﻿using GMap.NET;
using GMap.NET.MapProviders;
//using SkiaSharp;
//using SkiaSharp.Views.Desktop;
using System;
using System.Windows.Forms;
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

            // See if internet connection avaiable for map
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
            gMap.Zoom = 9;
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
            /*// Initialize map
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //gmap.SetPositionByKeywords("Paris, France");
            gmap.Position = new GMap.NET.PointLatLng(48.8589507, 2.2775175);

            // Hide center red cross
            gmap.ShowCenter = false;*/

            /*SKImageInfo imageInfo = new SKImageInfo(1284, 781);
            using (SKSurface surface = SKSurface.Create(imageInfo))
            {
                SKCanvas canvas = surface.Canvas;

                canvas.Clear(SKColors.Red.WithAlpha(0));

                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.Blue;
                    paint.IsAntialias = true;
                    paint.StrokeWidth = 15;
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawCircle(500, 500, 30, paint); //arguments are x position, y position, radius, and paint
                }

                using (SKImage image = surface.Snapshot())
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (MemoryStream mStream = new MemoryStream(data.ToArray()))
                {
                    Bitmap bm = new Bitmap(mStream, false);
                    pictureBox1.Image = bm;
                }
            }*/
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
