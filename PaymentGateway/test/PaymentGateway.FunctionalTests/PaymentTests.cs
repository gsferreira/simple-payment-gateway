using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PaymentGateway.Api;
using PaymentGateway.FunctionalTests.Extensions;
using Xunit;

namespace PaymentGateway.FunctionalTests
{
    public class PaymentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public PaymentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        private readonly HttpClient _client;

        [Fact]
        public async Task Create_Succeed()
        {
            var requestData = new
            {
                card = new
                {
                    type = "VISA",
                    name = "Tyrion Lannister",
                    number = 4532367296473418,
                    expireMonth = 12,
                    expireYear = DateTime.Today.Year,
                    cvv = 765
                },
                amount = 123.53,
                currency = "EUR"
            };

            var response = await _client.PostAsJsonAsync("/api/payments", requestData);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}