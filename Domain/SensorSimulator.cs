﻿using Microsoft.Extensions.Options;
using SmartWaterSimulation.Worker.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaterSimulation.Worker.Domain
{
    public class SensorSimulator : ISensorSimulator
    {
        private readonly SensorSettings _settings;
        private readonly Random _random = new Random();
        private double _angle = 0;

        public SensorSimulator(IOptionsMonitor<SensorSettings> options)
        {
            _settings = options.CurrentValue;
        }

        public SensorData GenerateSensorData()
        {
            double value;
            if (_settings.DataPattern.Equals("Sine", StringComparison.OrdinalIgnoreCase))
            {
                value = Math.Sin(_angle);
                _angle += 0.1;
            }
            else
            {
                value = _random.NextDouble();
            }

            return new SensorData
            {
                SensorId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Value = value
            };
        }
    }
}
