using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DylanJustice.Demo.Business.Core.Models.Entities.Roles;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Roles
{
    public class RoleMap : Map<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> entity)
        {
        }
    }
}