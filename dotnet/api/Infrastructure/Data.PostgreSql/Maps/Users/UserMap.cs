using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users
{
    public class UserMap : Map<User>
    {
        public override void Configure(EntityTypeBuilder<User> entity)
        {
        }
    }
}