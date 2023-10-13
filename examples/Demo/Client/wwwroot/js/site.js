
let matched = window.matchMedia('(prefers-color-scheme: dark)').matches;

if (matched) {
    window.DefaultBaseLayerLuminance = 0.23;
    document.querySelector(`link[title="dark"]`).removeAttribute("disabled");
    document.querySelector(`link[title="light"]`).setAttribute("disabled", "disabled");
} else {
    window.DefaultBaseLayerLuminance = 1.0;
    document.querySelector(`link[title="light"]`).removeAttribute("disabled");
    document.querySelector(`link[title="dark"]`).setAttribute("disabled", "disabled");
}
