using System;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;
using PactNet.Matchers;
using DotnetSampleApi2;
using DotnetSampleApi2.ApiClients.Models;
using Newtonsoft.Json;

namespace WeatherApiTests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); 
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }


        [Fact]
        public void ItParseForecastForDay()
        {
            var expectedDate = new DateTime(2020, 2, 2).Date;

            // Arrange
            _mockProviderService.Given("Can get forecasts data for given date")
                .UponReceiving("A valid GET request for GetForecsastForDay")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/WeatherForecast/{expectedDate:yyyy-MM-ddTHH:mm:ssZ}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        date = $"{expectedDate:yyyy-MM-ddTHH:mm:ssZ}",
                        temperatureC = Match.Type(1),
                        temperatureF = Match.Type(1)
                    }
                });

            // Act
            var result = WeatherForecastsApiClient.GetForecastsForDay(expectedDate, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBody = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            //Aly URL
            // Assert
            Assert.NotEqual(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(expectedDate.ToString("yyyy-MM-dd"), resultBody);
        }

        [Fact]
        public void ItParseForecast()
        {
            var expectedDate = DateTime.UtcNow.Date;

            // Arrange
            _mockProviderService.Given("Can get forecasts data for current date")
                .UponReceiving("A valid GET request for Forecast")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/WeatherForecast",
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        date = $"{expectedDate:yyyy-MM-ddTHH:mm:ssZ}",
                        temperatureC = Match.Type(1),
                        temperatureF = Match.Type(1)
                    }
                });

            // Act
            var result = WeatherForecastsApiClient.GetForecastsForDay2(expectedDate, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBody = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(resultBody);
        }


        [Fact]
        public void ItCanAddWeatherForecast()
        {
            var expectedDate = new DateTime(2020, 1, 2).Date;

            var modelToSend = new WeatherForecastRequest
            {
                Date = $"{expectedDate:yyyy-MM-ddTHH:mm:ssZ}",
                Summary = "Bad weather",
                TemperatureC = 12
            };

            // Arrange
            _mockProviderService.Given("Can post forecasts data")
                .UponReceiving("A valid POST request for Forecast")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/WeatherForecast",
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = JsonConvert.DeserializeObject(WeatherForecastsApiClient.ToCamelCaseNotation(modelToSend))
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        date = $"{expectedDate:yyyy-MM-ddTHH:mm:ssZ}",
                        temperatureC = Match.Type(modelToSend.TemperatureC),
                        temperatureF = Match.Type(modelToSend.TemperatureC),
                        summary = Match.Type(modelToSend.Summary)
                    }
                });

            // Act
            var result = WeatherForecastsApiClient.AddForecast(modelToSend, _mockProviderServiceBaseUri).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(result);
        }
    }
}
