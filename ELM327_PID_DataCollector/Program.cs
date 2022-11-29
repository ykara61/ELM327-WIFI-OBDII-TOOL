using ELM327_PID_DataCollector.Helpers;
using System.Reflection;
using System.IO;
using System.Reflection;
using ELM327_PID_DataCollector.Items;

namespace ELM327_PID_DataCollector
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("*ELM 327 WiFi Data Collector 2022*");
            Console.WriteLine("Developed By sukurcan61");
            Console.WriteLine("***************************");


            Console.Write("Enter OBD Device IP address (ex:192.168.0.10): ");
            var Ip = Console.ReadLine();
            Console.Write("Enter OBD Device port number (ex:35000): ");
            int.TryParse(Console.ReadLine(),out int port);

            Console.WriteLine("***************************");


            Elm327wifi elmObd = new Elm327wifi(Ip,port);

            while (true)
            {
                elmObd.Start();

                Console.ReadKey();

                elmObd.Stop();
            }
        }

    }
}