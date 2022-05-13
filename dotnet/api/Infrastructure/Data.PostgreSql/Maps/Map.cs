using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps
{
    public abstract class Map<TEntity> where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> entity);
    }
}