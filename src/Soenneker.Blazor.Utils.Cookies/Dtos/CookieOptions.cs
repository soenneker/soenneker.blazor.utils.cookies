using System;
using Soenneker.Blazor.Utils.Cookies.Enums;

namespace Soenneker.Blazor.Utils.Cookies.Dtos;

/// <summary>
/// Options for setting a cookie (path, domain, expiration, etc.).
/// </summary>
public sealed class CookieOptions
{
    /// <summary>Path for the cookie. Default is "/".</summary>
    public string? Path { get; set; }

    /// <summary>Domain for the cookie.</summary>
    public string? Domain { get; set; }

    /// <summary>Max age. Takes precedence over <see cref="Expires"/> when set.</summary>
    public TimeSpan? MaxAge { get; set; }

    /// <summary>Expiration date. Ignored when <see cref="MaxAge"/> is set.</summary>
    public DateTimeOffset? Expires { get; set; }

    /// <summary>If true, cookie is sent only over HTTPS.</summary>
    public bool Secure { get; set; }

    /// <summary>SameSite attribute.</summary>
    public CookieSameSite SameSite { get; set; } = CookieSameSite.Lax;
}
