using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Blazor.Utils.Cookies.Dtos;
using Soenneker.Blazor.Utils.Cookies.Enums;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Extensions.String;

namespace Soenneker.Blazor.Utils.Cookies;

/// <inheritdoc cref="ICookiesInterop"/>
public sealed class CookiesInterop : ICookiesInterop
{
    private const string _modulePath = "/_content/Soenneker.Blazor.Utils.Cookies/js/cookiesinterop.js";

    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly CancellationScope _cancellationScope = new();

    public CookiesInterop(IModuleImportUtil moduleImportUtil)
    {
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask<string?> Get(string name, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return null;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<string?>("get", linked, name);
        }
    }

    public async ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("set", linked, name, value ?? "", ToJsOptions(options));
        }
    }

    public async ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (name.IsNullOrWhiteSpace())
            return;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("remove", linked, name, ToJsOptions(options));
        }
    }

    public async ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            var dict = await module.InvokeAsync<Dictionary<string, string>>("getAll", linked);
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
        await _moduleImportUtil.DisposeContentModule(_modulePath);
        await _cancellationScope.DisposeAsync();
    }
}
