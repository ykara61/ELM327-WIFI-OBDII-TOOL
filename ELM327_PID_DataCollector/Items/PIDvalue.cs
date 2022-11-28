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
        public int PID_DECIMAL { get; set; }
        public object ?Data_bytes_returned { get; set; }
        public string ?Description { get; set; }
        public string ?Formula { get; set; }
        public decimal ?Minvalue { get; set; }
        public decimal ?Maxvalue { get; set; }
        public string ?Units { get; set; }
    }
}
