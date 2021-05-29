using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DotnetSampleApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        public TourController(IConfiguration configuration, ILogger<TourController> logger)
        {
            _weatherForecastsApiUrl = configuration.GetValue<string>("WeatherForecastsApiUrl");
            _logger = logger;
        }
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bryacing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TourController> _logger;
        private string _weatherForecastsApiUrl;

        [HttpGet]
        [Route("{date}")]
        public async Task<TourPlan> GetPlanForDay(DateTime date)
        {
            var client = WeatherForecastsApiClient.GetForecastsForDay(date, _weatherForecastsApiUrl);
            var weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(await client.Result.Content.ReadAsStringAsync());

            return new TourPlan
            {
                Date = date,
                Description = $"Hiking tour for day {date:yyyy-MM-dd}, attractions included ...",
                WeatherForecast = weatherForecast
            };
        }
    }
}
