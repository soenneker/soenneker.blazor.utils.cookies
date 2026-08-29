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
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get.</returns>
    ValueTask<string?> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value. Optional options for path, domain, expiration, and security.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="value">Cookie value to store.</param>
    /// <param name="options">Options to configure for the cookies.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the set operation is complete.</returns>
    ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="options">Options to configure for the cookies.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the remove operation is complete.</returns>
    ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all cookies as a read-only dictionary of name to value.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested read Only Dictionary.</returns>
    ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default);
}
