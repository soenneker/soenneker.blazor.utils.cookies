using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Utils.Cookies.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CookiesUtilTests : HostedUnitTest
{
    private readonly ICookiesUtil _blazorlibrary;

    public CookiesUtilTests(Host host) : base(host)
    {
        _blazorlibrary = Resolve<ICookiesUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
