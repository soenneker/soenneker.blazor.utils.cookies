function parseCookies() {
    const str = typeof document !== "undefined" ? document.cookie : "";
    const result = Object.create(null);

    if (!str)
        return result;

    const pairs = str.split(";");

    for (const part of pairs) {
        const eq = part.indexOf("=");

        if (eq === -1)
            continue;

        const name = decode(part.slice(0, eq).trim());
        const value = decode(part.slice(eq + 1).trim());
        result[name] = value;
    }

    return result;
}

function decode(value) {
    try {
        return decodeURIComponent(value);
    } catch {
        return value;
    }
}

function validateAttribute(value, name) {
    if (value != null && /[;\r\n]/.test(value))
        throw new Error(`${name} cannot contain semicolons or line breaks.`);
}

export function get(name) {
    if (typeof name !== "string")
        return parseCookies()[name] ?? null;

    const str = typeof document !== "undefined" ? document.cookie : "";
    let result = null;
    let start = 0;
    let eq = str.indexOf("=");
    while (start < str.length) {
        let end = str.indexOf(";", start);
        if (end === -1)
            end = str.length;
        if (eq !== -1 && eq < end) {
            if (decode(str.slice(start, eq).trim()) === name)
                result = decode(str.slice(eq + 1, end).trim());
            eq = str.indexOf("=", end + 1);
        }
        start = end + 1;
    }
    return result;
}

export function getAll() {
    return parseCookies();
}

export function set(name, value, options) {
    if (typeof document === "undefined")
        return;

    let cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value ?? "");
    const opts = options || {};

    validateAttribute(opts.path, "Cookie path");
    validateAttribute(opts.domain, "Cookie domain");

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
