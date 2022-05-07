using DotnetSampleApi2;
using DotnetSampleApi2.ApiClients.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                    var response = await client.GetAsync($"/WeatherForecast");
                    return response;
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }

        public static async Task<WeatherForecast> AddForecast(WeatherForecastRequest forecast, string baseUri)
        {
            using (var client = new HttpClient 
            {
                BaseAddress = new Uri(baseUri),
            })
            {
                try
                {
                    var response = await client.PostAsync($"/WeatherForecast", 
                        new StringContent(ToCamelCaseNotation(forecast), encoding: System.Text.Encoding.UTF8, "application/json"));

                    if(!response.IsSuccessStatusCode)
                    {
                        throw new Exception(await response.Content.ReadAsStringAsync());
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherForecast>(result);
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }

        private static string ToCamelCaseNotation<T>(T model)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(model, serializerSettings);

            return json;
        }
    }
}