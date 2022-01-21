using Lombiq.Tests.Helpers;
using Shouldly;
using Xunit;

namespace Lombiq.OSOCE.NuGet.TestTests
{
    public class MockHelperTests
    {
        [Fact]
        public void AutoMockerShouldBeInstantiated() => MockHelper.CreateAutoMockerInstance<MockedClass>().ShouldNotBeNull();
    }
}
