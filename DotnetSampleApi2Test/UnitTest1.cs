//using TourApi;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using PactNet;
//using PactNet.Mocks.MockHttpService;
//using PactNet.Mocks.MockHttpService.Models;
//using PactNet.Models;
//using System;
//using System.Collections.Generic;

//namespace DotnetSampleApi2Test
//{
//    public class Tests
//    {
//        private IMockProviderService _mockProviderService;
//        private string _mockProviderServiceBaseUri;

//        public Tests()
//        {
//            var data = new ConsumerMyApiPact();
//            _mockProviderService = data.MockProviderService;
//            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
//            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
//        }

//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test1()
//        {
//            //Arrange
//            _mockProviderService
//              .Given("There is a weather'")
//              .UponReceiving("A GET request to retrieve w5the something")
//              .With(new ProviderServiceRequest
//              {
//                  Method = HttpVerb.Get,
//                  Path = "/WeatherForecast",
//                  Headers = new Dictionary<string, object>
//                {
//          { "Accept", "application/json" }
//                }
//              })
//              .WillRespondWith(new ProviderServiceResponse
//              {
//                  Status = 200,
//                  Headers = new Dictionary<string, object>
//                {
//          { "Content-Type", "application/json; charset=utf-8" }
//                },
//                  Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
//                  {
//                      TemperatureC = 14,
//                      Date = DateTime.UtcNow,
//                      Summary = "Awesome"
//                  }
//              }); //NOTE: WillRespondWith call must come last as it will register the interaction

//            var consumer = new SampleApiClient(_mockProviderServiceBaseUri);

//            //Act
//            var result = consumer.GetSomething().GetAwaiter().GetResult();

//            //Assert
//            Assert.IsNotEmpty(result.Summary);

//            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
//        }
//    }
//}

//public class ConsumerMyApiPact : IDisposable
//{
//    public IPactBuilder PactBuilder { get; private set; }
//    public IMockProviderService MockProviderService { get; private set; }

//    public int MockServerPort { get { return 9222; } }
//    public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

//    public ConsumerMyApiPact()
//    {
//        PactBuilder = new PactBuilder(); //Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
//                                         //or
//        PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" }); //Configures the Specification Version
//                                                                                          //or
//        PactBuilder = new PactBuilder(new PactConfig { PactDir = @"..\..\..\..\..\pacts", LogDir = @"c:\temp\logs" }); //Configures the PactDir and/or LogDir.

//        PactBuilder
//          .ServiceConsumer("Consumer")
//          .HasPactWith("Something API");

//        MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
//                                                                       //or
//        MockProviderService = PactBuilder.MockService(MockServerPort, false); //By passing true as the second param, you can enabled SSL. A self signed SSL cert will be provisioned by default.
//                                                                              //or
//                                                                              //MockProviderService = PactBuilder.MockService(MockServerPort, true, sslCert: sslCert, sslKey: sslKey); //By passing true as the second param and an sslCert and sslKey, you can enabled SSL with a custom certificate. See "Using a Custom SSL Certificate" for more details.
//                                                                              //or
//        MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings()); //You can also change the default Json serialization settings using this overload    
//                                                                                                     //or
//        MockProviderService = PactBuilder.MockService(MockServerPort, host: IPAddress.Any); //By passing host as IPAddress.Any, the mock provider service will bind and listen on all ip addresses

//    }

//    public void Dispose()
//    {
//        PactBuilder.Build(); //NOTE: Will save the pact file once finished
//    }
//}
