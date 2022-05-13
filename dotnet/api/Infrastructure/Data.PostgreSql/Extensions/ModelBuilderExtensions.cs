using Microsoft.EntityFrameworkCore;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Add mapping for the given entity type
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="map"></param>
        public static void AddMapping<TEntity>(this ModelBuilder builder, Map<TEntity> map)
        where TEntity : class
        {
            builder.Entity<TEntity>(map.Configure);
        }

    }
}