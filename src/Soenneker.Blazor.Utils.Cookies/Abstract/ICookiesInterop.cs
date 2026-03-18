using Soenneker.Blazor.Utils.Cookies.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Utils.Cookies.Abstract;

/// <summary>
/// Blazor interop for browser cookie management via <c>document.cookie</c>.
/// </summary>
public interface ICookiesInterop : IAsyncDisposable
{
    /// <summary>
    /// Gets the value of a cookie by name, or null if not found.
    /// </summary>
    ValueTask<string?> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value. Optional options for path, domain, expiration, and security.
    /// </summary>
    ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name.
    /// </summary>
    ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all cookies as a read-only dictionary of name to value.
    /// </summary>
    ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default);
}
