function highlightElement(element) {
  if (element.dataset.highlighted) {
    globalThis.hljs.unhighlightElement(element);
  }

  globalThis.hljs.highlightElement(element);
}

async function refreshTabs() {
  await globalThis.customElements.whenDefined("fluent-tablist");

  for (const tablist of document.querySelectorAll(".doc-viewer fluent-tablist")) {
    if (tablist.getAttribute("role") !== "tablist") {
      tablist.replaceWith(tablist.cloneNode(true));
    }
    else {
      tablist.tabsChanged();
    }
  }
}

export async function initialize() {
  if (!globalThis.hljs) {
    console.warn("highlight.js is not yet initialized.");
    return;
  }
  await refreshTabs();
  await refreshTabs();

  for (const element of document.querySelectorAll("[data-highlight]")) {
    highlightElement(element);
  }

  const sourceElements = document.querySelectorAll("[data-source-url]");

  for (const element of sourceElements) {
    const sourceUrl = element.dataset.sourceUrl;
    if (element.dataset.sourceLoadingUrl === sourceUrl) {
      continue;
    }

    element.dataset.sourceLoadingUrl = sourceUrl;

    try {
      const response = await fetch(sourceUrl);
      if (!response.ok) {
        throw new Error(`HTTP ${response.status} ${response.statusText}`);
      }

      const source = await response.text();
      if (!element.isConnected || element.dataset.sourceUrl !== sourceUrl) {
        continue;
      }

      element.textContent = source;
      highlightElement(element);
    }
    catch (error) {
      console.error(`Error loading the file '${sourceUrl}':`, error);
    }
    finally {
      if (element.dataset.sourceLoadingUrl === sourceUrl) {
        delete element.dataset.sourceLoadingUrl;
      }
    }
  }
}
