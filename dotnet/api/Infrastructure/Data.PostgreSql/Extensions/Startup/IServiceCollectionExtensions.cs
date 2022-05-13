using System;
using AndcultureCode.CSharp.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Repositories;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Extensions.Startup
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }

}