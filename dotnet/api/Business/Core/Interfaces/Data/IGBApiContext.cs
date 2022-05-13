using System.Linq;
using DylanJustice.Demo.Business.Core.Models.Entities.Roles;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;
using DylanJustice.Demo.Business.Core.Models.Jobs;
using AndcultureCode.CSharp.Core.Interfaces.Data;

namespace DylanJustice.Demo.Business.Core.Interfaces.Data
{
    public interface IGBApiContext : IApplicationContext
    {
        IQueryable<Job> Jobs { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<User> Users { get; }
        IQueryable<UserLogin> UserLogins { get; }
        IQueryable<UserMetadata> UserMetadata { get; }
        IQueryable<UserRole> UserRoles { get; }
    }
}
