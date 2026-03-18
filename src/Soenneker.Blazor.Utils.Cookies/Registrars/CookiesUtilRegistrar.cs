using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Utils.Cookies.Registrars;

/// <summary>
/// Registration for cookie interop and util services.
/// </summary>
public static class CookiesUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ICookiesInterop"/> and <see cref="ICookiesUtil"/> as scoped services.
    /// Registers <see cref="Soenneker.Blazor.Utils.ResourceLoader.Abstract.IResourceLoader"/> as scoped when needed.
    /// </summary>
    public static IServiceCollection AddCookiesUtilAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped()
                .TryAddScoped<ICookiesInterop, CookiesInterop>();

        services.TryAddScoped<ICookiesUtil, CookiesUtil>();

        return services;
    }
}