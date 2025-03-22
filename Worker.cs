using Microsoft.Extensions.Options;
using SmartWaterSimulation.Worker.Configuration;
using SmartWaterSimulation.Worker.Domain;

namespace SmartWaterSimulation.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISensorSimulator _sensorSimulator;
        private readonly ISensorPublisher _sensorPublisher;
        private readonly IOptionsMonitor<SensorSettings> _settings;

        public Worker(ILogger<Worker> logger,
                      ISensorSimulator sensorSimulator,
                      ISensorPublisher sensorPublisher,
                      IOptionsMonitor<SensorSettings> settings)
        {
            _logger = logger;
            _sensorSimulator = sensorSimulator;
            _sensorPublisher = sensorPublisher;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                SensorData sensorData = _sensorSimulator.GenerateSensorData();
                _logger.LogInformation("Generated Sensor Data: {data}", sensorData);

                try
                {
                    await _sensorPublisher.PublishSensorDataAsync(sensorData);
                    _logger.LogInformation("Published sensor data to Service Bus.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing sensor data.");
                }

                int interval = _settings.CurrentValue.IntervalMilliseconds;
                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
