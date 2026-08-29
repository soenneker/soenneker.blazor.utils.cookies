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
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get.</returns>
    ValueTask<string?> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to retrieve the entry for the specified key without creating a new value.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested cookie Get Result.</returns>
    ValueTask<CookieGetResult> TryGet(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns whether a cookie with the given name exists.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if a cookie with the specified name exists; otherwise, false.</returns>
    ValueTask<bool> Exists(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value. Optional options for path, domain, expiration, etc.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="value">Cookie value to store.</param>
    /// <param name="options">Options to configure for the cookies.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the set operation is complete.</returns>
    ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name, value, and expiration date.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="value">Cookie value to store.</param>
    /// <param name="expires">Expires for the set operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the set operation is complete.</returns>
    ValueTask Set(string name, string value, DateTimeOffset expires, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name, value, and max age.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="value">Cookie value to store.</param>
    /// <param name="maxAge">Max Age for the set operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the set operation is complete.</returns>
    ValueTask Set(string name, string value, TimeSpan maxAge, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cookie with the given name and value, expiring after the specified number of days.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="value">Cookie value to store.</param>
    /// <param name="days">Days for the set days operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the days has been stored.</returns>
    ValueTask SetDays(string name, string value, double days, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name (sets max-age=0). Uses path "/" and current host unless options are provided.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="options">Options to configure for the cookies.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the remove operation is complete.</returns>
    ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cookie by name with default path.
    /// </summary>
    /// <param name="name">Name of the Cookies value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the remove operation is complete.</returns>
    ValueTask Remove(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all cookies as a read-only dictionary of name -> value.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested read Only Dictionary.</returns>
    ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default);
}
