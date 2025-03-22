using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaterSimulation.Worker.Domain
{
    public interface ISensorPublisher
    {
        Task PublishSensorDataAsync(SensorData sensorData);
    }
}
