using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotnetSampleApi2
{
    public class SampleApiClient
    {
        private readonly HttpClient _client;

        public SampleApiClient(string baseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my-api") };
        }

        public async Task<WeatherForecast> GetSomething()
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, "/WeatherForecast");
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            reasonPhrase = response.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !String.IsNullOrEmpty(content) ?
                  JsonConvert.DeserializeObject<WeatherForecast>(content)
                  : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}
