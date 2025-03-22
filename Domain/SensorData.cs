namespace SmartWaterSimulation.Worker.Domain
{
    public class SensorData
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public override string ToString() => $"ID: {Id}, Time: {Timestamp}, Value: {Value:F3}";
    }
}
