using Microsoft.Maps.MapControl.WPF;
using MissionPlanner.ArduPilot;
using MissionPlanner.ArduPilot.Mavlink;
using MissionPlanner.Comms;
using MissionPlanner.Controls;
using MissionPlanner.GCSViews.ConfigurationView;
using MissionPlanner.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static MissionPlanner.AFTController;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using Timer = System.Windows.Forms.Timer;

namespace MissionPlanner
{
    public partial class AFTGround : Form
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

        // Keep here since it references private functions
        public static void _doConnect(MAVLinkInterface comPort, string portname, string baud, bool getparams = true, bool showui = true)
        {
            bool skipconnectcheck = false;
            Console.WriteLine($"Connecting to {portname} {baud}");

            switch (portname)
            {
                case "preset":
                    skipconnectcheck = true;
                    break;
                case "TCP":
                    comPort.BaseStream = new TcpSerial();
                    break;
                case "UDP":
                    comPort.BaseStream = new UdpSerial();
                    break;
                case "WS":
                    comPort.BaseStream = new WebSocket();
                    break;
                case "UDPCl":
                    comPort.BaseStream = new UdpSerialConnect();
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
            comPort.MAV.cs.ResetInternals();

            //cleanup any log being played
            comPort.logreadmode = false;
            if (comPort.logplaybackfile != null)
                comPort.logplaybackfile.Close();
            comPort.logplaybackfile = null;

            try
            {
                // Set port name, then options
                if (portname.ToLower() != "preset")
                    comPort.BaseStream.PortName = portname;

                // Set baudrate
                try
                {
                    if (baud != "" && baud != "0" && baud.IsNumber())
                        comPort.BaseStream.BaudRate = int.Parse(baud);
                }
                catch (Exception exp)
                {
                    Console.WriteLine($"Error: {exp.Message}");
                }

                // prevent serialreader from doing anything
                comPort.giveComport = true;

                // Do dtr if needed
                // reset on connect logic.
                if (Settings.Instance.GetBoolean("CHK_resetapmonconnect") == true)
                {
                    //log.Info("set dtr rts to false");
                    comPort.BaseStream.DtrEnable = false;
                    comPort.BaseStream.RtsEnable = false;

                    comPort.BaseStream.toggleDTR();
                }

                comPort.giveComport = false;

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
                    if (File.Exists(comPort.MAV.ParamCachePath) &&
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

                // Check for newer firmware
                if (showui)
                    Task.Run(() =>
                    {
                        try
                        {
                            string[] fields1 = comPort.MAV.VersionString.Split(' ');

                            var softwares = APFirmware.GetReleaseNewest(APFirmware.RELEASE_TYPES.OFFICIAL);

                            foreach (var item in softwares)
                            {
                                // check primary firmware type. ie arudplane, arducopter
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
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    });

                // Get any rallypoints
                if (AFTController.comPort.MAV.param.ContainsKey("RALLY_TOTAL") &&
                    int.Parse(AFTController.comPort.MAV.param["RALLY_TOTAL"].ToString()) > 0 && showui)
                {
                    try
                    {
                        if ((AFTController.comPort.MAV.cs.capabilities & (uint)MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_RALLY) >= 0)
                        {
                            mav_mission.download(AFTController.comPort, AFTController.comPort.MAV.sysid, AFTController.comPort.MAV.compid,
                                MAVLink.MAV_MISSION_TYPE.RALLY).AwaitSync();
                            return;
                        }

                        if (AFTController.comPort.MAV.param["RALLY_TOTAL"] == null || int.Parse(AFTController.comPort.MAV.param["RALLY_TOTAL"].ToString()) < 1)
                        {
                            Console.WriteLine("Not Supported or Nothing to Download");
                            return;
                        }

                        // Reset layers
                        aftGround._initializeMapChildren();

                        // Total # of rally points
                        int count = int.Parse(AFTController.comPort.MAV.param["RALLY_TOTAL"].ToString());

                        for (int a = 0; a < (count); a++)
                        {
                            // Update rally point, aka home location (only 1 is supported at this time)
                            // It will overwrite the first polygon location with every rally point,
                            // leaving the last rally point as the new home location
                            try
                            {
                                var plla = AFTController.comPort.getRallyPoint(a).AwaitSync();
                                count = plla.total;

                                Location pll = new Location(plla.plla.Lat, plla.plla.Lng);
                                aftGround._updatePolygonBoundary(pll);

                                // Leave until more rally points supported
                                Console.WriteLine("Only 1 rally point supported at this time; see doConnect() in AFTController.cs");
                            }
                            catch
                            {
                                Console.WriteLine("Failed to get rally point", Strings.ERROR);
                                return;
                            }
                        }

                        // Set home position
                        var newHomeLat = newPolygon.Locations[0].Latitude;
                        var newHomeLng = newPolygon.Locations[0].Longitude;
                        var newHomeAlt = newPolygon.Locations[0].Altitude;

                        PointLatLngAlt newDroneHome = new PointLatLngAlt(newHomeLat, newHomeLng, newHomeAlt);
                        AFTController.comPort.MAV.cs.PlannedHomeLocation = newDroneHome;

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
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                // Get any fences
                if (AFTController.comPort.MAV.param.ContainsKey("FENCE_TOTAL") &&
                    int.Parse(AFTController.comPort.MAV.param["FENCE_TOTAL"].ToString()) > 1 &&
                    AFTController.comPort.MAV.param.ContainsKey("FENCE_ACTION") && showui)
                {
                    try
                    {
                        //bool polygongridmode = false;
                        int count = 1;

                        if ((MainV2.comPort.MAV.cs.capabilities & (uint)MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_FENCE) > 0)
                        {
                            mav_mission.download(MainV2.comPort, MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid,
                                    MAVLink.MAV_MISSION_TYPE.FENCE).AwaitSync();
                        }

                        if (MainV2.comPort.MAV.param["FENCE_ACTION"] == null || MainV2.comPort.MAV.param["FENCE_TOTAL"] == null)
                        {
                            Console.WriteLine("Not Supported");
                            return;
                        }

                        if (int.Parse(MainV2.comPort.MAV.param["FENCE_TOTAL"].ToString()) <= 1)
                        {
                            Console.WriteLine("Nothing to download");
                            return;
                        }

                        // Reset layers
                        aftGround._initializeMapChildren();

                        for (int a = 0; a < count; a++)
                        {
                            // Update polygon with fence points
                            try
                            {
                                var plla = MainV2.comPort.getFencePoint(a).AwaitSync();
                                count = plla.total;

                                Location pll = new Location(plla.plla.Lat, plla.plla.Lng);
                                aftGround._updatePolygonBoundary(pll);
                            }
                            catch
                            {
                                Console.WriteLine("Failed to get fence point", Strings.ERROR);
                                return;
                            }
                        }

                        // Set new home location
                        var newHomeLat = newPolygon.Locations[0].Latitude;
                        var newHomeLng = newPolygon.Locations[0].Longitude;
                        var newHomeAlt = newPolygon.Locations[0].Altitude;

                        PointLatLngAlt newDroneHome = new PointLatLngAlt(newHomeLat, newHomeLng, newHomeAlt);
                        AFTController.comPort.MAV.cs.PlannedHomeLocation = newDroneHome;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                // Drone should now be connected
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                try
                {
                    //_connectionControl.IsConnected(false);
                    //UpdateConnectIcon();
                    comPort.Close();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Error: {ex2.Message}");
                }

                Console.WriteLine($"Can not establish a connection\n\n{ex.Message}");
                return;
            }
        }

        public static async void SerialReaderGround()
        {
            if (serialThread == true)
                return;
            serialThread = true;

            SerialThreadrunner.Reset();

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
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    // 30 seconds interval speech options
                    if (speechEnabled() && speechEngine != null && (DateTime.Now - speechcustomtime).TotalSeconds > 30 &&
                        (AFTController.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (AFTController.speechEngine.IsReady)
                        {
                            if (Settings.Instance.GetBoolean("speechcustomenabled"))
                            {
                                AFTController.speechEngine.SpeakAsync(ArduPilot.Common.speechConversion(comPort.MAV,
                                    "" + Settings.Instance["speechcustom"]));
                            }

                            speechcustomtime = DateTime.Now;
                        }

                        // Speech for battery alerts
                        // speechbatteryvolt
                        float warnvolt = Settings.Instance.GetFloat("speechbatteryvolt");
                        float warnpercent = Settings.Instance.GetFloat("speechbatterypercent");

                        if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                            AFTController.comPort.MAV.cs.battery_voltage <= warnvolt &&
                            AFTController.comPort.MAV.cs.battery_voltage >= 5.0)
                        {
                            if (AFTController.speechEngine.IsReady)
                            {
                                AFTController.speechEngine.SpeakAsync(ArduPilot.Common.speechConversion(comPort.MAV,
                                    "" + Settings.Instance["speechbattery"]));
                            }
                        }
                        else if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                                 (AFTController.comPort.MAV.cs.battery_remaining) < warnpercent &&
                                 AFTController.comPort.MAV.cs.battery_voltage >= 5.0 &&
                                 AFTController.comPort.MAV.cs.battery_remaining != 0.0)
                        {
                            if (AFTController.speechEngine.IsReady)
                            {
                                AFTController.speechEngine.SpeakAsync(
                                    ArduPilot.Common.speechConversion(comPort.MAV,
                                        "" + Settings.Instance["speechbattery"]));
                            }
                        }
                    }

                    // speech for airspeed alerts
                    if (speechEnabled() && speechEngine != null && (DateTime.Now - speechlowspeedtime).TotalSeconds > 10 &&
                        (AFTController.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (Settings.Instance.GetBoolean("speechlowspeedenabled") == true &&
                            AFTController.comPort.MAV.cs.armed)
                        {
                            float warngroundspeed = Settings.Instance.GetFloat("speechlowgroundspeedtrigger");
                            float warnairspeed = Settings.Instance.GetFloat("speechlowairspeedtrigger");

                            if (AFTController.comPort.MAV.cs.airspeed < warnairspeed)
                            {
                                if (AFTController.speechEngine.IsReady)
                                {
                                    AFTController.speechEngine.SpeakAsync(
                                        ArduPilot.Common.speechConversion(comPort.MAV,
                                            "" + Settings.Instance["speechlowairspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else if (AFTController.comPort.MAV.cs.groundspeed < warngroundspeed)
                            {
                                if (AFTController.speechEngine.IsReady)
                                {
                                    AFTController.speechEngine.SpeakAsync(
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
                        (AFTController.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        float warnalt = float.MaxValue;
                        if (Settings.Instance.ContainsKey("speechaltheight"))
                        {
                            warnalt = Settings.Instance.GetFloat("speechaltheight");
                        }

                        try
                        {
                            altwarningmax = (int)Math.Max(AFTController.comPort.MAV.cs.alt, altwarningmax);

                            if (Settings.Instance.GetBoolean("speechaltenabled") == true &&
                                AFTController.comPort.MAV.cs.alt != 0.00 &&
                                (AFTController.comPort.MAV.cs.alt <= warnalt) && AFTController.comPort.MAV.cs.armed)
                            {
                                if (altwarningmax > warnalt)
                                {
                                    if (AFTController.speechEngine.IsReady)
                                        AFTController.speechEngine.SpeakAsync(
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
                            if (AFTController.speechEngine.IsReady &&
                                lastmessagehigh != AFTController.comPort.MAV.cs.messageHigh &&
                                AFTController.comPort.MAV.cs.messageHigh != null)
                            {
                                if (!AFTController.comPort.MAV.cs.messageHigh.StartsWith("PX4v2 ") &&
                                    !AFTController.comPort.MAV.cs.messageHigh.StartsWith("PreArm:")) // Supress audibly repeating PreArm messages
                                {
                                    AFTController.speechEngine.SpeakAsync(AFTController.comPort.MAV.cs.messageHigh);
                                    lastmessagehigh = AFTController.comPort.MAV.cs.messageHigh;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    // not doing anything
                    if (!AFTController.comPort.logreadmode && !comPort.BaseStream.IsOpen)
                    {
                        altwarningmax = 0;
                    }

                    // attenuate the link qualty over time
                    if ((DateTime.Now - AFTController.comPort.MAV.lastvalidpacket).TotalSeconds >= 1)
                    {
                        if (linkqualitytime.Second != DateTime.Now.Second)
                        {
                            AFTController.comPort.MAV.cs.linkqualitygcs =
                                (ushort)(AFTController.comPort.MAV.cs.linkqualitygcs * 0.8f);
                            linkqualitytime = DateTime.Now;

                            // force redraw if there are no other packets are being read
                            // Is this needed for AFTGround?
                            /*this.BeginInvokeIfRequired(
                                (Action)
                                delegate { GCSViews.FlightData.myhud.Invalidate(); });*/
                        }
                    }

                    // Data loss warning - wait min of 3 seconds, ignore first 30 seconds of connect, repeat at 5 seconds interval
                    if ((DateTime.Now - AFTController.comPort.MAV.lastvalidpacket).TotalSeconds > 3
                        && (DateTime.Now - connecttime).TotalSeconds > 30
                        && (DateTime.Now - nodatawarning).TotalSeconds > 5
                        && (AFTController.comPort.logreadmode || comPort.BaseStream.IsOpen)
                        && AFTController.comPort.MAV.cs.armed)
                    {
                        var msg = "WARNING No Data for " + (int)(DateTime.Now - AFTController.comPort.MAV.lastvalidpacket).TotalSeconds + " Seconds";
                        AFTController.comPort.MAV.cs.messageHigh = msg;
                        if (speechEnabled() && speechEngine != null)
                        {
                            if (AFTController.speechEngine.IsReady)
                            {
                                AFTController.speechEngine.SpeakAsync(msg);
                                nodatawarning = DateTime.Now;
                            }
                        }
                    }

                    // Get home point on armed status change
                    if (armedstatus != AFTController.comPort.MAV.cs.armed && comPort.BaseStream.IsOpen)
                    {
                        armedstatus = AFTController.comPort.MAV.cs.armed;
                        // status just changed to armed
                        if (AFTController.comPort.MAV.cs.armed == true &&
                            AFTController.comPort.MAV.apname != MAVLink.MAV_AUTOPILOT.INVALID &&
                            AFTController.comPort.MAV.aptype != MAVLink.MAV_TYPE.GIMBAL)
                        {
                            System.Threading.ThreadPool.QueueUserWorkItem(state =>
                            {
                                Thread.CurrentThread.Name = "Arm State change";
                                try
                                {
                                    while (comPort.giveComport == true)
                                        Thread.Sleep(100);

                                    AFTController.comPort.MAV.cs.HomeLocation = new PointLatLngAlt(AFTController.comPort.getWP(0));
                                    if (aftGround != null)
                                    {
                                        // Update home location and pushpin; assumes that polygon is already created and has at least one point
                                        newPolygon.Locations[0] = new Location(AFTController.comPort.MAV.cs.HomeLocation.Lat, AFTController.comPort.MAV.cs.HomeLocation.Lng);
                                        if (polygonPointLayer.Children[0] is Pushpin)
                                        {
                                            Pushpin p = (Pushpin)polygonPointLayer.Children[0];
                                            p.Location = newPolygon.Locations[0];
                                        }
                                    }
                                }
                                catch
                                {
                                    // Don't hang this loop
                                    aftGround.BeginInvokeIfRequired(
                                        (Action)
                                        delegate
                                        {
                                            CustomMessageBox.Show("Failed to update home location (" +
                                                                  AFTController.comPort.MAV.sysid + ")");
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
                                    AFTController.speechEngine.SpeakAsync(
                                        ArduPilot.Common.speechConversion(comPort.MAV, speech));
                                }
                            }
                        }
                    }

                    if (comPort.MAV.param.TotalReceived < comPort.MAV.param.TotalReported)
                    {
                        if (comPort.MAV.param.TotalReported > 0 && comPort.BaseStream.IsOpen)
                        {
                            aftGround.BeginInvokeIfRequired(() =>
                            {
                                try
                                {
                                    if (aftGround != null)
                                    {
                                        aftGround.status1.Percent =
                                        (comPort.MAV.param.TotalReceived / (double)comPort.MAV.param.TotalReported) * 100.0;
                                    }
                                    else if (aftAir != null)
                                    {
                                        aftAir.status1.Percent =
                                        (comPort.MAV.param.TotalReceived / (double)comPort.MAV.param.TotalReported) * 100.0;
                                    }
                                    else { }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Error: {e.Message}");
                                }
                            });
                        }
                    }

                    // Send a hb every seconds from gcs to ap
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
                                    Console.WriteLine($"Error: {ex.Message}");

                                    // Close the bad port
                                    try
                                    {
                                        port.Close();
                                    }
                                    catch
                                    {
                                    }
                                    /*
                                    // refresh the screen if needed
                                    if (port == AFTController.comPort)
                                    {
                                        // refresh config window if needed
                                        if (MyView.current != null)
                                        {
                                            this.BeginInvoke((MethodInvoker)delegate ()
                                            {
                                                if (MyView.current.Name == "HWConfig")
                                                    MyView.ShowScreen("HWConfig");
                                                if (MyView.current.Name == "SWConfig")
                                                    MyView.ShowScreen("SWConfig");
                                            });
                                        }
                                    }*/
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
                                Console.WriteLine($"Error: {ex.Message}");
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
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Tracking.AddException(e);
                    Console.WriteLine("Serial Reader fail :" + e.ToString());

                    try
                    {
                        comPort.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("SerialReader Done");
            SerialThreadrunner.Set();
        }

        /// <summary>
        /// Update telemetry display with new values
        /// </summary>
        private void _updateTelemetryData()
        {
            if (comPort.BaseStream.IsOpen)
            {
                // Get the latest telemetry values from MAVLinkInterface
                double altitude = comPort.MAV.cs.alt;
                double groundSpeed = comPort.MAV.cs.groundspeed;
                double distanceToWaypoint = comPort.MAV.cs.wp_dist;
                double yaw = comPort.MAV.cs.yaw;
                double verticalSpeed = comPort.MAV.cs.climbrate;
                double distanceToMAV = comPort.MAV.cs.DistToHome;

                // Update the UI controls with telemetry data
                lblAltDisplay.Text = altitude.ToString("F2");
                lblGSpdDisplay.Text = groundSpeed.ToString("F2");
                lblWPDistDisplay.Text = distanceToWaypoint.ToString("F2");
                lblYawDisplay.Text = yaw.ToString("F2");
                lblVSpdDisplay.Text = verticalSpeed.ToString("F2");
                lblMAVDistDisplay.Text = distanceToMAV.ToString("F2");
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
            if (newPolygon.Locations.Count() > 0)
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
            InitializeComponent();

            // Define default basestream
            comPort.BaseStream = new SerialPort();
            comPort.BaseStream.BaudRate = 57600;

            Comports.Add(comPort);
            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            // Initialize timer
            updateTimer = new Timer();
            updateTimer.Interval = 500; // Update every 500 milliseconds (adjust as needed)
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

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

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            _updateTelemetryData();

            if (comPort.BaseStream.IsOpen && comPort.MAV.cs.Location != null)
            {
                // Current drone location
                var currentDroneLoc = new Location(comPort.MAV.cs.lat, comPort.MAV.cs.lng);

                // Lazy way to set map location once to initial flight controller location
                if (bingMapsUserControl1.myMap.Center == locationStart)
                {
                    bingMapsUserControl1.myMap.Center = currentDroneLoc;
                }

                // Display pushpin representing drone location
                if (droneLocationLayer != null)
                {
                    bingMapsUserControl1.myMap.Children.Remove(droneLocationLayer);
                }

                droneLocationLayer = new MapLayer();
                bingMapsUserControl1.myMap.Children.Add(droneLocationLayer);

                Pushpin dronePushPin = new Pushpin();
                dronePushPin.Width = 16;
                dronePushPin.Height = 16;
                dronePushPin.Background = new SolidColorBrush(System.Windows.Media.Colors.ForestGreen);

                polygonPointLayer.AddChild(dronePushPin, currentDroneLoc);

            }
        }

        private void AFTGround_Load(object sender, EventArgs e)
        {
            // Set animation level of Bing map
            bingMapsUserControl1.myMap.AnimationLevel = AnimationLevel.Full;

            // Set firmware
            comPort.MAV.cs.firmware = Firmwares.ArduRover;
            /* update telem
            // Start the UDP listener to receive MAVLink packets
            udpClient = new UdpClient(udpPort);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);*/
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

            // Set drone home location
            var homeLat = newPolygon.Locations[0].Latitude;
            var homeLng = newPolygon.Locations[0].Longitude;
            var homeAlt = newPolygon.Locations[0].Altitude;

            PointLatLngAlt droneHome = new PointLatLngAlt(homeLat, homeLng, homeAlt);
            comPort.MAV.cs.PlannedHomeLocation = droneHome;
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

        void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            // try prevent crash on resume
            if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
            {
                doDisconnect(AFTController.comPort);
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            // Hide menu panel
            if (aftGround.Controls.GetChildIndex(sideMenuPanel) == 0)
            {
                sideMenuPanel.Dock = DockStyle.None;
                sideMenuPanel.SendToBack();
            }
            // Show menu panel
            else
            {
                sideMenuPanel.Dock = DockStyle.Left;
                aftGround.Controls.SetChildIndex(sideMenuPanel, 0);
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

        private void btnFlightLines_Click(object sender, EventArgs e)
        {
            if (pathPolyline == null)
            {
                LocationCollection shortestPath = CalculateShortestPathHome().Result;

                pathPolyline = new MapPolyline();
                pathPolyline.Locations = shortestPath;

                // Customize the polyline appearance (color and thickness)
                pathPolyline.Stroke = new SolidColorBrush(missionBoundaryColor);
                pathPolyline.StrokeThickness = 3;

                // Add the polyline to the map
                bingMapsUserControl1.myMap.Children.Add(pathPolyline);

                // Set the map view to include the MAV and home locations
                LocationRect rect = new LocationRect(shortestPath);
                //bingMapsUserControl1.myMap.SetView(rect);
            }
            else
            {
                bingMapsUserControl1.myMap.Children.Remove(pathPolyline);
                pathPolyline = null;
                //bingMapsUserControl1.myMap.SetView(previousView);
            }

        }

        private void btnVidDownlink_Click(object sender, EventArgs e)
        {
            var mapIdx = aftGround.Controls.GetChildIndex(elementHost1);
            var vidIdx = aftGround.Controls.GetChildIndex(picVidDownlink);

            if (mapIdx < vidIdx)
            {
                AutoConnect.NewVideoStream += (send, gststring) =>
                {
                    try
                    {
                        Console.WriteLine("AutoConnect.NewVideoStream " + gststring);
                        GStreamer.gstlaunch = GStreamer.LookForGstreamer();

                        if (!GStreamer.gstlaunchexists)
                        {
                            GStreamerUI.DownloadGStreamer();
                            if (!GStreamer.gstlaunchexists)
                            {
                                return;
                            }
                        }

                        GStreamer.StartA(gststring);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");
                    }
                };
                AutoConnect.Start();

                // debound based on url
                List<string> videourlseen = new List<string>();
                // prevent spaming the ui
                SemaphoreSlim videodetect = new SemaphoreSlim(1);

                CameraProtocol.OnRTSPDetected += (send, s) =>
                {
                    if (isHerelink)
                    {
                        return;
                    }

                    if (!videourlseen.Contains(s) && videodetect.Wait(0))
                    {
                        videourlseen.Add(s);
                        AutoConnect.RaiseNewVideoStream(sender,
                                String.Format(
                                    "rtspsrc location={0} latency=41 udp-reconnect=1 timeout=0 do-retransmission=false ! application/x-rtp ! decodebin3 ! queue leaky=2 ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink sync=false",
                                    s));

                        videodetect.Release();
                    }
                };
                // When receive a new frame from GStreamer
                GStreamer.onNewImage += (send, image) =>
                {
                    try
                    {
                        if (image == null)
                        {
                            picVidDownlink.Image = null;
                            return;
                        }

                        var old = picVidDownlink.Image;
                        picVidDownlink.Image = new Bitmap(image.Width, image.Height, 4 * image.Width,
                            System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                            image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888)
                                .Scan0);
                        if (old != null)
                            old.Dispose();
                    }
                    catch
                    {
                    }
                };
                // When receive a new frame from video rendered with VLC (VideoLAN Client)
                vlcrender.onNewImage += (send, image) =>
                {
                    try
                    {
                        if (image == null)
                        {
                            picVidDownlink.Image = null;
                            return;
                        }

                        var old = picVidDownlink.Image;
                        picVidDownlink.Image = new Bitmap(image.Width,
                            image.Height,
                            4 * image.Width,
                            System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                            image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888).Scan0);
                        if (old != null)
                            old.Dispose();
                    }
                    catch
                    {
                    }
                };
                // When receive a new frame from video captured using MJPEG (Motion JPEG) format
                CaptureMJPEG.onNewImage += (send, image) =>
                {
                    try
                    {
                        if (image == null)
                        {
                            picVidDownlink.Image = null;
                            return;
                        }

                        var old = picVidDownlink.Image;
                        picVidDownlink.Image = new Bitmap(image.Width, image.Height, 4 * image.Width,
                            System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                            image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888).Scan0);
                        if (old != null)
                            old.Dispose();
                    }
                    catch
                    {
                    }
                };

                // Switch views
                elementHost1.Dock = DockStyle.None;
                elementHost1.SendToBack();

                aftGround.Controls.SetChildIndex(picVidDownlink, mapIdx);
                picVidDownlink.Dock = DockStyle.Fill;
            }
            else
            {
                // Switch views
                picVidDownlink.Dock = DockStyle.None;
                picVidDownlink.SendToBack();

                aftGround.Controls.SetChildIndex(elementHost1, vidIdx);
                elementHost1.Dock = DockStyle.Fill;

                // Stop video downlink
                Console.WriteLine("Stopping GStreamer");
                GStreamer.StopAll();

                Console.WriteLine("Closing vlcrender");
                try
                {
                    while (vlcrender.store.Count > 0)
                        vlcrender.store[0].Stop();
                }
                catch
                {
                }
            }
        }

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

            // Stop adsb
            Utilities.adsb.Stop();

            // Stop WarningEngine
            Warnings.WarningEngine.Stop();

            // Stop GStreamer
            GStreamer.StopAll();

            // Close vlcrender
            try
            {
                while (vlcrender.store.Count > 0)
                    vlcrender.store[0].Stop();
            }
            catch
            {
            }

            // Close pluginthread
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

            // Close serialthread
            serialThread = false;

            // Close httpthread

            // if we are waiting on a socket we need to force an abort
            httpserver.Stop();

            // Close all forms
            //MyView.Dispose();

            try
            {
                if (comPort.BaseStream.IsOpen)
                    comPort.Close();
            }
            catch
            {
            } // i get alot of these errors, the port is still open, but not valid - user has unpluged usb

            //Console.WriteLine(httpthread?.IsAlive);
            Console.WriteLine(pluginthread?.IsAlive);

            Console.WriteLine("AFTGround_FormClosing done");

            //if (MONO)
            //this.Dispose();
        }
    }
}
