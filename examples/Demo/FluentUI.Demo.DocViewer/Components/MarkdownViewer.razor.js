/*
    In App.razor, add these links.
      <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.10.0/styles/vs.min.css">
      <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.10.0/highlight.min.js"></script>
      <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.10.0/languages/csharp.min.js"></script>
*/
export function applyHighlight(elementId) {

    if (!hljs) {
        console.warn("highlight.js is not yet initialized.");
        return;
    }

    const element = document.getElementById(elementId);
    if (element) {
        hljs.highlightElement(element);
    }
    else {
        hljs.highlightAll();
    }
}
