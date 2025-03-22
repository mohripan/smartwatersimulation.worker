using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaterSimulation.Worker.Domain
{
    public class SensorData
    {
        public string SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public override string ToString() => $"ID: {SensorId}, Time: {Timestamp}, Value: {Value:F3}";
    }
}
