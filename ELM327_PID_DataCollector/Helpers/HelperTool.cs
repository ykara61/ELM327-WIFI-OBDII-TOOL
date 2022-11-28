using ELM327_PID_DataCollector.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ELM327_PID_DataCollector.Helpers
{
    public static class HelperTool
    {
        public static string hex2bin(string value)
        {
            if (value.Length == 1) value = value.Insert(0, "0");
            return Convert.ToString(Convert.ToInt32(value, 16), 2).PadLeft(value.Length * 4, '0');
        }

        public static List<PIDvalue> ReadJsonConfiguration(string JsonValue)
        {
            List<PIDvalue> list = new List<PIDvalue>();
            try
            {
                list = JsonConvert.DeserializeObject<List<PIDvalue>>(JsonValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //kufotalConf = JsonConvert.DeserializeObject<KufotalJsonConfiguration>(jsonString);
            return list;
        }

        public static string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
