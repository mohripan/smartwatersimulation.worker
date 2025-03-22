namespace SmartWaterSimulation.Worker.Domain
{
    public interface ISensorPublisher
    {
        Task PublishSensorDataAsync(SensorData sensorData);
    }
}
