using MissionPlanner.Comms;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using static MissionPlanner.AFTController;
using static MissionPlanner.AFTGround;

namespace MissionPlanner
{
    public partial class AFTVehicleConnecting : Form
    {
        public AFTVehicleConnecting()
        {
            InitializeComponent();
        }

        private void AFTVehicleConnecting_Load(object sender, EventArgs e)
        {
            //Connect drone
            try
            {
                // Check if ground or air
                if (aftGround != null)
                {
                    // setup main serial reader
                    SerialReaderGround();
                }
                else if (aftAir != null)
                {
                    // setup main serial reader
                    //SerialReaderAir();
                }
                else { }

            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            // Start plugin thread
            try
            {
                // setup main plugin thread
                pluginthread = new Thread(PluginThread)
                {
                    IsBackground = true,
                    Name = "plugin runner thread",
                    Priority = ThreadPriority.BelowNormal
                };
                pluginthread.Start();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            //log.Info("start AutoConnect");
            AutoConnect.NewMavlinkConnection += (send, serial) =>
            {
                try
                {
                    // AutoConnect.NewMavlinkConnection serial.PortName
                    aftGround.BeginInvoke((Action)delegate
                    {
                        if (AFTController.comPort.BaseStream.IsOpen)
                        {
                            var mav = new MAVLinkInterface();
                            mav.BaseStream = serial;

                            // Check if ground or air
                            if (aftGround != null)
                            {
                                AFTGround._doConnect(mav, "preset", serial.PortName);
                            }
                            else if (aftAir != null)
                            {
                                //AFTAir._doConnect(mav, "preset", serial.PortName);
                            }
                            else { }

                            AFTController.Comports.Add(mav);

                            try
                            {
                                Comports = Comports.Distinct().ToList();
                            }
                            catch { }
                        }
                        else
                        {
                            AFTController.comPort.BaseStream = serial;

                            // Check if ground or air
                            if (aftGround != null)
                            {
                                AFTGround._doConnect(AFTController.comPort, "preset", serial.PortName);
                            }
                            else if (aftAir != null)
                            {
                                //AFTAir._doConnect(AFTController.comPort, "preset", serial.PortName);
                            }
                            else { }
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            };
            AutoConnect.Start();

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

                            // Connect to vehicle
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
                                // Check if ground or air
                                if (aftGround != null)
                                {
                                    AFTGround._doConnect(mav, "preset", port.ToString());
                                }
                                else if (aftAir != null)
                                {
                                    //AFTAir._doConnect(mav, "preset", port.ToString());
                                }
                                else { }

                                AFTController.Comports.Add(mav);

                                try
                                {
                                    Comports = Comports.Distinct().ToList();
                                }
                                catch { }
                            });

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
                    // CommsSerialScan.doConnect invoke
                    aftGround.BeginInvoke(
                        (Action)delegate ()
                        {
                            MAVLinkInterface mav = new MAVLinkInterface();
                            mav.BaseStream = port;

                            // Check if ground or air
                            if (aftGround != null)
                            {
                                AFTGround._doConnect(mav, "preset", "0");
                            }
                            else if (aftAir != null)
                            {
                                //AFTAir._doConnect(mav, "preset", "0");
                            }
                            else { }

                            AFTController.Comports.Add(mav);

                            try
                            {
                                Comports = Comports.Distinct().ToList();
                            }
                            catch { }
                        });
                }
                else
                {

                    // CommsSerialScan.doConnect NO invoke
                    MAVLinkInterface mav = new MAVLinkInterface();
                    mav.BaseStream = port;

                    // Check if ground or air
                    if (aftGround != null)
                    {
                        AFTGround._doConnect(mav, "preset", "0");
                    }
                    else if (aftAir != null)
                    {
                        //AFTAir._doConnect(mav, "preset", "0");
                    }
                    else { }

                    AFTController.Comports.Add(mav);

                    try
                    {
                        Comports = Comports.Distinct().ToList();
                    }
                    catch { }
                }
            };

            // Show pre-flight checklist
            if ((checklist == null) || checklist.IsDisposed)
            {
                checklist = new AFTChecklist();
            }

            checklist.Show();
            checklist.BringToFront();
            this.Close();
        }
    }
}
