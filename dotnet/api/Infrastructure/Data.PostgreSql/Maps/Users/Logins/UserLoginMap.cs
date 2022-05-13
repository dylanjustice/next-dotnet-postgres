using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users.Logins
{
    public class UserLoginMap : Map<UserLogin>
    {
        public override void Configure(EntityTypeBuilder<UserLogin> entity)
        {
        }
    }
}