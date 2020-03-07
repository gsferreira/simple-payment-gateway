using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Application.Commands;
using PaymentGateway.Core.Model;
using PaymentGateway.Infrastructure.BankProviders;
using PaymentGateway.Infrastructure.Events;
using PaymentGateway.Infrastructure.Repositories;
using PaymentGateway.SharedKernel;
using Microsoft.OpenApi.Models;

namespace PaymentGateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("FakeBank", c =>
            {
                c.BaseAddress = new Uri(Configuration["FakeBankService"]);
            });

            services.AddScoped<IAcquiringBank, FakeBankClient>();
            services.AddSingleton<IDeferredEventDispatcher, DeferredEventDispatcher>();
            services.AddSingleton<IEventStoreRepository<Payment>, InMemoryEventStore<Payment>>();

            services.AddMediatR(typeof(Startup).Assembly,
                typeof(PaymentCommand).Assembly,
                typeof(DeferredEventDispatcher).Assembly,
                typeof(Payment).Assembly);
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
