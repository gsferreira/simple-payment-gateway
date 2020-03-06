using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api;
using PaymentGateway.Application.BankProviders;

namespace PaymentGateway.FunctionalTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("PaymentGateway/src/PaymentGateway.Api")
                .ConfigureServices(services =>
                {
                    services.AddScoped<IAcquiringBank, Fakes.AcquiringBankFake>();
                });
        }
    }
}
