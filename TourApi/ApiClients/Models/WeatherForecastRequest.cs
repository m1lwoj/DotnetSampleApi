using System;

namespace DotnetSampleApi2.ApiClients.Models
{
    public class WeatherForecastRequest
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
