using DotnetSampleApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotnetSampleApiTest
{
    public class UnitTest1
    {
        private string _providerUri { get; }
        private string _pactServiceUri { get; }
        private IWebHost _webHost { get; }
        private ITestOutputHelper _outputHelper { get; }

        public UnitTest1(ITestOutputHelper output)
        {
            _outputHelper = output;

            _providerUri = "http://localhost:9000";
            _pactServiceUri = "http://localhost:9001";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public async Task EnsureSomethingApiHonoursPactWithConsumer()
        {
            IPactVerifier pactVerifier = new PactVerifier(new PactVerifierConfig
            {
                Outputters = new List<IOutput> //NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output, so a custom outputter is required.
				{
                    new XUnitOutput(_outputHelper)
                },
                Verbose = true //Output verbose verification logs to the test output
            });
            ///XXXX testowanie provider

            //HttpClient client = _testServer.CreateClient();
            //HttpResponseMessage response = await client.GetAsync($"/WeatherForecast?day={DateTime.UtcNow:yyyy-MM-ddThh:mm:ssZ}");
            //var aa = response.StatusCode;
            //string responseHtml = await response.Content.ReadAsStringAsync();

            //Arrange
            //Act / Assert
            pactVerifier
                //.ProviderState($"{_pactServiceUri}/provider-states")
                .ServiceProvider("Something API", _pactServiceUri) //NOTE: You can also use your own client by using the ServiceProvider method which takes a Func<ProviderServiceRequest, ProviderServiceResponse>. You are then responsible for mapping and performing the actual provider verification HTTP request within that Func.
                .HonoursPactWith("TourApi")
                .PactUri("D:/Projekty/pacts_new/pactss/tourapi-weatherforecastapi.json")
                //or
                .Verify(); //NOTE: Optionally you can control what interactions are verified by specifying a providerDescription and/or providerState
        }

        private void AddTesterIfItDoesntExist()
        {
            //Logic to add the 'tester' something
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _webHost.StopAsync().GetAwaiter().GetResult();
                    _webHost.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }

    public class XUnitOutput : IOutput
    {
        private readonly ITestOutputHelper _output;

        public XUnitOutput(ITestOutputHelper output)
        {
            _output = output;
        }

        public void WriteLine(string line)
        {
            _output.WriteLine(line);
        }
    }

    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "Consumer";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is no data",
                    RemoveAllData
                },
                {
                    "There is data",
                    AddData
                }
            };
        }

        private void RemoveAllData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"D:\Projekty\pacts_new\pactss\data");
            var deletePath = Path.Combine(path, "somedata.txt");

            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
        }
        //XXXXX https://github.com/pact-foundation/pact-workshop-dotnet-core-v1
        private void AddData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"D:\Projekty\pacts_new\pactss\data");
            var writePath = Path.Combine(path, "somedata.txt");

            if (!File.Exists(writePath))
            {
                File.Create(writePath);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                await this.HandleProviderStatesRequestAsync(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await this._next(context);
            }
        }

        private async Task HandleProviderStatesRequestAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = String.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !String.IsNullOrEmpty(providerState.State) &&
                    providerState.Consumer == ConsumerName)
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }

    public class ProviderState
    {
        public string Consumer { get; set; }
        public string State { get; set; }
    }

    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());

            //app.UseMiddleware<ProviderStateMiddleware>();
            //app.UseMvc();
        }
    }
}
