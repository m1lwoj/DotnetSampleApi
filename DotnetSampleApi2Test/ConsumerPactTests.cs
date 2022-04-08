using System;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;
using PactNet.Matchers;

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
        public void ItParsesForecastsForDay()
        {
            var expectedDate = new DateTime(2020, 2, 2);
        
            // Arrange
            _mockProviderService.Given("Can get forecasts data")
                .UponReceiving("A valid GET request for Date Validation6")
                .With(new ProviderServiceRequest 
                {
                    Method = HttpVerb.Get,
                    Path = "/WeatherForecast",
                    Query = $"day={expectedDate:yyyy-MM-ddThh:mm:ssZ}",
                })
                .WillRespondWith(new ProviderServiceResponse {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new 
                    {
                        date = $"{expectedDate:yyyy-MM-ddThh:mm:ssZ}",
                        temperatureC = Match.Type(1),
                        temperatureF = Match.Type(1)
                    }
                });
        
            // Act
            var result = WeatherForecastsApiClient.GetForecastsForDay2(expectedDate, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBody = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        
            // Assert
            Assert.Contains(expectedDate.ToString("yyyy-MM-dd"), resultBody);
        }
    }
}
