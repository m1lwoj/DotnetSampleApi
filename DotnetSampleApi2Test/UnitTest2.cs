using Newtonsoft.Json;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotnetSampleApi2Test
{
    public class ParcelsApiPactConsumerTests
    {
        [Test]
        public async Task Given_Valid_Parcel_Id_Parcel_Should_Be_Returned()
        {
            _mockProviderService
                .Given("Existing parcel")
                .UponReceiving("A GET request to retrieve parcel details")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/parcels/{ParcelId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new ParcelDto
                    {
                        Id = new Guid(ParcelId),
                        Name = "Product",
                        Size = "Huge",
                        Variant = "Weapon"
                    }
                });

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_serviceUri}/parcels/{ParcelId}");
            var json = await response.Content.ReadAsStringAsync();
            var parcel = JsonConvert.DeserializeObject<ParcelDto>(json);

            Assert.AreEqual(parcel.Id.ToString(), ParcelId);
        }

        #region ARRANGE

        private const string ParcelId = "c68a24ea-384a-4fdc-99ce-8c9a28feac64";

        private readonly IMockProviderService _mockProviderService;
        private readonly string _serviceUri;

        public ParcelsApiPactConsumerTests()
        {
            var fixture = new ParcelsApiMock();
            _mockProviderService = fixture.MockProviderService;
            _serviceUri = fixture.ServiceUri;
            _mockProviderService.ClearInteractions();
        }

        #endregion
    }
}
