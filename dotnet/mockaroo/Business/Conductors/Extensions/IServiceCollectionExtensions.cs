using AndcultureCode.CSharp.Conductors;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Mockaroo.Business.Core.Interfaces;
using Mockaroo.Business.Conductors.Domain;

namespace Mockaroo.Business.Conductors.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConductors(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Repository defaults - Should appear last
            services.AddScoped<IUserReadConductor, UserReadConductor>();

            return services;
        }
    }
}