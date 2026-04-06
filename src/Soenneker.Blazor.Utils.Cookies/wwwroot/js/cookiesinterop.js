function parseCookies() {
    const str = typeof document !== "undefined" ? document.cookie : "";
    const result = {};

    if (!str)
        return result;

    const pairs = str.split(";");

    for (const part of pairs) {
        const eq = part.indexOf("=");

        if (eq === -1)
            continue;

        const name = decodeURIComponent(part.slice(0, eq).trim());
        const value = decodeURIComponent(part.slice(eq + 1).trim());
        result[name] = value;
    }

    return result;
}

export function get(name) {
    const cookies = parseCookies();
    return cookies[name] ?? null;
}

export function getAll() {
    return parseCookies();
}

export function set(name, value, options) {
    if (typeof document === "undefined")
        return;

    let cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value ?? "");
    const opts = options || {};

    if (opts.path != null)
        cookie += "; path=" + opts.path;

    if (opts.domain != null)
        cookie += "; domain=" + opts.domain;

    if (opts.maxAge != null)
        cookie += "; max-age=" + opts.maxAge;

    if (opts.expires != null)
        cookie += "; expires=" + new Date(opts.expires).toUTCString();

    if (opts.secure === true)
        cookie += "; secure";

    const sameSite = opts.sameSite != null ? String(opts.sameSite).toLowerCase() : "lax";

    if (sameSite !== "lax")
        cookie += "; samesite=" + sameSite;

    document.cookie = cookie;
}

export function remove(name, options) {
    const opts = options || {};

    set(name, "", {
        path: opts.path ?? "/",
        domain: opts.domain,
        maxAge: 0,
        secure: opts.secure,
        sameSite: opts.sameSite ?? "lax"
    });
}
