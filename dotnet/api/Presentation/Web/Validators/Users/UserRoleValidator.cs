using FluentValidation;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Users;

namespace DylanJustice.Demo.Presentation.Web.Validators.Users
{
    public class UserRoleValidator : AbstractValidator<UserRoleDto>
    {
        public UserRoleValidator()
        {
            RuleFor(m => m.RoleId)
                .GreaterThan(0);

            RuleFor(m => m.UserId)
                .GreaterThan(0);
        }
    }
}
