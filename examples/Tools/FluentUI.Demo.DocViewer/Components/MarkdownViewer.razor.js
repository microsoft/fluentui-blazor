function highlightElement(element) {
  if (element.dataset.highlighted) {
    globalThis.hljs.unhighlightElement(element);
  }

  globalThis.hljs.highlightElement(element);
}

async function refreshTabs() {
  await globalThis.customElements.whenDefined("fluent-tablist");

  for (const container of document.querySelectorAll(".doc-viewer .demo-tabs")) {
    const tablist = container.querySelector(":scope > fluent-tablist");
    const tabs = tablist?.querySelectorAll(":scope > fluent-tab");
    const panels = container.querySelectorAll(":scope > [role='tabpanel']");

    if (!tablist || tabs.length !== panels.length) {
      console.error("Unable to initialize documentation tabs: tabs and panels do not match.");
      continue;
    }

    tabs.forEach((tab, index) => {
      const panelId = panels[index].id;
      tab.id = panelId.endsWith("-panel") ? panelId.slice(0, -6) : `${panelId}-tab`;
      tab.setAttribute("aria-controls", panelId);
    });

    tablist.replaceWith(tablist.cloneNode(true));
  }
}

export async function initialize() {
  if (!globalThis.hljs) {
    console.warn("highlight.js is not yet initialized.");
    return;
  }

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
