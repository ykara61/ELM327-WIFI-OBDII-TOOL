using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
