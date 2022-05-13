using System;
using DylanJustice.Demo.Tests.Testing.Constants;
using DylanJustice.Demo.Tests.Testing.Fixtures;
using Xunit;

namespace DylanJustice.Demo.Business.Conductors.Tests.Integration
{
    /// <summary>
    /// Purely so XUnit can dependency inject me for testing this assembly
    /// </summary>
    public class ConductorFixture : DatabaseFixture, IDisposable
    {
        public ConductorFixture() : base(nameof(ConductorFixture), FixturePorts.CONDUCTOR_FIXTURE_PORT)
        {
        }
    }

    [CollectionDefinition("ConductorIntegration")]
    public class ConductorTestCollection : ICollectionFixture<ConductorFixture> { }
}
