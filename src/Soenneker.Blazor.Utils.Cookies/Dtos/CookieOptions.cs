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

    /// <summary>Lifetime from the time the cookie is set. Cannot be combined with <see cref="Expires"/>.</summary>
    public TimeSpan? MaxAge { get; set; }

    /// <summary>Absolute expiration date. Cannot be combined with <see cref="MaxAge"/>.</summary>
    public DateTimeOffset? Expires { get; set; }

    /// <summary>If true, cookie is sent only over HTTPS.</summary>
    public bool Secure { get; set; }

    /// <summary>SameSite attribute.</summary>
    public CookieSameSite SameSite { get; set; } = CookieSameSite.Lax;
}
