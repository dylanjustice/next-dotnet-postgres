using System.Reflection;
using AndcultureCode.CSharp.Testing.Tests;
using Microsoft.Extensions.Localization;
using Moq;
using DylanJustice.Demo.Testing.Tests;
using Xunit.Abstractions;

namespace DylanJustice.Demo.Business.Conductors.Tests.Unit
{
    public class ConductorUnitTest : ApiUnitTest
    {
        public ConductorUnitTest(ITestOutputHelper output) : base(output)
        {
        }

        /// <summary>
        /// Static constructor to set up suite-level actors
        /// </summary>
        static ConductorUnitTest()
        {
            LoadFactories(typeof(ConductorUnitTest).GetTypeInfo().Assembly);
        }

    }
}
