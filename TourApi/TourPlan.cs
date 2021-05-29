using System;

namespace DotnetSampleApi2
{
    public class TourPlan
    {
        public DateTime Date { get; set; }

        public string Description  { get; set; }
        public WeatherForecast WeatherForecast  { get; set; }
    }
    
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
