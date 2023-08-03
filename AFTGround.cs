using Microsoft.Maps.MapControl.WPF;
using MissionPlanner.ArduPilot;
using MissionPlanner.Comms;
using MissionPlanner.Controls;
using MissionPlanner.GCSViews.ConfigurationView;
using MissionPlanner.Utilities;
using MissionPlanner.Warnings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static MissionPlanner.AFTController;

namespace MissionPlanner
{
    public partial class AFTGround : Form
    {

        #region From MainV2, for MAVLink

        //private static readonly ILog log =
        //LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /*
        public static event EventHandler LayoutChanged;

        private static DisplayView _displayConfiguration = File.Exists(DisplayViewExtensions.custompath)
            ? new DisplayView().Custom()
            : new DisplayView().Advanced();

        public static DisplayView DisplayConfiguration
        {
            get { return _displayConfiguration; }
            set
            {
                _displayConfiguration = value;
                Settings.Instance["displayview"] = _displayConfiguration.ConvertToString();
                LayoutChanged?.Invoke(null, EventArgs.Empty);
            }
        }
        */
        /// <summary>
        /// Active Comport interface
        /// </summary>
        public static MAVLinkInterface comPort
        {
            get { return _comPort; }
            set
            {
                if (_comPort == value)
                    return;
                _comPort = value;
                if (aftGround == null)
                    return;
                _comPort.MavChanged -= aftGround.comPort_MavChanged;
                _comPort.MavChanged += aftGround.comPort_MavChanged;
                aftGround.comPort_MavChanged(null, null);
            }
        }

        static MAVLinkInterface _comPort = new MAVLinkInterface();

        /// <summary>
        /// passive comports
        /// </summary>
        public static List<MAVLinkInterface> Comports = new List<MAVLinkInterface>();

        /// <summary>
        /// Comport name
        /// </summary>
        //public static string comPortName = "";

        //public static int comPortBaud = 57600;

        public static string inputtedPortName = "preset";
        public static string inputtedBaud = "57600";

        /// <summary>
        /// store the time we first connect
        /// </summary>
        public static DateTime connecttime = DateTime.Now;

        DateTime nodatawarning = DateTime.Now;

        /// <summary>
        /// track the last heartbeat sent
        /// </summary>
        private DateTime heatbeatSend = DateTime.Now;

        /// <summary>
        /// declared here if i want a "single" instance of the form
        /// ie configuration gets reloaded on every click
        /// </summary>
        //public GCSViews.FlightData FlightData;

        //public static GCSViews.FlightPlanner FlightPlanner;

        //public static bool UseCachedParams { get; set; } = false;

        public static string titlebar;

        /// <summary>
        /// controls the main serial reader thread
        /// </summary>
        bool serialThread = false;

        bool pluginthreadrun = false;

        Thread pluginthread;

        ManualResetEvent SerialThreadrunner = new ManualResetEvent(false);

        /// <summary>
        /// speech engine enable
        /// </summary>
        public static bool speechEnable
        {
            get { return speechEngine == null ? false : speechEngine.speechEnable; }
            set
            {
                if (speechEngine != null) speechEngine.speechEnable = value;
            }
        }

        public static bool speech_armed_only = false;
        public static bool speechEnabled()
        {
            if (!speechEnable)
            {
                return false;
            }
            if (speech_armed_only)
            {
                return AFTGround.comPort.MAV.cs.armed;
            }
            return true;
        }

        /// <summary>
        /// spech engine static class
        /// </summary>
        public static ISpeech speechEngine { get; set; }

        /// <summary>
        /// This 'Control' is the toolstrip control that holds the comport combo, baudrate combo etc
        /// Otiginally seperate controls, each hosted in a toolstip sqaure, combined into this custom
        /// control for layout reasons.
        /// </summary>
        //public static ConnectionControl _connectionControl;

