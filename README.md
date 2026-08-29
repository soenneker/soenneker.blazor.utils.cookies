[![](https://img.shields.io/nuget/v/soenneker.blazor.utils.cookies.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.cookies/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.cookies/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.cookies/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.utils.cookies.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.cookies/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.utils.cookies)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.cookies/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.cookies/actions/workflows/codeql.yml)

# Soenneker.Blazor.Utils.Cookies

A scoped Blazor utility for reading and writing cookies available through the browser’s `document.cookie` API.

This library is intended for client-readable preferences and similar browser state. It cannot create, read, or remove `HttpOnly` cookies; authentication and session cookies should normally be issued by the server with `HttpOnly`, `Secure`, and an appropriate `SameSite` policy.

## Installation

```bash
dotnet add package Soenneker.Blazor.Utils.Cookies
```

```csharp
using Soenneker.Blazor.Utils.Cookies.Registrars;

builder.Services.AddCookiesUtilAsScoped();
```

Inject `ICookiesUtil` after registering it:

```razor
@using Soenneker.Blazor.Utils.Cookies.Abstract
@inject ICookiesUtil Cookies
```

Calls require a browser document, so use the service after interactive rendering. It is not an HTTP response-cookie API and does not operate during server prerendering.

## Read a cookie

```csharp
string? theme = await Cookies.Get("theme");

CookieGetResult result = await Cookies.TryGet("theme");
if (result.Found)
{
    string value = result.Value!;
}

bool hasTheme = await Cookies.Exists("theme");
```

`Get` returns `null` when no client-visible cookie has that name. `TryGet` distinguishes a missing cookie from a cookie whose value is an empty string.

## Set a cookie

```csharp
await Cookies.Set("theme", "dark", new CookieOptions
{
    Path = "/",
    MaxAge = TimeSpan.FromDays(30),
    Secure = true,
    SameSite = CookieSameSite.Lax
});
```

Names and values are URI-encoded when written and decoded when read. `Path` defaults to `/`. `MaxAge` must be at least one second and cannot be combined with `Expires`. `SameSite=None` requires `Secure=true`.

Convenience overloads are available for absolute and relative expiration:

```csharp
await Cookies.Set("notice-dismissed", "yes", DateTimeOffset.UtcNow.AddDays(7));
await Cookies.Set("theme", "dark", TimeSpan.FromDays(30));
await Cookies.SetDays("theme", "dark", 30);
```

Assigning `document.cookie` does not report whether the browser accepted the cookie. Invalid domains, browser policy, size limits, or privacy settings can cause a write to be ignored. Read it back if the application needs confirmation.

## Remove a cookie

A cookie is identified by its name, path, and domain. Supply the same path and domain used when it was created:

```csharp
await Cookies.Remove("theme", new CookieOptions
{
    Path = "/",
    Domain = "example.com",
    Secure = true,
    SameSite = CookieSameSite.Lax
});
```

`Remove("theme")` targets the current host and path `/`. It cannot remove a cookie outside the current page’s domain rules or an `HttpOnly` cookie.

## List client-visible cookies

```csharp
IReadOnlyDictionary<string, string> cookies = await Cookies.GetAll();
```

The dictionary contains only cookies exposed by `document.cookie`; cookie attributes are not available. If multiple visible cookies have the same name but different paths, the dictionary cannot represent both distinctly.

## Security

Anything readable through this library is readable by JavaScript running on the page. Do not store bearer tokens, passwords, server session identifiers, or other secrets in these cookies. A successful cross-site scripting attack can read them.

Treat cookie values as untrusted input even when your application originally wrote them. Validate them before using them in authorization decisions, queries, URLs, or rendered output.
