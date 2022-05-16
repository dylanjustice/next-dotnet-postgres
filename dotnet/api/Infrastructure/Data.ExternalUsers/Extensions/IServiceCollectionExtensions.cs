using System;
using DylanJustice.Demo.Api.Business.Core.Interfaces.Providers;
using DylanJustice.Demo.Business.Core.Models.Configuration;
using DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DylanJustice.Demo.Infrastructure.Data.ExternalUsers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployeeService(this IServiceCollection services, IConfiguration configuration)
        {
            var employeeConfig = configuration.GetSection("EmployeeApi").Get<EmployeeApiConfiguration>();
            services.AddHttpClient<IEmployeeProvider, EmployeeProvider>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new System.Uri(employeeConfig.BaseUri);
                        // client.DefaultRequestHeaders.Add(//dapr stuff);
                    })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}