        void comPort_MavChanged(object sender, EventArgs e)
        {
            //log.Info($"Mav Changed {AFTGround.comPort.MAV.sysid}");

            HUD.Custom.src = AFTGround.comPort.MAV.cs;

            CustomWarning.defaultsrc = AFTGround.comPort.MAV.cs;

            MissionPlanner.Controls.PreFlight.CheckListItem.defaultsrc = AFTGround.comPort.MAV.cs;
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    //enable the payload control page if a mavlink gimbal is detected
                    if (aftGround.FlightData != null)
                    {
                        aftGround.FlightData.updatePayloadTabVisible();
                    }

                    //_connectionControl.UpdateSysIDS();
                });
            }
            else
            {
                //enable the payload control page if a mavlink gimbal is detected
                if (aftGround.FlightData != null)
                {
                    aftGround.FlightData.updatePayloadTabVisible();
                }

                //_connectionControl.UpdateSysIDS();
            }*/
        }

        void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            // try prevent crash on resume
            if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
            {
                doDisconnect(AFTGround.comPort);
            }
        }

        public static void doDisconnect(MAVLinkInterface comPort)
        {
            //log.Info("We are disconnecting");
            try
            {
                if (speechEngine != null) // cancel all pending speech
                    speechEngine.SpeakAsyncCancelAll();

                comPort.BaseStream.DtrEnable = false;
                comPort.Close();
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
            /*
            // now that we have closed the connection, cancel the connection stats
            // so that the 'time connected' etc does not grow, but the user can still
            // look at the now frozen stats on the still open form
            try
            {
                // if terminal is used, then closed using this button.... exception
                if (this.connectionStatsForm != null)
                    ((ConnectionStats)this.connectionStatsForm.Controls[0]).StopUpdates();
            }
            catch
            {
            }*/

            // refresh config window if needed
            //if (MyView.current != null)
            //{
            //if (MyView.current.Name == "HWConfig")
            //MyView.ShowScreen("HWConfig");
            //if (MyView.current.Name == "SWConfig")
            //MyView.ShowScreen("SWConfig");
            //}

            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    try
                    {
                        MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                );
            }
            catch
            {
            }

            //this.MenuConnect.Image = global::MissionPlanner.Properties.Resources.light_connect_icon;
        }
        /*
        void adsb_UpdatePlanePosition(object sender, MissionPlanner.Utilities.adsb.PointLatLngAltHdg adsb)
        {
            lock (adsblock)
            {
                var id = adsb.Tag;

                if (MainV2.instance.adsbPlanes.ContainsKey(id))
                {
                    // update existing
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Lat = adsb.Lat;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Lng = adsb.Lng;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Alt = adsb.Alt;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Heading = adsb.Heading;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Time = DateTime.Now;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).CallSign = adsb.CallSign;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Squawk = adsb.Squawk;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Raw = adsb.Raw;
                    ((adsb.PointLatLngAltHdg)instance.adsbPlanes[id]).Speed = adsb.Speed;
                }
                else
                {
                    // create new plane
                    MainV2.instance.adsbPlanes[id] =
                        new adsb.PointLatLngAltHdg(adsb.Lat, adsb.Lng,
                                adsb.Alt, adsb.Heading, adsb.Speed, id,
                                DateTime.Now)
                        { CallSign = adsb.CallSign, Squawk = adsb.Squawk, Raw = adsb.Raw };
                }
            }

            try
            {
                // dont rebroadcast something that came from the drone
                if (sender != null && sender is MAVLinkInterface)
                    return;

                MAVLink.mavlink_adsb_vehicle_t packet = new MAVLink.mavlink_adsb_vehicle_t();

                packet.altitude = (int)(adsb.Alt * 1000);
                packet.altitude_type = (byte)MAVLink.ADSB_ALTITUDE_TYPE.GEOMETRIC;
                packet.callsign = adsb.CallSign.MakeBytes();
                packet.squawk = adsb.Squawk;
                packet.emitter_type = (byte)MAVLink.ADSB_EMITTER_TYPE.NO_INFO;
                packet.heading = (ushort)(adsb.Heading * 100);
                packet.lat = (int)(adsb.Lat * 1e7);
                packet.lon = (int)(adsb.Lng * 1e7);
                packet.ICAO_address = uint.Parse(adsb.Tag, NumberStyles.HexNumber);

                packet.flags = (ushort)(MAVLink.ADSB_FLAGS.VALID_ALTITUDE | MAVLink.ADSB_FLAGS.VALID_COORDS |
                                            MAVLink.ADSB_FLAGS.VALID_HEADING | MAVLink.ADSB_FLAGS.VALID_CALLSIGN);

                //send to current connected
                MainV2.comPort.sendPacket(packet, MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid);
            }
            catch
            {

            }
        }
        */
        public static void doConnect(MAVLinkInterface comPort, string portname, string baud, bool getparams = true, bool showui = true)
        {
            bool skipconnectcheck = false;
            //log.Info($"We are connecting to {portname} {baud}");
            switch (portname)
            {
                case "preset":
                    skipconnectcheck = true;
                    /*this.BeginInvokeIfRequired(() =>
                    {
                        if (comPort.BaseStream is TcpSerial)
                            _connectionControl.CMB_serialport.Text = "TCP";
                        if (comPort.BaseStream is UdpSerial)
                            _connectionControl.CMB_serialport.Text = "UDP";
                        if (comPort.BaseStream is UdpSerialConnect)
                            _connectionControl.CMB_serialport.Text = "UDPCl";
                        if (comPort.BaseStream is SerialPort)
                        {
                            _connectionControl.CMB_serialport.Text = comPort.BaseStream.PortName;
                            _connectionControl.CMB_baudrate.Text = comPort.BaseStream.BaudRate.ToString();
                        }
                    });*/
                    break;
                case "TCP":
                    comPort.BaseStream = new TcpSerial();
                    //_connectionControl.CMB_serialport.Text = "TCP";
                    break;
                case "UDP":
                    comPort.BaseStream = new UdpSerial();
                    //_connectionControl.CMB_serialport.Text = "UDP";
                    break;
                case "WS":
                    comPort.BaseStream = new WebSocket();
                    //_connectionControl.CMB_serialport.Text = "WS";
                    break;
                case "UDPCl":
                    comPort.BaseStream = new UdpSerialConnect();
                    //_connectionControl.CMB_serialport.Text = "UDPCl";
                    break;
                case "AUTO":
                    // do autoscan
                    Comms.CommsSerialScan.Scan(true);
                    DateTime deadline = DateTime.Now.AddSeconds(50);
                    ProgressReporterDialogue prd = new ProgressReporterDialogue();
                    prd.UpdateProgressAndStatus(-1, "Waiting for ports");
                    prd.DoWork += sender =>
                    {
                        while (Comms.CommsSerialScan.foundport == false || Comms.CommsSerialScan.run == 1)
                        {
                            System.Threading.Thread.Sleep(500);
                            Console.WriteLine("wait for port " + CommsSerialScan.foundport + " or " +
                                              CommsSerialScan.run);
                            if (sender.doWorkArgs.CancelRequested)
                            {
                                sender.doWorkArgs.CancelAcknowledged = true;
                                return;
                            }

                            if (DateTime.Now > deadline)
                            {
                                //_connectionControl.IsConnected(false);
                                throw new Exception(Strings.Timeout);
                            }
                        }
                    };
                    prd.RunBackgroundOperationAsync();
                    return;
                default:
                    comPort.BaseStream = new SerialPort();
                    break;
            }
            /*
            // Tell the connection UI that we are now connected.
            this.BeginInvokeIfRequired(() =>
            {
                _connectionControl.IsConnected(true);

                // Here we want to reset the connection stats counter etc.
                this.ResetConnectionStats();
            });
            */
            comPort.MAV.cs.ResetInternals();

            //cleanup any log being played
            comPort.logreadmode = false;
            if (comPort.logplaybackfile != null)
                comPort.logplaybackfile.Close();
            comPort.logplaybackfile = null;

            try
            {
                //log.Info("Set Portname");
                // set port, then options
                if (portname.ToLower() != "preset")
                    comPort.BaseStream.PortName = portname;

                //log.Info("Set Baudrate");
                try
                {
                    if (baud != "" && baud != "0" && baud.IsNumber())
                        comPort.BaseStream.BaudRate = int.Parse(baud);
                }
                catch (Exception exp)
                {
                    //log.Error(exp);
                }

                // prevent serialreader from doing anything
                comPort.giveComport = true;

                //log.Info("About to do dtr if needed");
                /*// reset on connect logic.
                if (Settings.Instance.GetBoolean("CHK_resetapmonconnect") == true)
                {
                    //log.Info("set dtr rts to false");
                    comPort.BaseStream.DtrEnable = false;
                    comPort.BaseStream.RtsEnable = false;

                    comPort.BaseStream.toggleDTR();
                }
                */
                comPort.giveComport = false;
                /*
                // setup to record new logs
                try
                {
                    Directory.CreateDirectory(Settings.Instance.LogDir);
                    lock (this)
                    {
                        // create log names
                        var dt = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                        var tlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                   dt + ".tlog";
                        var rlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                   dt + ".rlog";

                        // check if this logname already exists
                        int a = 1;
                        while (File.Exists(tlog))
                        {
                            Thread.Sleep(1000);
                            // create new names with a as an index
                            dt = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-" + a.ToString();
                            tlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                   dt + ".tlog";
                            rlog = Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                   dt + ".rlog";
                        }

                        //open the logs for writing
                        comPort.logfile =
                            new BufferedStream(
                                File.Open(tlog, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                        comPort.rawlogfile =
                            new BufferedStream(
                                File.Open(rlog, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                        log.Info($"creating logfile {dt}.tlog");
                    }
                }
                catch (Exception exp2)
                {
                    log.Error(exp2);
                    CustomMessageBox.Show(Strings.Failclog);
                } // soft fail
                */
                // reset connect time - for timeout functions
                connecttime = DateTime.Now;

                // do the connect
                comPort.Open(false, skipconnectcheck, showui);

                if (!comPort.BaseStream.IsOpen)
                {
                    //log.Info("comport is closed. existing connect");
                    try
                    {
                        //_connectionControl.IsConnected(false);
                        //UpdateConnectIcon();
                        comPort.Close();
                    }
                    catch
                    {
                    }

                    return;
                }

                //158	MAV_COMP_ID_PERIPHERAL	Generic autopilot peripheral component ID. Meant for devices that do not implement the parameter microservice.
                if (getparams && comPort.MAV.compid != (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_PERIPHERAL)
                {
                    if (/*UseCachedParams && */File.Exists(comPort.MAV.ParamCachePath) &&
                        new FileInfo(comPort.MAV.ParamCachePath).LastWriteTime > DateTime.Now.AddHours(-1))
                    {
                        File.ReadAllText(comPort.MAV.ParamCachePath).FromJSON<MAVLink.MAVLinkParamList>()
                            .ForEach(a => comPort.MAV.param.Add(a));
                        comPort.MAV.param.TotalReported = comPort.MAV.param.TotalReceived;
                    }
                    else
                    {
                        if (Settings.Instance.GetBoolean("Params_BG", false))
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    comPort.getParamListMavftp(comPort.MAV.sysid, comPort.MAV.compid);
                                }
                                catch
                                {

                                }
                            });
                        }
                        else
                        {
                            comPort.getParamList();
                        }
                    }
                }

                // check for newer firmware
                if (showui)
                    Task.Run(() =>
                    {
                        try
                        {
                            string[] fields1 = comPort.MAV.VersionString.Split(' ');

                            var softwares = APFirmware.GetReleaseNewest(APFirmware.RELEASE_TYPES.OFFICIAL);

                            foreach (var item in softwares)
                            {
                                // check primare firmware type. ie arudplane, arducopter
                                if (fields1[0].ToLower().Contains(item.VehicleType.ToLower()))
                                {
                                    Version ver1 = VersionDetection.GetVersion(comPort.MAV.VersionString);
                                    Version ver2 = item.MavFirmwareVersion;

                                    if (ver2 > ver1)
                                    {
                                        Common.MessageShowAgain(Strings.NewFirmware + "-" + item.VehicleType + " " + ver2,
                                            Strings.NewFirmwareA + item.VehicleType + " " + ver2 + Strings.Pleaseup +
                                            "[link;https://discuss.ardupilot.org/tags/stable-release;Release Notes]");
                                        break;
                                    }

                                    // check the first hit only
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //log.Error(ex);
                        }
                    });

                //_connectionControl.UpdateSysIDS();

                //FlightData.CheckBatteryShow();

                // save the baudrate for this port
                //Settings.Instance[_connectionControl.CMB_serialport.Text.Replace(" ", "_") + "_BAUD"] =
                //_connectionControl.CMB_baudrate.Text;

                //this.Text = titlebar + " " + comPort.MAV.VersionString;
                /*
                // refresh config window if needed
                if (MyView.current != null && showui)
                {
                    if (MyView.current.Name == "HWConfig")
                        MyView.ShowScreen("HWConfig");
                    if (MyView.current.Name == "SWConfig")
                        MyView.ShowScreen("SWConfig");
                }
                */
                /*// load wps on connect option.
                if (Settings.Instance.GetBoolean("loadwpsonconnect") == true && showui)
                {
                    // only do it if we are connected.
                    if (comPort.BaseStream.IsOpen)
                    {
                        //MenuFlightPlanner_Click(null, null);
                        FlightPlanner.BUT_read_Click(null, null);
                    }
                }
                */
                // get any rallypoints
                if (AFTGround.comPort.MAV.param.ContainsKey("RALLY_TOTAL") &&
                    int.Parse(AFTGround.comPort.MAV.param["RALLY_TOTAL"].ToString()) > 0 && showui)
                {
                    try
                    {
                        if ((AFTGround.comPort.MAV.cs.capabilities & (uint)MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_RALLY) >= 0)
                        {/*
                            if (!MainV2.comPort.BaseStream.IsOpen)
                            {
                                CustomMessageBox.Show(Strings.PleaseConnect);
                                return;
                            }*/

                            mav_mission.download(AFTGround.comPort, AFTGround.comPort.MAV.sysid, AFTGround.comPort.MAV.compid,
                                MAVLink.MAV_MISSION_TYPE.RALLY).AwaitSync();
                            return;
                        }

                        if (AFTGround.comPort.MAV.param["RALLY_TOTAL"] == null || int.Parse(AFTGround.comPort.MAV.param["RALLY_TOTAL"].ToString()) < 1)
                        {
                            CustomMessageBox.Show("Not Supported or Nothing to Download");
                            return;
                        }

                        //rallypointoverlay.Markers.Clear();
                        aftGround._initializeMapChildren();

                        int count = int.Parse(AFTGround.comPort.MAV.param["RALLY_TOTAL"].ToString());

                        for (int a = 0; a < (count); a++)
                        {
                            try
                            {
                                var plla = AFTGround.comPort.getRallyPoint(a).AwaitSync();
                                count = plla.total;

                                Location pll = new Location(plla.plla.Lat, plla.plla.Lng);
                                aftGround._updatePolygonBoundary(pll);
                                /*rallypointoverlay.Markers.Add(new GMapMarkerRallyPt(new PointLatLng(plla.plla.Lat, plla.plla.Lng))
                                {
                                    Alt = (int)plla.plla.Alt,
                                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                                    ToolTipText = "Rally Point" + "\nAlt: " + (plla.plla.Alt * CurrentState.multiplieralt)
                                });*/
                            }
                            catch
                            {
                                CustomMessageBox.Show("Failed to get rally point", Strings.ERROR);
                                return;
                            }
                        }

                        //MainMap.UpdateMarkerLocalPosition(rallypointoverlay.Markers[0]);
                        var newHomeLat = newPolygon.Locations[0].Latitude;
                        var newHomeLng = newPolygon.Locations[0].Longitude;
                        var newHomeAlt = newPolygon.Locations[0].Altitude;

                        PointLatLngAlt newDroneHome = new PointLatLngAlt(newHomeLat, newHomeLng, newHomeAlt);
                        AFTGround.comPort.MAV.cs.PlannedHomeLocation = newDroneHome;

                        //MainMap.Invalidate();

                        // Max distance between rally points
                        double maxdist = 0;

                        foreach (var rally in comPort.MAV.rallypoints)
                        {
                            foreach (var rally1 in comPort.MAV.rallypoints)
                            {
                                var pnt1 = new PointLatLngAlt(rally.Value.y / 10000000.0f, rally.Value.x / 10000000.0f);
                                var pnt2 = new PointLatLngAlt(rally1.Value.y / 10000000.0f,
                                    rally1.Value.x / 10000000.0f);

                                var dist = pnt1.GetDistance(pnt2);

                                maxdist = Math.Max(maxdist, dist);
                            }
                        }

                        if (comPort.MAV.param.ContainsKey("RALLY_LIMIT_KM") &&
                            (maxdist / 1000.0) > (float)comPort.MAV.param["RALLY_LIMIT_KM"])
                        {
                            CustomMessageBox.Show(Strings.Warningrallypointdistance + " " +
                                                  (maxdist / 1000.0).ToString("0.00") + " > " +
                                                  (float)comPort.MAV.param["RALLY_LIMIT_KM"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Warn(ex);
                    }
                }

                // get any fences
                if (AFTGround.comPort.MAV.param.ContainsKey("FENCE_TOTAL") &&
                    int.Parse(AFTGround.comPort.MAV.param["FENCE_TOTAL"].ToString()) > 1 &&
                    AFTGround.comPort.MAV.param.ContainsKey("FENCE_ACTION") && showui)
                {
                    try
                    {
                        bool polygongridmode = false;
                        int count = 1;

                        if ((MainV2.comPort.MAV.cs.capabilities & (uint)MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_FENCE) > 0)
                        {/*
                            if (!MainV2.comPort.BaseStream.IsOpen)
                            {
                                CustomMessageBox.Show(Strings.PleaseConnect);
                                return;
                            }*/
                            mav_mission.download(MainV2.comPort, MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid,
                                    MAVLink.MAV_MISSION_TYPE.FENCE).AwaitSync();
                        }

                        if (MainV2.comPort.MAV.param["FENCE_ACTION"] == null || MainV2.comPort.MAV.param["FENCE_TOTAL"] == null)
                        {
                            CustomMessageBox.Show("Not Supported");
                            return;
                        }

                        if (int.Parse(MainV2.comPort.MAV.param["FENCE_TOTAL"].ToString()) <= 1)
                        {
                            CustomMessageBox.Show("Nothing to download");
                            return;
                        }
                        /*
                        geofenceoverlay.Polygons.Clear();
                        geofenceoverlay.Markers.Clear();
                        geofencepolygon.Points.Clear();
                        */
                        aftGround._initializeMapChildren();

                        for (int a = 0; a < count; a++)
                        {
                            try
                            {
                                var plla = MainV2.comPort.getFencePoint(a).AwaitSync();
                                count = plla.total;
                                //geofencepolygon.Points.Add(new PointLatLng(plla.plla.Lat, plla.plla.Lng));

                                Location pll = new Location(plla.plla.Lat, plla.plla.Lng);
                                aftGround._updatePolygonBoundary(pll);
                            }
                            catch
                            {
                                CustomMessageBox.Show("Failed to get fence point", Strings.ERROR);
                                return;
                            }
                        }

                        //MainMap.UpdateMarkerLocalPosition(rallypointoverlay.Markers[0]);
                        var newHomeLat = newPolygon.Locations[0].Latitude;
                        var newHomeLng = newPolygon.Locations[0].Longitude;
                        var newHomeAlt = newPolygon.Locations[0].Altitude;

                        PointLatLngAlt newDroneHome = new PointLatLngAlt(newHomeLat, newHomeLng, newHomeAlt);
                        AFTGround.comPort.MAV.cs.PlannedHomeLocation = newDroneHome;
                        /*
                        // do return location
                        geofenceoverlay.Markers.Add(
                            new GMarkerGoogle(new PointLatLng(geofencepolygon.Points[0].Lat, geofencepolygon.Points[0].Lng),
                                GMarkerGoogleType.red)
                            {
                                ToolTipMode = MarkerTooltipMode.OnMouseOver,
                                ToolTipText = "GeoFence Return"
                            });
                        geofencepolygon.Points.RemoveAt(0);

                        // add now - so local points are calced
                        geofenceoverlay.Polygons.Add(geofencepolygon);*/
                        /*
                        // update flight data
                        FlightData.geofence.Markers.Clear();
                        FlightData.geofence.Polygons.Clear();
                        FlightData.geofence.Polygons.Add(new GMapPolygon(geofencepolygon.Points, "gf fd")
                        {
                            Stroke = geofencepolygon.Stroke,
                            Fill = Brushes.Transparent
                        });
                        FlightData.geofence.Markers.Add(
                            new GMarkerGoogle(geofenceoverlay.Markers[0].Position, GMarkerGoogleType.red)
                            {
                                ToolTipText = geofenceoverlay.Markers[0].ToolTipText,
                                ToolTipMode = geofenceoverlay.Markers[0].ToolTipMode
                            });

                        MainMap.UpdatePolygonLocalPosition(geofencepolygon);
                        MainMap.UpdateMarkerLocalPosition(geofenceoverlay.Markers[0]);

                        MainMap.Invalidate();*/
                    }
                    catch (Exception ex)
                    {
                        //log.Warn(ex);
                    }
                }

                //Add HUD custom items source
                HUD.Custom.src = AFTGround.comPort.MAV.cs;

                // set connected icon
                //this.MenuConnect.Image = displayicons.disconnect;
            }
            catch (Exception ex)
            {
                //log.Warn(ex);
                try
                {
                    //_connectionControl.IsConnected(false);
                    //UpdateConnectIcon();
                    comPort.Close();
                }
                catch (Exception ex2)
                {
                    //log.Warn(ex2);
                }

                CustomMessageBox.Show($"Can not establish a connection\n\n{ex.Message}");
                return;
            }
        }

        public static void Connect()
        {
            comPort.giveComport = false;

            //log.Info("MenuConnect Start");

            // sanity check
            if (comPort.BaseStream.IsOpen && comPort.MAV.cs.groundspeed > 4)
            {
                if ((int)DialogResult.No ==
                    CustomMessageBox.Show(Strings.Stillmoving, Strings.Disconnect, MessageBoxButtons.YesNo))
                {
                    return;
                }
            }
            /*
            try
            {
                //log.Info("Cleanup last logfiles");
                // cleanup from any previous sessions
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.ErrorClosingLogFile + ex.Message, Strings.ERROR);
            }

            comPort.logfile = null;
            comPort.rawlogfile = null;
            */
            // decide if this is a connect or disconnect
            if (comPort.BaseStream.IsOpen)
            {
                doDisconnect(comPort);
            }
            else
            {
                doConnect(comPort, inputtedPortName, inputtedBaud);
            }

            //_connectionControl.UpdateSysIDS();

            if (comPort.BaseStream.IsOpen)
                loadph_serial();
        }

        public static void loadph_serial()
        {
            try
            {
                if (comPort.MAV.SerialString == "")
                    return;

                if (comPort.MAV.SerialString.Contains("CubeBlack") &&
                    !comPort.MAV.SerialString.Contains("CubeBlack+") &&
                    comPort.MAV.param.ContainsKey("INS_ACC3_ID") && comPort.MAV.param["INS_ACC3_ID"].Value == 0 &&
                    comPort.MAV.param.ContainsKey("INS_GYR3_ID") && comPort.MAV.param["INS_GYR3_ID"].Value == 0 &&
                    comPort.MAV.param.ContainsKey("INS_ENABLE_MASK") && comPort.MAV.param["INS_ENABLE_MASK"].Value >= 7)
                {
                    MissionPlanner.Controls.SB.Show("Param Scan");
                }
            }
            catch
            {
            }

            try
            {
                if (comPort.MAV.SerialString == "")
                    return;

                // brd type should be 3
                // devids show which sensor is not detected
                // baro does not list a devid

                //devop read spi lsm9ds0_ext_am 0 0 0x8f 1
                if (comPort.MAV.SerialString.Contains("CubeBlack") && !comPort.MAV.SerialString.Contains("CubeBlack+"))
                {
                    Task.Run(() =>
                    {
                        bool bad1 = false;
                        byte[] data = new byte[0];

                        comPort.device_op(comPort.MAV.sysid, comPort.MAV.compid, out data,
                            MAVLink.DEVICE_OP_BUSTYPE.SPI,
                            "lsm9ds0_ext_g", 0, 0, 0x8f, 1);
                        if (data.Length != 0 && (data[0] != 0xd4 && data[0] != 0xd7))
                            bad1 = true;

                        comPort.device_op(comPort.MAV.sysid, comPort.MAV.compid, out data,
                            MAVLink.DEVICE_OP_BUSTYPE.SPI,
                            "lsm9ds0_ext_am", 0, 0, 0x8f, 1);
                        if (data.Length != 0 && data[0] != 0x49)
                            bad1 = true;

                        if (bad1)
                            aftGround.BeginInvoke(method: (Action)delegate
                            {
                                MissionPlanner.Controls.SB.Show("SPI Scan");
                            });
                    });
                }

            }
            catch
            {
            }
        }
        /*
        private void CMB_serialport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_connectionControl.CMB_serialport.SelectedItem == _connectionControl.CMB_serialport.Text)
                return;

            comPortName = _connectionControl.CMB_serialport.Text;
            if (comPortName == "UDP" || comPortName == "UDPCl" || comPortName == "TCP" || comPortName == "AUTO")
            {
                _connectionControl.CMB_baudrate.Enabled = false;
            }
            else
            {
                _connectionControl.CMB_baudrate.Enabled = true;
            }

            try
            {
                // check for saved baud rate and restore
                if (Settings.Instance[_connectionControl.CMB_serialport.Text.Replace(" ", "_") + "_BAUD"] != null)
                {
                    _connectionControl.CMB_baudrate.Text =
                        Settings.Instance[_connectionControl.CMB_serialport.Text.Replace(" ", "_") + "_BAUD"];
                }
            }
            catch
            {
            }
        }
        */
        /// <summary>
        /// overriding the OnCLosing is a bit cleaner than handling the event, since it
        /// is this object.
        ///
        /// This happens before FormClosed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            //log.Info("MainV2_FormClosing");

            //log.Info("GMaps write cache");
            // speed up tile saving on exit
            //GMap.NET.GMaps.Instance.CacheOnIdleRead = false;
            //GMap.NET.GMaps.Instance.BoostCacheEngine = true;

            //Settings.Instance["MainHeight"] = this.Height.ToString();
            //Settings.Instance["MainWidth"] = this.Width.ToString();
            //Settings.Instance["MainMaximised"] = this.WindowState.ToString();

            //Settings.Instance["MainLocX"] = this.Location.X.ToString();
            //Settings.Instance["MainLocY"] = this.Location.Y.ToString();

            //log.Info("close logs");
            /*
            // close bases connection
            try
            {
                comPort.logreadmode = false;
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();

                comPort.logfile = null;
                comPort.rawlogfile = null;
            }
            catch
            {
            }
            */
            //log.Info("close ports");
            /*// close all connections
            foreach (var port in Comports)
            {
                try
                {
                    port.logreadmode = false;
                    if (port.logfile != null)
                        port.logfile.Close();

                    if (port.rawlogfile != null)
                        port.rawlogfile.Close();

                    port.logfile = null;
                    port.rawlogfile = null;
                }
                catch
                {
                }
            }
            */
            //log.Info("stop adsb");
            Utilities.adsb.Stop();

            //log.Info("stop WarningEngine");
            Warnings.WarningEngine.Stop();

            //log.Info("stop GStreamer");
            GStreamer.StopAll();

            //log.Info("closing vlcrender");
            try
            {
                while (vlcrender.store.Count > 0)
                    vlcrender.store[0].Stop();
            }
            catch
            {
            }

            //log.Info("closing pluginthread");
            /*
            pluginthreadrun = false;

            if (pluginthread != null)
            {
                try
                {
                    while (!PluginThreadrunner.WaitOne(100)) System.Windows.Forms.Application.DoEvents();
                }
                catch
                {
                }

                pluginthread.Join();
            }
            */
            //log.Info("closing serialthread");

            serialThread = false;

            //log.Info("closing joystickthread");

            //joystickthreadrun = false;

            //log.Info("closing httpthread");

            // if we are waiting on a socket we need to force an abort
            httpserver.Stop();

            //log.Info("sorting tlogs");
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    try
                    {
                        MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                );
            }
            catch
            {
            }

            //log.Info("closing MyView");

            // close all tabs
            //MyView.Dispose();

            //log.Info("closing fd");
            try
            {
                //FlightData.Dispose();
            }
            catch
            {
            }

            //log.Info("closing fp");
            try
            {
                //FlightPlanner.Dispose();
            }
            catch
            {
            }

            //log.Info("closing sim");
            try
            {
                //Simulation.Dispose();
            }
            catch
            {
            }

            try
            {
                if (comPort.BaseStream.IsOpen)
                    comPort.Close();
            }
            catch
            {
            } // i get alot of these errors, the port is still open, but not valid - user has unpluged usb

            // save config
            //SaveConfig();

            //Console.WriteLine(httpthread?.IsAlive);
            //Console.WriteLine(pluginthread?.IsAlive);

            //log.Info("MainV2_FormClosing done");

            //if (MONO)
            //this.Dispose();
        }
        /*
        private void LoadConfig()
        {
            try
            {
                //log.Info("Loading config");

                Settings.Instance.Load();

                comPortName = Settings.Instance.ComPort;
            }
            catch (Exception ex)
            {
                //log.Error("Bad Config File", ex);
            }
        }*/
        /*
        private void SaveConfig()
        {
            try
            {
                //log.Info("Saving config");
                Settings.Instance.ComPort = comPortName;

                //if (_connectionControl != null)
                    //Settings.Instance.BaudRate = _connectionControl.CMB_baudrate.Text;

                Settings.Instance.APMFirmware = AFTGround.comPort.MAV.cs.firmware.ToString();

                Settings.Instance.Save();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString());
            }
        }
        */
        private async void SerialReader()
        {
            if (serialThread == true)
                return;
            serialThread = true;

            //SerialThreadrunner.Reset();

            int minbytes = 10;

            int altwarningmax = 0;

            bool armedstatus = false;

            string lastmessagehigh = "";

            DateTime speechcustomtime = DateTime.Now;

            DateTime speechlowspeedtime = DateTime.Now;

            DateTime linkqualitytime = DateTime.Now;

            while (serialThread)
            {
                try
                {
                    await Task.Delay(1).ConfigureAwait(false); // was 5

                    try
                    {
                        if (ConfigTerminal.comPort is MAVLinkSerialPort)
                        {
                        }
                        else
                        {
                            if (ConfigTerminal.comPort != null && ConfigTerminal.comPort.IsOpen)
                                continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex);
                    }
                    /*
                    // update connect/disconnect button and info stats
                    try
                    {
                        //UpdateConnectIcon();
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex);
                    }
                    */
                    // 30 seconds interval speech options
                    if (speechEnabled() && speechEngine != null && (DateTime.Now - speechcustomtime).TotalSeconds > 30 &&
                        (AFTGround.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (AFTGround.speechEngine.IsReady)
                        {
                            if (Settings.Instance.GetBoolean("speechcustomenabled"))
                            {
                                AFTGround.speechEngine.SpeakAsync(ArduPilot.Common.speechConversion(comPort.MAV,
                                    "" + Settings.Instance["speechcustom"]));
                            }

                            speechcustomtime = DateTime.Now;
                        }

                        // speech for battery alerts
                        //speechbatteryvolt
                        float warnvolt = Settings.Instance.GetFloat("speechbatteryvolt");
                        float warnpercent = Settings.Instance.GetFloat("speechbatterypercent");

                        if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                            AFTGround.comPort.MAV.cs.battery_voltage <= warnvolt &&
                            AFTGround.comPort.MAV.cs.battery_voltage >= 5.0)
                        {
                            if (AFTGround.speechEngine.IsReady)
                            {
                                AFTGround.speechEngine.SpeakAsync(ArduPilot.Common.speechConversion(comPort.MAV,
                                    "" + Settings.Instance["speechbattery"]));
                            }
                        }
                        else if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                                 (AFTGround.comPort.MAV.cs.battery_remaining) < warnpercent &&
                                 AFTGround.comPort.MAV.cs.battery_voltage >= 5.0 &&
                                 AFTGround.comPort.MAV.cs.battery_remaining != 0.0)
                        {
                            if (AFTGround.speechEngine.IsReady)
                            {
                                AFTGround.speechEngine.SpeakAsync(
                                    ArduPilot.Common.speechConversion(comPort.MAV,
                                        "" + Settings.Instance["speechbattery"]));
                            }
                        }
                    }

                    // speech for airspeed alerts
                    if (speechEnabled() && speechEngine != null && (DateTime.Now - speechlowspeedtime).TotalSeconds > 10 &&
                        (AFTGround.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (Settings.Instance.GetBoolean("speechlowspeedenabled") == true &&
                            AFTGround.comPort.MAV.cs.armed)
                        {
                            float warngroundspeed = Settings.Instance.GetFloat("speechlowgroundspeedtrigger");
                            float warnairspeed = Settings.Instance.GetFloat("speechlowairspeedtrigger");

                            if (AFTGround.comPort.MAV.cs.airspeed < warnairspeed)
                            {
                                if (AFTGround.speechEngine.IsReady)
                                {
                                    AFTGround.speechEngine.SpeakAsync(
                                        ArduPilot.Common.speechConversion(comPort.MAV,
                                            "" + Settings.Instance["speechlowairspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else if (AFTGround.comPort.MAV.cs.groundspeed < warngroundspeed)
                            {
                                if (AFTGround.speechEngine.IsReady)
                                {
                                    AFTGround.speechEngine.SpeakAsync(
                                        ArduPilot.Common.speechConversion(comPort.MAV,
                                            "" + Settings.Instance["speechlowgroundspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else
                            {
                                speechlowspeedtime = DateTime.Now;
                            }
                        }
                    }

                    // speech altitude warning - message high warning
                    if (speechEnabled() && speechEngine != null &&
                        (AFTGround.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        float warnalt = float.MaxValue;
                        if (Settings.Instance.ContainsKey("speechaltheight"))
                        {
                            warnalt = Settings.Instance.GetFloat("speechaltheight");
                        }

                        try
                        {
                            altwarningmax = (int)Math.Max(AFTGround.comPort.MAV.cs.alt, altwarningmax);

                            if (Settings.Instance.GetBoolean("speechaltenabled") == true &&
                                AFTGround.comPort.MAV.cs.alt != 0.00 &&
                                (AFTGround.comPort.MAV.cs.alt <= warnalt) && AFTGround.comPort.MAV.cs.armed)
                            {
                                if (altwarningmax > warnalt)
                                {
                                    if (AFTGround.speechEngine.IsReady)
                                        AFTGround.speechEngine.SpeakAsync(
                                            ArduPilot.Common.speechConversion(comPort.MAV,
                                                "" + Settings.Instance["speechalt"]));
                                }
                            }
                        }
                        catch
                        {
                        } // silent fail


                        try
                        {
                            // say the latest high priority message
                            if (AFTGround.speechEngine.IsReady &&
                                lastmessagehigh != AFTGround.comPort.MAV.cs.messageHigh &&
                                AFTGround.comPort.MAV.cs.messageHigh != null)
                            {
                                if (!AFTGround.comPort.MAV.cs.messageHigh.StartsWith("PX4v2 ") &&
                                    !AFTGround.comPort.MAV.cs.messageHigh.StartsWith("PreArm:")) // Supress audibly repeating PreArm messages
                                {
                                    AFTGround.speechEngine.SpeakAsync(AFTGround.comPort.MAV.cs.messageHigh);
                                    lastmessagehigh = AFTGround.comPort.MAV.cs.messageHigh;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    // not doing anything
                    if (!AFTGround.comPort.logreadmode && !comPort.BaseStream.IsOpen)
                    {
                        altwarningmax = 0;
                    }

                    // attenuate the link qualty over time
                    if ((DateTime.Now - AFTGround.comPort.MAV.lastvalidpacket).TotalSeconds >= 1)
                    {
                        if (linkqualitytime.Second != DateTime.Now.Second)
                        {
                            AFTGround.comPort.MAV.cs.linkqualitygcs =
                                (ushort)(AFTGround.comPort.MAV.cs.linkqualitygcs * 0.8f);
                            linkqualitytime = DateTime.Now;

                            // force redraw if there are no other packets are being read
                            this.BeginInvokeIfRequired(
                                (Action)
                                delegate { GCSViews.FlightData.myhud.Invalidate(); });
                        }
                    }

                    // data loss warning - wait min of 3 seconds, ignore first 30 seconds of connect, repeat at 5 seconds interval
                    if ((DateTime.Now - AFTGround.comPort.MAV.lastvalidpacket).TotalSeconds > 3
                        && (DateTime.Now - connecttime).TotalSeconds > 30
                        && (DateTime.Now - nodatawarning).TotalSeconds > 5
                        && (AFTGround.comPort.logreadmode || comPort.BaseStream.IsOpen)
                        && AFTGround.comPort.MAV.cs.armed)
                    {
                        var msg = "WARNING No Data for " + (int)(DateTime.Now - AFTGround.comPort.MAV.lastvalidpacket).TotalSeconds + " Seconds";
                        AFTGround.comPort.MAV.cs.messageHigh = msg;
                        if (speechEnabled() && speechEngine != null)
                        {
                            if (AFTGround.speechEngine.IsReady)
                            {
                                AFTGround.speechEngine.SpeakAsync(msg);
                                nodatawarning = DateTime.Now;
                            }
                        }
                    }

                    // get home point on armed status change.
                    if (armedstatus != AFTGround.comPort.MAV.cs.armed && comPort.BaseStream.IsOpen)
                    {
                        armedstatus = AFTGround.comPort.MAV.cs.armed;
                        // status just changed to armed
                        if (AFTGround.comPort.MAV.cs.armed == true &&
                            AFTGround.comPort.MAV.apname != MAVLink.MAV_AUTOPILOT.INVALID &&
                            AFTGround.comPort.MAV.aptype != MAVLink.MAV_TYPE.GIMBAL)
                        {
                            System.Threading.ThreadPool.QueueUserWorkItem(state =>
                            {
                                Thread.CurrentThread.Name = "Arm State change";
                                try
                                {
                                    while (comPort.giveComport == true)
                                        Thread.Sleep(100);

                                    AFTGround.comPort.MAV.cs.HomeLocation = new PointLatLngAlt(AFTGround.comPort.getWP(0));
                                    if (aftGround != null)// && MyView.current.Name == "FlightPlanner")
                                    {
                                        // MainV2: update home if we are on flight data tab
                                        //aftGround: Update home location and pushpin; assumes that polygon is already created and has at least one point
                                        newPolygon.Locations[0] = new Location(AFTGround.comPort.MAV.cs.HomeLocation.Lat, AFTGround.comPort.MAV.cs.HomeLocation.Lng);
                                        if (polygonPointLayer.Children[0] is Pushpin)
                                        {
                                            Pushpin p = (Pushpin)polygonPointLayer.Children[0];
                                            p.Location = newPolygon.Locations[0];
                                        }
                                    }
                                }
                                catch
                                {
                                    // dont hang this loop
                                    this.BeginInvokeIfRequired(
                                        (Action)
                                        delegate
                                        {
                                            CustomMessageBox.Show("Failed to update home location (" +
                                                                  AFTGround.comPort.MAV.sysid + ")");
                                        });
                                }
                            });
                        }

                        if (speechEnable && speechEngine != null)
                        {
                            if (Settings.Instance.GetBoolean("speecharmenabled"))
                            {
                                string speech = armedstatus
                                    ? Settings.Instance["speecharm"]
                                    : Settings.Instance["speechdisarm"];
                                if (!string.IsNullOrEmpty(speech))
                                {
                                    AFTGround.speechEngine.SpeakAsync(
                                        ArduPilot.Common.speechConversion(comPort.MAV, speech));
                                }
                            }
                        }
                    }
                    /*
                    if (comPort.MAV.param.TotalReceived < comPort.MAV.param.TotalReported)
                    {
                        if (comPort.MAV.param.TotalReported > 0 && comPort.BaseStream.IsOpen)
                        {
                            this.BeginInvokeIfRequired(() =>
                            {
                                try
                                {
                                    //aftGround.status1.Percent =
                                    //(comPort.MAV.param.TotalReceived / (double)comPort.MAV.param.TotalReported) *
                                    //100.0;
                                }
                                catch (Exception e)
                                {
                                    //log.Error(e);
                                }
                            });
                        }
                    }
                    */
                    // send a hb every seconds from gcs to ap
                    if (heatbeatSend.Second != DateTime.Now.Second)
                    {
                        MAVLink.mavlink_heartbeat_t htb = new MAVLink.mavlink_heartbeat_t()
                        {
                            type = (byte)MAVLink.MAV_TYPE.GCS,
                            autopilot = (byte)MAVLink.MAV_AUTOPILOT.INVALID,
                            mavlink_version = 3 // MAVLink.MAVLINK_VERSION
                        };

                        // enumerate each link
                        foreach (var port in Comports.ToArray())
                        {
                            if (!port.BaseStream.IsOpen)
                                continue;

                            // poll for params at heartbeat interval - primary mav on this port only
                            if (!port.giveComport)
                            {
                                try
                                {
                                    // poll only when not armed
                                    if (!port.MAV.cs.armed && DateTime.Now > connecttime.AddSeconds(60))
                                    {
                                        port.getParamPoll();
                                        port.getParamPoll();
                                    }
                                }
                                catch
                                {
                                }
                            }

                            // there are 3 hb types we can send, mavlink1, mavlink2 signed and unsigned
                            bool sentsigned = false;
                            bool sentmavlink1 = false;
                            bool sentmavlink2 = false;

                            // enumerate each mav
                            foreach (var MAV in port.MAVlist)
                            {
                                try
                                {
                                    // poll for version if we dont have it - every mav every port
                                    if (!port.giveComport && MAV.cs.capabilities == 0 &&
                                        (DateTime.Now.Second % 20) == 0 && MAV.cs.version < new Version(0, 1))
                                        port.getVersion(MAV.sysid, MAV.compid, false);

                                    // are we talking to a mavlink2 device
                                    if (MAV.mavlinkv2)
                                    {
                                        // is signing enabled
                                        if (MAV.signing)
                                        {
                                            // check if we have already sent
                                            if (sentsigned)
                                                continue;
                                            sentsigned = true;
                                        }
                                        else
                                        {
                                            // check if we have already sent
                                            if (sentmavlink2)
                                                continue;
                                            sentmavlink2 = true;
                                        }
                                    }
                                    else
                                    {
                                        // check if we have already sent
                                        if (sentmavlink1)
                                            continue;
                                        sentmavlink1 = true;
                                    }

                                    port.sendPacket(htb, MAV.sysid, MAV.compid);
                                }
                                catch (Exception ex)
                                {
                                    //log.Error(ex);
                                    // close the bad port
                                    try
                                    {
                                        port.Close();
                                    }
                                    catch
                                    {
                                    }

                                    // refresh the screen if needed
                                    if (port == AFTGround.comPort)
                                    {
                                        // refresh config window if needed
                                        /*if (MyView.current != null)
                                        {
                                            this.BeginInvoke((MethodInvoker)delegate ()
                                            {
                                                if (MyView.current.Name == "HWConfig")
                                                    MyView.ShowScreen("HWConfig");
                                                if (MyView.current.Name == "SWConfig")
                                                    MyView.ShowScreen("SWConfig");
                                            });
                                        }*/
                                    }
                                }
                            }
                        }

                        heatbeatSend = DateTime.Now;
                    }

                    // if not connected or busy, sleep and loop
                    if (!comPort.BaseStream.IsOpen || comPort.giveComport == true)
                    {
                        if (!comPort.BaseStream.IsOpen)
                        {
                            // check if other ports are still open
                            foreach (var port in Comports)
                            {
                                if (port.BaseStream.IsOpen)
                                {
                                    Console.WriteLine("Main comport shut, swapping to other mav");
                                    comPort = port;
                                    break;
                                }
                            }
                        }

                        await Task.Delay(100).ConfigureAwait(false);
                    }

                    // read the interfaces
                    foreach (var port in Comports.ToArray())
                    {
                        if (!port.BaseStream.IsOpen)
                        {
                            // skip primary interface
                            if (port == comPort)
                                continue;

                            // modify array and drop out
                            Comports.Remove(port);
                            port.Dispose();
                            break;
                        }

                        DateTime startread = DateTime.Now;

                        // must be open, we have bytes, we are not yielding the port,
                        // the thread is meant to be running and we only spend 1 seconds max in this read loop
                        while (port.BaseStream.IsOpen && port.BaseStream.BytesToRead > minbytes &&
                               port.giveComport == false && serialThread && startread.AddSeconds(1) > DateTime.Now)
                        {
                            try
                            {
                                await port.readPacketAsync().ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex);
                            }
                        }

                        // update currentstate of sysids on the port
                        foreach (var MAV in port.MAVlist)
                        {
                            try
                            {
                                MAV.cs.UpdateCurrentSettings(null, false, port, MAV);
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Tracking.AddException(e);
                    //log.Error("Serial Reader fail :" + e.ToString());
                    try
                    {
                        comPort.Close();
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex);
                    }
                }
            }

            Console.WriteLine("SerialReader Done");
            SerialThreadrunner.Set();
        }

        #endregion

        #region Custom bitmap showing shortest distance

        private const string RoutesApiUrl = "http://dev.virtualearth.net/REST/v1/Routes/Driving";

        public async Task<double> GetDistanceAsync(Location loc1, Location loc2)
        {
            var lat1 = loc1.Latitude;
            var lon1 = loc1.Longitude;
            var lat2 = loc2.Latitude;
            var lon2 = loc2.Longitude;

            using (HttpClient httpClient = new HttpClient())
            {
                string requestUrl = $"{RoutesApiUrl}?wp.0={lat1},{lon1}&wp.1={lat2},{lon2}&key={bingMapsKey}";

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseContent);

                        double distanceInMeters = (double)jsonResponse["resourceSets"][0]["resources"][0]["travelDistance"];
                        double distanceInKilometers = distanceInMeters / 1000.0;

                        return distanceInKilometers;
                    }
                    else
                    {
                        // Handle API error here
                        Console.WriteLine("Bing Maps API request failed.");
                        return -1; // Or throw an exception, depending on your error handling strategy
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception here
                    Console.WriteLine($"Error: {ex.Message}");
                    return -1; // Or throw an exception, depending on your error handling strategy
                }
            }
        }

        // Draw a bitmap with the shortest distance and display it on the form
        private async Task DrawDistanceBitmap()
        {
            if (newPolygon.Locations.Count < 2)
            {
                // Not enough locations to calculate distance
                return;
            }

            // Calculate the shortest distance between the first and last pushpins
            double shortestDistance = await GetDistanceAsync(newPolygon.Locations[0], newPolygon.Locations[newPolygon.Locations.Count - 1]);

            // Create a new bitmap
            Bitmap bitmap = new Bitmap(1284, 781);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Fill the background
                graphics.Clear(System.Drawing.Color.White);

                // Draw the shortest distance on the bitmap
                string distanceText = $"Shortest Distance Home: {shortestDistance:F2} km";
                Font font = new Font(System.Drawing.FontFamily.GenericSansSerif, 12, System.Drawing.FontStyle.Regular);
                System.Drawing.Brush brush = System.Drawing.Brushes.Black;
                graphics.DrawString(distanceText, font, brush, new PointF(500, 500));
            }

            // Display the bitmap on the form
            picFlightLines.Image = bitmap;
        }

        #endregion

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

            // Leave first location (home location) as default color
            if (newPolygon.Locations.Count() > 1)
            {
                polygonPushPin.Background = new SolidColorBrush(missionBoundaryColor);
            }

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

        public AFTGround()
        {
            // MainV2
            // define default basestream
            comPort.BaseStream = new SerialPort();
            comPort.BaseStream.BaudRate = 57600;

            var homeLat = newPolygon.Locations[0].Latitude;
            var homeLng = newPolygon.Locations[0].Longitude;
            var homeAlt = newPolygon.Locations[0].Altitude;

            PointLatLngAlt droneHome = new PointLatLngAlt(homeLat, homeLng, homeAlt);
            AFTGround.comPort.MAV.cs.PlannedHomeLocation = droneHome;

            Comports.Add(comPort);
            AFTGround.comPort.MavChanged += comPort_MavChanged;
            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            InitializeComponent();

            // Send menu panel and compass button to correct starting location
            sideMenuPanel.Dock = DockStyle.None;
            sideMenuPanel.SendToBack();
            btnFlightLines.Location = new System.Drawing.Point(12, 654);

            // Initialize map and polygon
            elementHost1.Dock = DockStyle.Fill;
            bingMapsUserControl1.myMap.CredentialsProvider = new ApplicationIdCredentialsProvider(bingMapsKey);
            _initializeMapChildren();
            bingMapsUserControl1.myMap.Focus();

            // Subscribe to events for mouse double click, mouse move, map loading, and for initiating a polygon edit
            bingMapsUserControl1.myMap.MouseDoubleClick += new MouseButtonEventHandler(MyMap_MouseDoubleClick);
            bingMapsUserControl1.myMap.MouseMove += new System.Windows.Input.MouseEventHandler(myMap_MouseMove);
            bingMapsUserControl1.myMap.Loaded += MyMap_Loaded;
            aftNewMission.GroundPolygonEditRequested += aftNewMission_GroundPolygonEditRequested;
        }

        // MainV2
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                // setup main serial reader
                SerialReader();
            }
            catch (NotSupportedException ex)
            {
                //log.Error(ex);
            }

            //log.Info("start AutoConnect");
            AutoConnect.NewMavlinkConnection += (sender, serial) =>
            {
                try
                {
                    //log.Info("AutoConnect.NewMavlinkConnection " + serial.PortName);
                    aftGround.BeginInvoke((Action)delegate
                    {
                        if (AFTGround.comPort.BaseStream.IsOpen)
                        {
                            var mav = new MAVLinkInterface();
                            mav.BaseStream = serial;
                            AFTGround.doConnect(mav, "preset", serial.PortName);

                            AFTGround.Comports.Add(mav);

                            try
                            {
                                Comports = Comports.Distinct().ToList();
                            }
                            catch { }
                        }
                        else
                        {
                            AFTGround.comPort.BaseStream = serial;
                            AFTGround.doConnect(AFTGround.comPort, "preset", serial.PortName);
                        }
                    });
                }
                catch (Exception ex)
                {
                    //log.Error(ex);
                }
            };

            try
            {
                object locker = new object();
                List<string> seen = new List<string>();

                ZeroConf.StartUDPMavlink += (zeroconfHost) =>
                {
                    try
                    {
                        var ip = zeroconfHost.IPAddress;
                        var service = zeroconfHost.Services.Where(a => a.Key == "_mavlink._udp.local.");
                        var port = service.First().Value.Port;

                        lock (locker)
                        {
                            if (Comports.Any((a) =>
                            {
                                return a.BaseStream.PortName == "UDPCl" + port.ToString() && a.BaseStream.IsOpen;
                            }
                            ))
                                return;

                            if (seen.Contains(zeroconfHost.Id))
                                return;

                            if (CustomMessageBox.Show(
                                    "A Mavlink stream has been detected, " + zeroconfHost.DisplayName + "(" +
                                    zeroconfHost.Id + "). Would you like to connect to it?",
                                    "Mavlink", System.Windows.Forms.MessageBoxButtons.YesNo) ==
                                (int)System.Windows.Forms.DialogResult.Yes)
                            {
                                var mav = new MAVLinkInterface();

                                if (!comPort.BaseStream.IsOpen)
                                    mav = comPort;

                                var udc = new UdpSerialConnect();
                                udc.Port = port.ToString();
                                udc.client = new UdpClient(ip, port);
                                udc.IsOpen = true;
                                udc.hostEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                                mav.BaseStream = udc;

                                aftGround.Invoke((Action)delegate
                                {
                                    AFTGround.doConnect(mav, "preset", port.ToString());

                                    AFTGround.Comports.Add(mav);

                                    try
                                    {
                                        Comports = Comports.Distinct().ToList();
                                    }
                                    catch { }

                                    //AFTGround._connectionControl.UpdateSysIDS();
                                });

                            }

                            // add to seen list, so we skip on next refresh
                            seen.Add(zeroconfHost.Id);
                        }
                    }
                    catch (Exception)
                    {

                    }
                };

                ZeroConf.ProbeForMavlink();

                ZeroConf.ProbeForRTSP();
            }
            catch
            {
            }

            CommsSerialScan.doConnect += port =>
            {
                if (aftGround.InvokeRequired)
                {
                    //log.Info("CommsSerialScan.doConnect invoke");
                    aftGround.BeginInvoke(
                        (Action)delegate ()
                        {
                            MAVLinkInterface mav = new MAVLinkInterface();
                            mav.BaseStream = port;
                            AFTGround.doConnect(mav, "preset", "0");
                            AFTGround.Comports.Add(mav);

                            try
                            {
                                Comports = Comports.Distinct().ToList();
                            }
                            catch { }
                        });
                }
                else
                {

                    //log.Info("CommsSerialScan.doConnect NO invoke");
                    MAVLinkInterface mav = new MAVLinkInterface();
                    mav.BaseStream = port;
                    AFTGround.doConnect(mav, "preset", "0");
                    AFTGround.Comports.Add(mav);

                    try
                    {
                        Comports = Comports.Distinct().ToList();
                    }
                    catch { }
                }
            };
            /*
            var cmds = ProcessCommandLine(Program.args);

            if (cmds.ContainsKey("rtk"))
            {
                var inject = new ConfigSerialInjectGPS();
                if (cmds["rtk"].ToLower().Contains("http"))
                {
                    inject.CMB_serialport.Text = "NTRIP";
                    var nt = new CommsNTRIP();
                    ConfigSerialInjectGPS.comPort = nt;
                    Task.Run(() =>
                    {
                        try
                        {
                            nt.Open(cmds["rtk"]);
                            nt.lat = AFTGround.comPort.MAV.cs.PlannedHomeLocation.Lat;
                            nt.lng = AFTGround.comPort.MAV.cs.PlannedHomeLocation.Lng;
                            nt.alt = AFTGround.comPort.MAV.cs.PlannedHomeLocation.Alt;
                            this.BeginInvokeIfRequired(() => { inject.DoConnect(); });
                        }
                        catch (Exception ex)
                        {
                            this.BeginInvokeIfRequired(() => { CustomMessageBox.Show(ex.ToString()); });
                        }
                    });
                }
            }*/
        }
        /*
        private void CMB_baudrate_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(_connectionControl.CMB_baudrate.Text, out comPortBaud))
            {
                CustomMessageBox.Show(Strings.InvalidBaudRate, Strings.ERROR);
                return;
            }

            var sb = new StringBuilder();
            int baud = 0;
            for (int i = 0; i < _connectionControl.CMB_baudrate.Text.Length; i++)
                if (char.IsDigit(_connectionControl.CMB_baudrate.Text[i]))
                {
                    sb.Append(_connectionControl.CMB_baudrate.Text[i]);
                    baud = baud * 10 + _connectionControl.CMB_baudrate.Text[i] - '0';
                }

            if (_connectionControl.CMB_baudrate.Text != sb.ToString())
            {
                _connectionControl.CMB_baudrate.Text = sb.ToString();
            }

            try
            {
                if (baud > 0 && comPort.BaseStream.BaudRate != baud)
                    comPort.BaseStream.BaudRate = baud;
            }
            catch (Exception)
            {
            }
        }
        */
        private void AFTGround_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map
            bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;

            // Set firmware
            AFTGround.comPort.MAV.cs.firmware = Firmwares.ArduRover;
        }

        private void MyMap_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Set starting map location and zoom level of map
            bingMapsUserControl1.myMap.SetView(locationStart, zoomStart);
        }

        private void aftNewMission_GroundPolygonEditRequested(object sender, PolygonEventArgs e)
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
            System.Windows.Point mousePosition = e.GetPosition(null);
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
            _mouseToMarker = System.Windows.Point.Subtract(
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
                    SelectedPushpin.Location = bingMapsUserControl1.myMap.ViewportPointToLocation(System.Windows.Point.Add(e.GetPosition(bingMapsUserControl1.myMap), _mouseToMarker));
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
            // Show new mission screen
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

        private async void btnFlightLines_Click(object sender, EventArgs e)
        {
            var mapIdx = aftGround.Controls.GetChildIndex(elementHost1);
            var bmapIdx = aftGround.Controls.GetChildIndex(picFlightLines);

            if (mapIdx < bmapIdx)
            {
                await DrawDistanceBitmap();

                picFlightLines.Dock = DockStyle.Fill;

                elementHost1.SendToBack();
                aftGround.Controls.SetChildIndex(picFlightLines, mapIdx);
            }
            else
            {
                picFlightLines.SendToBack();
                aftGround.Controls.SetChildIndex(elementHost1, bmapIdx);
            }
        }

        private void btnVidDownlink_Click(object sender, EventArgs e)
        {
            /*Switch to video downlink*/
        }
    }
}
