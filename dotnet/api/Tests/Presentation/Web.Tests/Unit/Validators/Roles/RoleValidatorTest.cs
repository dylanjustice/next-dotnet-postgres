using FluentValidation.TestHelper;
using DylanJustice.Demo.Business.Core.Models.Configuration;
using DylanJustice.Demo.Presentation.Web.Validators.Roles;
using System;
using DylanJustice.Demo.Testing.Tests;
using Xunit;
using Xunit.Abstractions;
using AndcultureCode.CSharp.Core.Models.Configuration;

namespace DylanJustice.Demo.Presentation.Web.Tests.Unit.Validators.Roles
{
    public class RoleValidatorTest : ApiUnitTest, IDisposable
    {
        #region Member Variables

        RoleValidator _sut;

        #endregion Member Variables


        #region Setup

        public RoleValidatorTest(ITestOutputHelper output) : base(output)
        {
            _sut = new RoleValidator();
        }

        #endregion Setup


        #region Name

        [Fact]
        public void Name_ShouldHaveError_When_Empty()
        {
            _sut.ShouldHaveValidationErrorFor(m => m.Name, string.Empty);
        }

        [Fact]
        public void Name_ShouldNotHaveError_When_Length_LessThanOrEqualTo_MaxLength()
        {
            for (var i = 1; i <= DataConfiguration.SHORT_STRING_LENGTH; i++)
            {
                _sut.ShouldNotHaveValidationErrorFor(m => m.Name, new String('x', i));
            }
        }

        [Fact]
        public void Name_ShouldHaveError_When_Length_GreaterThan_MaxLength()
        {
            _sut.ShouldHaveValidationErrorFor(m => m.Name, new String('x', DataConfiguration.SHORT_STRING_LENGTH + 1));
        }

        #endregion Name
    }
}
