using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Blazor.Utils.Cookies.Dtos;
using Soenneker.Blazor.Utils.Cookies.Enums;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Extensions.String;

namespace Soenneker.Blazor.Utils.Cookies;

/// <inheritdoc cref="ICookiesInterop"/>
public sealed class CookiesInterop : ICookiesInterop
{
    private const string _modulePath = "Soenneker.Blazor.Utils.Cookies/js/cookiesinterop.js";
    private const string _jsGet = "CookiesInterop.get";
    private const string _jsGetAll = "CookiesInterop.getAll";
    private const string _jsSet = "CookiesInterop.set";
    private const string _jsRemove = "CookiesInterop.remove";

    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncInitializer _initializer;
    private readonly CancellationScope _cancellationScope = new();

    private bool _disposed;

    public CookiesInterop(IJSRuntime jsRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jsRuntime;
        _resourceLoader = resourceLoader;
        _initializer = new AsyncInitializer(Initialize);
    }

    private async ValueTask Initialize(CancellationToken token)
    {
        _ = await _resourceLoader.ImportModule(_modulePath, token);
    }

    private async ValueTask EnsureInitialized(CancellationToken cancellationToken)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _initializer.Init(linked);
        }
    }

    public async ValueTask<string?> Get(string name, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return null;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            return await _jsRuntime.InvokeAsync<string?>(_jsGet, linked, name);
        }
    }

    public async ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            await _jsRuntime.InvokeVoidAsync(_jsSet, linked, name, value ?? "", ToJsOptions(options));
        }
    }

    public async ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            await _jsRuntime.InvokeVoidAsync(_jsRemove, linked, name, ToJsOptions(options));
        }
    }

    public async ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            var dict = await _jsRuntime.InvokeAsync<Dictionary<string, string>>(_jsGetAll, linked);
            return dict ?? [];
        }
    }

    private static object? ToJsOptions(CookieOptions? options)
    {
        if (options == null)
            return null;

        return new
        {
            path = options.Path,
            domain = options.Domain,
            maxAge = options.MaxAge.HasValue ? (int)options.MaxAge.Value.TotalSeconds : (int?)null,
            expires = options.Expires?.ToUniversalTime()
                             .ToString("O"),
            secure = options.Secure,
            sameSite = options.SameSite switch
            {
                CookieSameSite.Strict => "strict",
                CookieSameSite.Lax => "lax",
                CookieSameSite.None => "none",
                _ => "lax"
            }
        };
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        await _resourceLoader.DisposeModule(_modulePath);
        await _initializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}