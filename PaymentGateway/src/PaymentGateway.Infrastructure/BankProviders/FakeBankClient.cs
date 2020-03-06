using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Core.Model;

namespace PaymentGateway.Infrastructure.BankProviders
{
    public class FakeBankClient : IAcquiringBank
    {
        private readonly IHttpClientFactory _clientFactory;

        public FakeBankClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<BankPaymentResult> Process(Payment payment)
        {
            var client = _clientFactory.CreateClient("FakeBank");

            var dataAsString = JsonConvert.SerializeObject(new
            {
                payment.Amount,
                CardName = payment.Card.Name,
                CardNumber = payment.Card.Number,
                CardExpireMonth = payment.Card.ExpireMonth,
                CardExpireYear = payment.Card.ExpireYear,
                CardCVV = payment.Card.CVV
            });
            var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("/retrieve", content);

                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return new BankPaymentResult()
                    {
                        Success = false,
                        Message = await response.Content.ReadAsStringAsync()
                    };
                
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                    await response.Content.ReadAsStringAsync());

                return new BankPaymentResult()
                {
                    Success = response.IsSuccessStatusCode,
                    Id = responseContent.ContainsKey("id") ? responseContent["id"].ToString() : null
                };
            }
            catch (Exception e)
            {
                return new BankPaymentResult()
                {
                    Success = false,
                    Message = $"Error connecting to Fake Bank: {e.Message}"
                };
            }
        }
    }
}
