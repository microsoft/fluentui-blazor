async function openCacheStorage() {
    return await window.caches.open("Microsoft.Fast.Components.FluentUI");
}

function createRequest(url, method, body = "") {
    let requestInit =
    {
        method: method
    };

    if (body != "") {
        requestInit.body = body;
    }

    let request = new Request(url, requestInit);

    return request;
}

export async function put(url, method, body = "", responseString) {
    let cache = await openCacheStorage();
    let request = createRequest(url, method, body);
    let response = new Response(responseString);
    await cache.put(request, response);
}

export async function get(url, method, body = "") {
    let cache = await openCacheStorage();
    let request = createRequest(url, method, body);
    let response = await cache.match(request);

    if (response == undefined) {
        return "";
    }

    let result = await response.text();

    return result;
}

export async function remove(url, method, body = "") {
    let cache = await openCacheStorage();
    let request = createRequest(url, method, body);
    await cache.delete(request);
}

export async function removeAll() {
    let cache = await openCacheStorage();
    let requests = await cache.keys();

    for (let request in requests) {
        await cache.delete(request);
    }
}