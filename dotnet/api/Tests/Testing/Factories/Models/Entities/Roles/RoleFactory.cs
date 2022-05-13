using AndcultureCode.CSharp.Testing.Factories;
using DylanJustice.Demo.Business.Core.Models.Entities.Roles;

namespace DylanJustice.Demo.Testing.Factories.Models.Entities.Roles
{
    public class RoleFactory : Factory
    {
        public override void Define()
        {
            this.DefineFactory(() => new Role
            {
                Description = $"Test Role Description {UniqueNumber}",
                Name = $"Test Role {UniqueNumber}"
            });
        }
    }
}