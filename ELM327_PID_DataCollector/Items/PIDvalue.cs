using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM327_PID_DataCollector.Items
{
    public class PIDvalue
    {
        public int PIDdec { get; set; }
        public string PIDhex { get; set; }
        public string Name { get; set; }
        public int ?BitStart { get; set; }
        public int ?BitLength { get; set; }
        public object ?Scale { get; set; }
        public object ?Offset { get; set; }
        public object ?Min { get; set; }
        public object ?Max { get; set; }
        public string ?Unit { get; set; }
    }
}
