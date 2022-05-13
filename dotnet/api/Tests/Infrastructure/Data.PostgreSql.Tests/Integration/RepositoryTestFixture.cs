using System;
using DylanJustice.Demo.Tests.Testing.Constants;
using DylanJustice.Demo.Tests.Testing.Fixtures;
using Xunit;

namespace Data.PostgreSql.Tests.Integration
{
    /// <summary>
    /// Purely so XUnit can dependency inject me for testing this assembly
    /// </summary>
    public class RepositoryTestFixture : DatabaseFixture, IDisposable
    {

        public RepositoryTestFixture() : base(nameof(RepositoryTestFixture), FixturePorts.REPOSITORY_FIXTURE_PORT)
        {
        }
    }

    [CollectionDefinition("Repository")]
    public class RepositoryTestCollection : ICollectionFixture<RepositoryTestFixture> { }
}
