using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaterSimulation.Worker.Configuration
{
    public class SensorSettings
    {
        public int IntervalMilliseconds { get; set; } = 1000;
        public string DataPattern { get; set; } = "Uniform";
    }
}
