[![](https://img.shields.io/nuget/v/soenneker.blazor.utils.cookies.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.cookies/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.cookies/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.cookies/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.utils.cookies.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.cookies/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.utils.cookies)

# Soenneker.Blazor.Utils.Cookies

Blazor library for reading, writing, removing, and listing browser cookies. Exposes a low-level `ICookiesInterop` for direct browser interop and a higher-level `ICookiesUtil` for app-facing usage.

---

## Install

```bash
dotnet add package Soenneker.Blazor.Utils.Cookies
```

---

## Setup

Register cookie services in `Program.cs`:

```csharp
builder.Services.AddCookiesUtilAsScoped();
```

Inject the higher-level utility in components/services:

```csharp
@inject ICookiesUtil Cookies
```

---

## Usage

### Get a cookie

```csharp
string? theme = await Cookies.Get("theme");
```

### Try get a cookie

```csharp
CookieGetResult result = await Cookies.TryGet("theme");

if (result.Found)
{
    Console.WriteLine(result.Value);
}
```

### Set a cookie

```csharp
await Cookies.Set("theme", "dark", new CookieOptions
{
    Path = "/",
    MaxAge = TimeSpan.FromDays(30),
    Secure = true,
    SameSite = CookieSameSite.Lax
});
```

### Remove a cookie

```csharp
await Cookies.Remove("theme");
```

### Read all cookies

```csharp
IReadOnlyDictionary<string, string> cookies = await Cookies.GetAll();

foreach ((string key, string value) in cookies)
{
    Console.WriteLine($"{key}={value}");
}
```