using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TourApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private string _weatherForecastsApiUrl;

        public TourController(IConfiguration configuration, ILogger<TourController> logger)
        {
            _weatherForecastsApiUrl = configuration.GetValue<string>("WeatherForecastsApiUrl");
        }
        
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
