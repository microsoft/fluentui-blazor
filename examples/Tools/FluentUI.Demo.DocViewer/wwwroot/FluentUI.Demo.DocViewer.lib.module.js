import { initialize } from "./Components/MarkdownViewer.razor.js";

let enhancedLoadRegistered = false;

function initializeMarkdownViewer(blazor) {
  initialize();

  if (!enhancedLoadRegistered && typeof blazor.addEventListener === "function") {
    blazor.addEventListener("enhancedload", initialize);
    enhancedLoadRegistered = true;
  }
}

export function afterStarted(blazor) {
  initializeMarkdownViewer(blazor);
}

export function afterWebStarted(blazor) {
  initializeMarkdownViewer(blazor);
}

export function afterWebAssemblyStarted(blazor) {
  initializeMarkdownViewer(blazor);
}
