using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.Cookies.Abstract;
using Soenneker.Blazor.Utils.Cookies.Dtos;
using Soenneker.Blazor.Utils.Cookies.Enums;
using Soenneker.Extensions.String;

namespace Soenneker.Blazor.Utils.Cookies;

/// <inheritdoc cref="ICookiesUtil"/>
public sealed class CookiesUtil : ICookiesUtil
{
    private readonly ICookiesInterop _cookiesInterop;

    public CookiesUtil(ICookiesInterop cookiesInterop)
    {
        _cookiesInterop = cookiesInterop ?? throw new ArgumentNullException(nameof(cookiesInterop));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<string?> Get(string name, CancellationToken cancellationToken = default)
    {
        ValidateName(name);
        return _cookiesInterop.Get(name, cancellationToken);
    }

    public async ValueTask<CookieGetResult> TryGet(string name, CancellationToken cancellationToken = default)
    {
        ValidateName(name);

        string? value = await _cookiesInterop.Get(name, cancellationToken)
                                             .ConfigureAwait(false);
        return new CookieGetResult(value is not null, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<bool> Exists(string name, CancellationToken cancellationToken = default)
    {
        ValidateName(name);
        return ExistsInternal(name, cancellationToken);
    }

    public ValueTask Set(string name, string value, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        ValidateName(name);
        ArgumentNullException.ThrowIfNull(value);

        options ??= new CookieOptions();
        NormalizeOptions(options);

        return _cookiesInterop.Set(name, value, options, cancellationToken);
    }

    public ValueTask Set(string name, string value, DateTimeOffset expires, CancellationToken cancellationToken = default)
    {
        ValidateName(name);
        ArgumentNullException.ThrowIfNull(value);

        var options = new CookieOptions
        {
            Expires = expires
        };
        NormalizeOptions(options);

        return _cookiesInterop.Set(name, value, options, cancellationToken);
    }

    public ValueTask Set(string name, string value, TimeSpan maxAge, CancellationToken cancellationToken = default)
    {
        ValidateName(name);
        ArgumentNullException.ThrowIfNull(value);

        if (maxAge.TotalSeconds < 1 || maxAge.TotalSeconds > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(maxAge), maxAge, $"Cookie max age must be between one second and {int.MaxValue} seconds.");

        var options = new CookieOptions
        {
            MaxAge = maxAge
        };
        NormalizeOptions(options);

        return _cookiesInterop.Set(name, value, options, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask SetDays(string name, string value, double days, CancellationToken cancellationToken = default)
    {
        if (days <= 0)
            throw new ArgumentOutOfRangeException(nameof(days), days, "Cookie lifetime in days must be greater than zero.");

        return Set(name, value, TimeSpan.FromDays(days), cancellationToken);
    }

    public ValueTask Remove(string name, CookieOptions? options = null, CancellationToken cancellationToken = default)
    {
        ValidateName(name);

        options ??= new CookieOptions();
        NormalizeOptions(options);

        return _cookiesInterop.Remove(name, options, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask Remove(string name, CancellationToken cancellationToken)
    {
        ValidateName(name);
        return _cookiesInterop.Remove(name, null, cancellationToken);
    }

    public ValueTask<IReadOnlyDictionary<string, string>> GetAll(CancellationToken cancellationToken = default)
    {
        return _cookiesInterop.GetAll(cancellationToken);
    }

    private async ValueTask<bool> ExistsInternal(string name, CancellationToken cancellationToken)
    {
        string? value = await _cookiesInterop.Get(name, cancellationToken)
                                             .ConfigureAwait(false);
        return value is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateName(string name)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentException("Cookie name cannot be null or whitespace.", nameof(name));
    }

    private static void NormalizeOptions(CookieOptions? options)
    {
        if (options is null)
            return;

        if (options.Path.IsNullOrWhiteSpace())
            options.Path = "/";

        ValidateAttributeValue(options.Path, nameof(options.Path));

        if (options.Domain is not null)
            ValidateAttributeValue(options.Domain, nameof(options.Domain));

        if (options.Expires.HasValue && options.MaxAge.HasValue)
            throw new ArgumentException("Only one of Expires or MaxAge should be set on CookieOptions.", nameof(options));

        if (options.MaxAge.HasValue && (options.MaxAge.Value.TotalSeconds < 1 || options.MaxAge.Value.TotalSeconds > int.MaxValue))
            throw new ArgumentOutOfRangeException(nameof(options), options.MaxAge, $"Cookie MaxAge must be between one second and {int.MaxValue} seconds.");

        if (options.SameSite == CookieSameSite.None && !options.Secure)
            throw new ArgumentException("Cookies using SameSite=None should also specify Secure=true.", nameof(options));
    }

    private static void ValidateAttributeValue(string value, string parameterName)
    {
        if (value.IndexOfAny([';', '\r', '\n']) >= 0)
            throw new ArgumentException("Cookie attribute values cannot contain semicolons or line breaks.", parameterName);
    }
}
