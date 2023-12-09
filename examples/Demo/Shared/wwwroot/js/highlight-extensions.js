// Add Stylesheets
addStylesheet('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.6.0/styles/vs.min.css', 'highlight-light', null);
addStylesheet('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.6.0/styles/vs2015.min.css', 'highlight-dark', 'disabled');
addStylesheet('https://cdn.jsdelivr.net/npm/highlightjs-copy@1.0.3/dist/highlightjs-copy.min.css', null, null);

addInlineStylesheet(`pre[class~="snippet"] {
    --font-monospace: "courier";
    --type-ramp-base-font-variations: unset;
    font-weight: bold;
    }`);

// Add Scripts
const highlight = addJavaScript('https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.6.0/highlight.min.js');

// Add custom code
highlight.onload = () => {
    const hljsRazor = addJavaScript('https://cdn.jsdelivr.net/npm/highlightjs-cshtml-razor@2.1.1/dist/cshtml-razor.min.js');
    const hljsCopy = addJavaScript('https://cdn.jsdelivr.net/npm/highlightjs-copy@1.0.3/dist/highlightjs-copy.min.js');

    hljsCopy.onload = () => {
        hljs.addPlugin(new CopyButtonPlugin());
    }

    // Switch highlight Dark/Light theme
    const theme = document.querySelector('loading-theme');
    theme.addEventListener('onchange', (e) => {
        const darkCss = document.querySelector('link[title="highlight-dark"]');
        const lightCss = document.querySelector('link[title="highlight-light"]');

        const isDark = e.detail.name == 'mode' && e.detail.newValue.includes('dark');

        if (isDark) {
            darkCss.removeAttribute("disabled");
            lightCss.setAttribute("disabled", "disabled");
        }
        else {
            lightCss.removeAttribute("disabled");
            darkCss.setAttribute("disabled", "disabled");
        }
    });
}

// Add a <script> to the <body> element
function addJavaScript(src) {
    const script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = src;
    script.async = true;

    script.onerror = () => {
        // Error occurred while loading script
        console.error('Error occurred while loading script', src);
    };

    document.body.appendChild(script);

    return script;
}

// Add a <link> to the <head> element
function addStylesheet(src, title, disabled) {
    const stylesheet = document.createElement('link');
    stylesheet.rel = 'stylesheet';
    stylesheet.href = src;
    if (title) stylesheet.title = title;
    if (disabled) stylesheet.disabled = disabled;

    stylesheet.onerror = () => {
        // Error occurred while loading stylesheet
        console.error('Error occurred while loading stylesheet', src);
    };

    document.head.appendChild(stylesheet);

    return stylesheet;
}

function addInlineStylesheet(code) {
    const stylesheet = document.createElement('style');
    stylesheet.innerText = code;

    document.head.appendChild(stylesheet);

    return stylesheet;
}