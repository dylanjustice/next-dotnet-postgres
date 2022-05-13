using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users.Roles
{
    public class UserRoleMap : Map<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> entity)
        {
        }
    }
}