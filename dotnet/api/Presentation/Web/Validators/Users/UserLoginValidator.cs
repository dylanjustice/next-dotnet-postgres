using FluentValidation;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Users;

namespace DylanJustice.Demo.Presentation.Web.Validators.Users
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(m => m.Password)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(m => m.UserName)
                .NotEmpty();
        }
    }
}
