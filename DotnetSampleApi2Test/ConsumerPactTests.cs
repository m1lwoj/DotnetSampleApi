using System;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;
using DotnetSampleApi2;
using PactNet.Matchers;

namespace tests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }

        // [Fact]
        // public void ItParsesForecastsForDay()
        // {
        //     var expectedDate = DateTime.UtcNow;
        //
        //     // Arrange
        //     _mockProviderService.Given("There is data")
        //                         .UponReceiving("A valid GET request for Date Validation")
        //                         .With(new ProviderServiceRequest 
        //                         {
        //                             Method = HttpVerb.Get,
        //                             Path = $"/WeatherForecast/{expectedDate:yyyy-MM-dd}",
        //                             Query = ""
        //                         })
        //                         .WillRespondWith(new ProviderServiceResponse {
        //                             Status = 200,
        //                             Headers = new Dictionary<string, object>
        //                             {
        //                                 { "Content-Type", "application/json; charset=utf-8" }
        //                             },
        //                             Body = new WeatherForecast
        //                             {
        //                                 Date = expectedDate,
        //                                 Summary = string.Empty,
        //                                 TemperatureC = default,
        //                             }
        //                         });
        //
        //     // Act
        //     var result = WeatherForecastsApiClient.GetForecastsForDay(expectedDate, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
        //     var resultBody = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        //
        //     // Assert
        //     Assert.Contains(expectedDate.ToString("yyyy-MM-dd"), resultBody);
        // }
        
        [Fact]
        public void ItParsesForecastsForDay2()
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
