using ELM327_PID_DataCollector.Helpers;
using ELM327_PID_DataCollector.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ELM327_PID_DataCollector
{
    public class Elm327wifi
    {
        private string ip { get; set; }
        private int port { get; set; }
        private TcpClientOBD client;
        private AutoResetEvent arEvent;
        private AutoResetEvent dataReceivedEvent;
        public int totalAvailablePIDcount = 0;
        public List<string> PIDlist = new List<string>();
        private List<PIDvalue> pidValues = new List<PIDvalue>();

        private enum Mode
        {
            PIDdetector,
            dataCollector,
            freeMode
        };

        private Mode mode;
        private bool forceStop = false;

        public Elm327wifi(string ip,int port)
        {
            pidValues = Helpers.HelperTool.ReadJsonConfiguration(Helpers.HelperTool.ReadResource("PID_Values.json"));
            this.ip= ip;
            this.port= port;
        }

        public void Start()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("1- Get available PIDs");
            Console.WriteLine("2- Get Current Vehicle Data");
            Console.WriteLine("3- Free Mode (Pre-Configured)");
            Console.WriteLine("***************************");
            Console.WriteLine("Enter your option:");
            var userOutput = Console.ReadLine();
            forceStop = false;
            switch (userOutput)
            {
                case "1":
                    mode = Mode.PIDdetector;
                    arEvent = new AutoResetEvent(false);
                    GetAvailablePIDs();
                    Console.ReadKey();
                    break;
                case "2":
                    mode = Mode.dataCollector;
                    dataReceivedEvent = new AutoResetEvent(false);
                    GetVehicleData();
                    Console.ReadKey();
                    break;
                case "3":
                    mode = Mode.freeMode;
                    dataReceivedEvent = new AutoResetEvent(false);
                    StartFreeMode();
                    break;
                default:
                    break;
            }
        }

        private void GetAvailablePIDs()
        {
            Console.WriteLine("...");
            Console.WriteLine("Press any Key to Stop");
            Console.WriteLine("...");

            client = new TcpClientOBD(ip, port);

            client.PidMessageArrived += Client_PidMessageArrived;
            client.OBDdeviceReady += Client_OBDdeviceReady;

            client.StartOBDdev();
        }

        private void GetVehicleData()
        {
            Console.WriteLine("...");
            Console.WriteLine("Press any Key to Stop");
            Console.WriteLine("...");

            client = new TcpClientOBD(ip, port);

            client.PidMessageArrived += Client_PidMessageArrived;
            client.OBDdeviceReady += Client_OBDdeviceReady;

            client.StartOBDdev();
        }

        private void StartFreeMode()
        {
            Console.WriteLine("...");
            Console.WriteLine("Confifuration being Set ...");
            Console.WriteLine("...");

            client = new TcpClientOBD(ip, port);

            
            client.OBDdeviceReady += Client_OBDdeviceReady;

            client.PidMessageArrived += Client_PidMessageArrived;

            client.StartOBDdevFreeMode();
            


        }

        public void Stop()
        {
            forceStop= true;
            client.Stop();
            client.Dispose();

            client.PidMessageArrived -= Client_PidMessageArrived;
            client.OBDdeviceReady -= Client_OBDdeviceReady;
        }

        private void Client_OBDdeviceReady()
        {
            Console.WriteLine("OBD device is ready");

            switch (mode)
            {
                case Mode.PIDdetector:
                    Task.Run(() =>
                    {
                        Console.WriteLine("OBDII port is being analyzed... Please Wait...");
                        var modes = new string[] { "01", "02", "03", "09" };
                        for (int i = 1; i < 256; i++)
                        {
                            foreach (var mode in modes)
                            {
                                client.SendRequest(DecToHex(i.ToString()), mode);
                                arEvent.WaitOne(1000);
                            }
                        }
                        Console.WriteLine("Total available pid value count : " + totalAvailablePIDcount);
                        Console.WriteLine();
                        Console.WriteLine("********************************");
                        foreach (var i in PIDlist)
                        {
                            if (!i.Contains("41")) continue;
                            var ValExceptSpaces = i.Replace(" ","");
                            var pidValHex = ValExceptSpaces.Substring(2, 2);
                            var pidVal = pidValues.Where(x => x.PIDhex == pidValHex).LastOrDefault();
                            if (pidVal != null)
                            {
                                Console.WriteLine("PID Name: " + pidVal.Name +" ----- "+" PID Unit: " + pidVal.Unit);
                                Console.WriteLine("//////////////");
                            }
                            else
                            {
                                Console.WriteLine("value is null");
                            }
                        }
                        Console.WriteLine("********************************");
                        PIDlist.Clear();
                        totalAvailablePIDcount = 0;
                    });

                    break;
                case Mode.dataCollector:
                    Task.Run(() =>
                    {
                        while (!forceStop)
                        {
                            client.SendSpeedRequest();
                            dataReceivedEvent.WaitOne(2000);
                            client.SendRpmRequest();
                            dataReceivedEvent.WaitOne(2000);
                            client.SendFuelLevelRequest();
                            dataReceivedEvent.WaitOne(2000);
                        }

                    });
                    break;
                case Mode.freeMode:
                    while (!forceStop)
                    {
                        dataReceivedEvent.WaitOne(10000);
                        Console.WriteLine("Type your message to send:");
                        var message = Console.ReadLine();
                        client.send(message+"\r");
                        
                    }
                    break;
                default:
                    break;
            }

        }

        private string DecToHex(string v)
        {
            var output = Convert.ToString(Convert.ToInt32(v, 10), 16);
            if (output.Length == 1) output = "0" + Convert.ToString(Convert.ToInt32(v, 10), 16);

            return output.ToUpper();
        }

        private void Client_PidMessageArrived(string message)
        {
            switch (mode)
            {
                case Mode.PIDdetector:
                    if (!message.Contains("NO DATA"))
                    {
                        totalAvailablePIDcount++;
                        PIDlist.Add(message);
                    }
                    arEvent.Set();
                    break;
                case Mode.dataCollector:
                    if (message.Contains("41 0D"))
                    {
                        var xx = message.Split("41 0D ")[1];
                        var spd = (message.Split("41 0D ")[1].Replace(" ", "").Substring(0, 2));
                        Console.WriteLine("SPEED : " + (Convert.ToInt32(HelperTool.hex2bin(spd), 2)) + " km/h");
                    }
                    else if (message.Contains("41 0C"))
                    {
                        var xx = message.Split("41 0C ")[1];
                        var rpm = (message.Split("41 0C ")[1].Replace(" ", "").Substring(0, 4));
                        Console.WriteLine("RPM : " + (Convert.ToInt32(HelperTool.hex2bin(rpm), 2) / 4) + " rpm");
                    }
                    else if (message.Contains("41 2F"))
                    {
                        var xx = message.Split("41 2F ")[1];
                        var fuelLevel = (message.Split("41 2F ")[1].Replace(" ", "").Substring(0, 2));
                        var val = (Convert.ToInt32(HelperTool.hex2bin(fuelLevel), 2) / 2.55);
                        Console.WriteLine("Fuel Level : % " + Math.Round(val, 2));
                    }
                    dataReceivedEvent.Set();
                    break;
                case Mode.freeMode:
                    Console.WriteLine("Received:"+message);
                    dataReceivedEvent.Set();
                    break;
                default:
                    break;
            }
        }
    }
}
