using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mockaroo.Business.Core.Configuration;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Infrastructure.Data.Mockaroo.Providers;

namespace Data.Mockaroo.Startup
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMockaroo(this IServiceCollection services, IConfiguration config)
        {

            var mockarooConfig = config.GetSection("MockarooApi").Get<MockarooApi>();
            services.AddHttpClient<IUsersProvider, UsersProvider>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new System.Uri(mockarooConfig.BaseUri!);
                        client.DefaultRequestHeaders.Add("X-API-Key", mockarooConfig.ApiKey);
                    })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}