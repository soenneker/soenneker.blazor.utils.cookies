using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Utils.Cookies.Tests;

[Collection("Collection")]
public sealed class CookiesUtilTests : FixturedUnitTest
{
    private readonly ICookiesUtil _blazorlibrary;

    public CookiesUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<ICookiesUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
