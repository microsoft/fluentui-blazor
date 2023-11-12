async function openCacheStorage() {
    try {
        return await window.caches.open("FluentUI.Demo")
    }
    catch (err) {
        return undefined;
    }
}

function createRequest(url: string, method: string, body = "") {
    let requestInit: { [key: string]: string } =
    {
        method: method
    };

    if (body != "") {
        requestInit.body = body;
    }

    let request = new Request(url, requestInit);

    return request;
}

export async function put(url: string, method: string, body = "", responseString: string) {
    const CACHING_DURATION = 7 * 24 * 3600;

    const expires = new Date();
    expires.setSeconds(expires.getSeconds() + CACHING_DURATION);

    const cachedResponseFields = {
        headers: { 'fluent-cache-expires': expires.toUTCString() },
    };

    let cache = await openCacheStorage();
    if (cache != null) {

        let request = createRequest(url, method, body);
        let response = new Response(responseString, cachedResponseFields);

        await cache.put(request, response);
    }
}

export async function get(url: string, method: string, body = "") {
    let cache = await openCacheStorage();
    if (cache == null) {
        return "";
    }

    let request = createRequest(url, method, body);
    let response = await cache.match(request);

    if (response == null) {
        return "";
    }
    else {
        /* TODO: Review this code */
        const expirationDate = Date.parse(response.headers.get('fluent-cache-expires')!);
        const now = new Date();
        // Check it is not already expired and return from the cache
        if (expirationDate > now.getTime()) {
            return await response.text();
        }
    }

    return "";
}

export async function remove(url: string, method: string, body = "") {
    let cache = await openCacheStorage();

    if (cache != null) {
        let request = createRequest(url, method, body);
        await cache.delete(request);
    }
}

export async function removeAll() {
    let cache = await openCacheStorage();

    if (cache != null) {
        cache.keys().then(function (names) {
            for (let name of names)
                cache!.delete(name);
        });
        //let requests = await cache.keys();

        //for (let request in requests) {
        //    await cache.delete(request);
        //}
    }
}