using AndcultureCode.CSharp.Testing.Factories;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Testing.Factories.Models.Entities.Users
{
    public class UserRoleFactory : Factory
    {
        public override void Define()
        {
            this.DefineFactory(() => new UserRole
            {
                RoleId = long.MaxValue,
                UserId = long.MaxValue
            });
        }
    }
}