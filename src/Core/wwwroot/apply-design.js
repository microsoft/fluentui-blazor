// To avoid Flash of Unstyled Content, the body is hidden.
// Here we'll find the first web component and wait for it to be upgraded.
// When it is, we'll remove this visibility from the body.

const defaultDarkColor = "#272727";
const defaultLightColor = "#fbfbfb";

const storageName = "theme"; // TODO: to create a function to send arguments
                             // see https://stackoverflow.com/questions/2190801/passing-parameters-to-javascript-files

// Compute the saved or the system theme (dark/light).
const className = "hidden-unstyled-body";
const isDarkSaved = JSON.parse(localStorage.getItem(storageName))?.mode === "dark";
const isSystemDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
const bgColor = isDarkSaved || isSystemDark ? defaultDarkColor : defaultLightColor;
    ;
// Create a ".hidden-unstyled-body"" class
// where the background-color is dark or light.
var css = `.${className} { visibility: hidden; background-color: ${bgColor}; }`;

// Add a <style> element to the <head> element
const head = document.head || document.getElementsByTagName('head')[0];
const style = document.createElement('style');

head.appendChild(style);
style.appendChild(document.createTextNode(css));

document.body.classList.add(className);

// Add a <fluent-design-theme storage-name="{<theme>}" /> element
const designTheme = document.createElement("fluent-design-theme");
designTheme.setAttribute("storage-name", storageName);
document.body.appendChild(designTheme);

const undefinedElements = document.body.querySelector(":not(:defined)");

if (undefinedElements) {
    customElements.whenDefined(undefinedElements.localName).then(() => {
        document.body.classList.remove(className);
    });
}