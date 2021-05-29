using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer
{
    public static class WeatherForecastsApiClient
    {
        public static async Task<HttpResponseMessage> GetForecastsForDay(DateTime dateTime, string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri)})
            {
                try
                {
                    var response = await client.GetAsync($"/WeatherForecast/{dateTime:yyyy-MM-dd}");
                    return response;
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
        
        public static async Task<HttpResponseMessage> GetForecastsForDay2(DateTime dateTime, string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri)})
            {
                try
                {
                    var response = await client.GetAsync($"/WeatherForecast?day={dateTime:yyyy-MM-ddThh:mm:ssZ}");
                    return response;
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
    }
}