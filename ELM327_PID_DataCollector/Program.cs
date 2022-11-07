using ELM327_PID_DataCollector.Helpers;

namespace ELM327_PID_DataCollector
{
    internal class Program
    {
        static TcpClientOBD client;
        static void Main(string[] args)
        {
            Console.WriteLine("ELM 327 WIFI DATA COLLECTOR STARTED");
            Console.WriteLine(".....");

            client = new TcpClientOBD("127.0.0.1", 35000);

            client.PidMessageArrived += Client_PidMessageArrived;
            client.OBDdeviceReady += Client_OBDdeviceReady;

            client.StartOBDdev();

            Console.ReadKey();
        }

        private static void Client_OBDdeviceReady()
        {
            client.SendSpeedRequest();
        }

        private static void Client_PidMessageArrived(string message)
        {
            if (message.Contains("41 0D"))
            {
                var xx = message.Split("41 0D ")[1];
                var spd = (message.Split("41 0D ")[1].Substring(0, 2));

                Console.WriteLine("SPEED : " + Convert.ToInt32(HelperTool.hex2bin(spd), 2));
            }
            else if (message.Contains("41 0C"))
            {
                var xx = message.Split("41 0C ")[1];
                var rpm = (message.Split("41 0C ")[1].Replace(" ", "").Substring(0, 4));


                Console.WriteLine("RPM : " + Convert.ToInt32(HelperTool.hex2bin(rpm), 2) / 4);

                client.send("010C" + "\r");
            }
        }
    }
}