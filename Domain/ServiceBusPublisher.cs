using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmartWaterSimulation.Worker.Configuration;

namespace SmartWaterSimulation.Worker.Domain
{
    public class ServiceBusPublisher : ISensorPublisher, IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly ILogger<ServiceBusPublisher> _logger;

        public ServiceBusPublisher(IOptions<ServiceBusSettings> options, ILogger<ServiceBusPublisher> logger)
        {
            _logger = logger;
            var settings = options.Value;
            _client = new ServiceBusClient(settings.ConnectionString);
            _sender = _client.CreateSender(settings.QueueName);
        }

        public async Task PublishSensorDataAsync(SensorData sensorData)
        {
            var jsonMessage = JsonConvert.SerializeObject(sensorData);
            var message = new ServiceBusMessage(jsonMessage)
            {
                MessageId = sensorData.Id
            };

            await _sender.SendMessageAsync(message);
        }

        public async ValueTask DisposeAsync()
        {
            if (_sender != null)
            {
                await _sender.DisposeAsync();
            }
            if (_client != null)
            {
                await _client.DisposeAsync();
            }
        }
    }
}
