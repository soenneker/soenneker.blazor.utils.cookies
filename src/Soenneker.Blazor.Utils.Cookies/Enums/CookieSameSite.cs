namespace Soenneker.Blazor.Utils.Cookies.Enums;

/// <summary>
/// SameSite cookie attribute values.
/// </summary>
public enum CookieSameSite
{
    /// <summary>Strict - cookie only sent in first-party context.</summary>
    Strict,

    /// <summary>Lax - cookie sent on top-level navigations (default in modern browsers).</summary>
    Lax,

    /// <summary>None - cookie sent in all contexts (requires Secure).</summary>
    None
}