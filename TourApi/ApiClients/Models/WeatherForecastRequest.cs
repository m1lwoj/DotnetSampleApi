using System;

namespace TourApi.ApiClients.Models
{
    public class WeatherForecastRequest
    {
        public string Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
