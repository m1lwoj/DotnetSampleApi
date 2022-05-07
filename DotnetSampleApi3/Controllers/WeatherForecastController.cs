using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetSampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public WeatherForecast GetForecasts()
        {
            var rng = new Random();
            return new WeatherForecast
            {
                Date = DateTime.UtcNow.Date,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }

        [HttpGet]
        [Route("{day}")]
        public WeatherForecast GetForecastsForDay(DateTime day)
        {
            var rng = new Random();
            return new WeatherForecast
            {
                Date = day.Date,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }

        [HttpPost]
        public IActionResult CreateForecast(WeatherForecast forecast)
            => Created($"weatherforecast/{forecast.Date:yyyy-MM-dd}", forecast);
    }
}
