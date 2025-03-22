using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using SmartWaterSimulation.Worker.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var jsonMessage = JsonSerializer.Serialize(sensorData);
            var message = new ServiceBusMessage(jsonMessage)
            {
                MessageId = sensorData.SensorId
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
