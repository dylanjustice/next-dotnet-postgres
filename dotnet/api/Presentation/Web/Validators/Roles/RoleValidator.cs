using FluentValidation;
using DylanJustice.Demo.Business.Core.Models.Configuration;
using DylanJustice.Demo.Presentation.Web.Models.Dtos.Roles;
using AndcultureCode.CSharp.Core.Models.Configuration;

namespace DylanJustice.Demo.Presentation.Web.Validators.Roles
{
    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(m => m.Description)
                .MaximumLength(DataConfiguration.LONG_DESCRIPTION_LENGTH);

            RuleFor(m => m.Name)
                .NotEmpty()
                .MaximumLength(DataConfiguration.SHORT_STRING_LENGTH);
        }
    }
}
