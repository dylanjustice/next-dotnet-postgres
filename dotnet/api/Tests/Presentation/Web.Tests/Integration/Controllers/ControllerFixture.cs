using System;
using DylanJustice.Demo.Tests.Testing.Constants;
using DylanJustice.Demo.Tests.Testing.Fixtures;
using Xunit;

namespace DylanJustice.Demo.Tests.Presentation.Web.Tests.Integration.Controllers
{
    public class ControllerFixture : DatabaseFixture, IDisposable
    {
        public ControllerFixture() : base(nameof(ControllerFixture), FixturePorts.CONTROLLER_FIXTURE_PORT)
        {
        }
    }

    [CollectionDefinition("ControllerIntegration")]
    public class ControllerTestCollection : ICollectionFixture<ControllerFixture> { }
}