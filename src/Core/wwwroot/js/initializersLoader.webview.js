function setupLoader() {
    const { Blazor: blazor } = window;

    if (typeof blazor !== 'undefined') {
        throw new Error("Blazor has been already registered. initializersLoader JS file must be added before Blazor's JS file.");
    }

    var appName = getAppName();
    let modulesResource = `${appName}.modules.json`;

    const { fetch: originalFetch } = window;

    window.fetch = async (...args) => {
        let [resource, config] = args;

        if (resource === '_framework/blazor.modules.json') {
            resource = modulesResource;
            window.fetch = originalFetch;
        }

        const response = await originalFetch(resource, config);
        return response;
    }
}

function getAppName() {
    let appName;

    if (document &&
        document.currentScript) {
        appName = document.currentScript.getAttribute('app-name');
    }

    if (!appName) {
        throw new Error("'app-name' attribute is missing on script tag, it's required to resolve JS Modules Manifest file.");
    }

    return appName;
}

setupLoader();