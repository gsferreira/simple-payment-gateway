using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PaymentGateway.Api;
using PaymentGateway.FunctionalTests.Extensions;
using Xunit;
using FluentAssertions;
using Moq;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Application.Queries;
using PaymentGateway.Core.Model;


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
            var response = await _client.PostAsJsonAsync("/api/payments", Payment());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Get_ExistingPayment_Succeed()
        {
            var response = await _client.PostAsJsonAsync("/api/payments", Payment());
            response.EnsureSuccessStatusCode();
            var expectedPaymentDetails = await response.Content.ReadAsJsonAsync<PaymentDto>();

            var getResponse = await _client.GetAsync($"/api/payments/{expectedPaymentDetails.Id}");
            var paymentDetails = await getResponse.Content.ReadAsJsonAsync<PaymentDto>();

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            paymentDetails.Should().BeEquivalentTo(expectedPaymentDetails);
        }

        [Fact]
        public async Task Get_ExistingPayment_HasMaskedCardNumber()
        {
            dynamic payment = Payment();
            var response = await _client.PostAsJsonAsync("/api/payments", Payment());
            
            response.EnsureSuccessStatusCode();
            var createResponse = await response.Content.ReadAsJsonAsync<PaymentDto>();

            var getResponse = await _client.GetAsync($"/api/payments/{createResponse.Id}");
            var paymentDetails = await getResponse.Content.ReadAsJsonAsync<PaymentDto>();

            Assert.NotEqual(payment.card.number, paymentDetails.Card.Number);
        }

        private static object Payment()
        => new
        {
            card = new
            {
                type = "VISA",
                name = "Tyrion Lannister",
                number = "4532367296473418",
                expireMonth = 12,
                expireYear = DateTime.Today.Year,
                cvv = 765
            },
            amount = 123.53,
            currency = "EUR"
        };

    }
}