namespace Soenneker.Blazor.Utils.Cookies.Dtos;

/// <summary>
/// Result of a try-get cookie operation.
/// </summary>
public readonly struct CookieGetResult
{
    /// <summary>Whether the cookie was found.</summary>
    public bool Found { get; }

    /// <summary>The cookie value when <see cref="Found"/> is true; otherwise null.</summary>
    public string? Value { get; }

    public CookieGetResult(bool found, string? value)
    {
        Found = found;
        Value = value;
    }
}
