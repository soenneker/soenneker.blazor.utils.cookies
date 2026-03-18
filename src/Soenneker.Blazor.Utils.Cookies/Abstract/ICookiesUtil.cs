using Soenneker.Blazor.Utils.Cookies.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Utils.Cookies.Abstract;

/// <summary>
/// A higher-level Blazor utility for cookie management built on top of <see cref="ICookiesInterop"/>.
/// </summary>
public interface ICookiesUtil
{
    /// <summary>
    /// Gets the value of a cookie by name, or null if not found.
    /// </summary>
    ValueTask<string?> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to get the value of a cookie. Returns a result with <see cref="CookieGetResult.Found"/> true and <see cref="CookieGetResult.Value"/> when found; otherwise found is false and value is null.
    /// </summary>
    ValueTask<CookieGetResult> TryGet(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns whether a cookie with the given name exists.
    /// </summary>
    ValueTask<bool> Exists(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value. Optional options for path, domain, expiration, etc.
    /// </summary>
    ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name, value, and expiration date.
    /// </summary>
    ValueTask Set(string name, string value, DateTimeOffset expires, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name, value, and max age.
    /// </summary>
    ValueTask Set(string name, string value, TimeSpan maxAge, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value, expiring after the specified number of days.
    /// </summary>
    ValueTask SetDays(string name, string value, double days, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name (sets max-age=0). Uses path "/" and current host unless options are provided.
    /// </summary>
    ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name with default path.
    /// </summary>
    ValueTask Remove(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all cookies as a read-only dictionary of name -> value.
    /// </summary>
    ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default);
}
