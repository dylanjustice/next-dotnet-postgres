using AndcultureCode.CSharp.Conductors;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Microsoft.Extensions.DependencyInjection;
using DylanJustice.Demo.Business.Core.Interfaces.Conductors.Jobs;
using DylanJustice.Demo.Business.Conductors.Domain.Jobs;
using Microsoft.Extensions.Configuration;
using DylanJustice.Demo.Business.Core.Interfaces.Conductors.Domain.Users;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;
using DylanJustice.Demo.Business.Conductors.Domain.UserLogins;

namespace DylanJustice.Demo.Business.Conductors.Extensions.Startup
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConductors(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Job
            services.AddScoped<IJobEnqueueConductor, JobEnqueueConductor>();

            // Users
            services.AddScoped<IUserLoginConductor<User>, UserLoginConductor<User>>();

            // Repository defaults - Should appear last
            services.AddScoped(typeof(IRepositoryCreateConductor<>), typeof(RepositoryCreateConductor<>));
            services.AddScoped(typeof(IRepositoryReadConductor<>), typeof(RepositoryReadConductor<>));
            services.AddScoped(typeof(IRepositoryUpdateConductor<>), typeof(RepositoryUpdateConductor<>));
            services.AddScoped(typeof(IRepositoryDeleteConductor<>), typeof(RepositoryDeleteConductor<>));
            services.AddScoped(typeof(IRepositoryConductor<>), typeof(RepositoryConductor<>));

            return services;
        }
    }
}
