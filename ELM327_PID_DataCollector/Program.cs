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

            foreach (var i in Helpers.HelperTool.ReadJsonConfiguration(Helpers.HelperTool.ReadResource("PID_Values.json")))
            {
                Console.WriteLine(i.Description);
            }

            Elm327wifi elmObd = new Elm327wifi("192.168.43.170",35000);

            while (true)
            {
                elmObd.Start();

                Console.ReadKey();

                elmObd.Stop();
            }
        }

    }
}