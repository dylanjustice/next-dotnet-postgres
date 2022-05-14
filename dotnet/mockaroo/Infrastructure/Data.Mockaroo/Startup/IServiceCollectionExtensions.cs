using Microsoft.Extensions.DependencyInjection;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Infrastructure.Data.Mockaroo.Providers;

namespace Data.Mockaroo.Startup
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMockaroo(this IServiceCollection services)
        {
            services.AddHttpClient<IMockarooProvider, MockarooProvider>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}