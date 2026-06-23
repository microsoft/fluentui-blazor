import { DotNet } from "../../../Core.Scripts/src/d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Overflow {
  interface OverflowItem {
    Id: string;
    Overflow: boolean;
    Text: string;
    Fixed?: string | null;
    Index?: number;
  }

  interface OverflowElement extends HTMLElement {
    overflowSize?: number | null;
  }

  interface OverflowElementHost extends HTMLElement {
    refresh?: () => void;
    getOverflowItems?: () => OverflowItem[];
  }

  interface ObserverContext {
    resizeObserver: ResizeObserver;
    mutationObserver: MutationObserver;
    resizeTimeout?: number;
    lastHandledState: boolean | null;
    listener?: EventListener;
  }

  interface RefreshResult {
    items: OverflowItem[];
    overflowChanged: boolean;
    isHorizontal: boolean;
  }

  const observerContexts = new Map<string, ObserverContext>();

  class FluentOverflow extends HTMLElement {
    private resizeObserver?: ResizeObserver;
    private mutationObserver?: MutationObserver;
    private resizeTimeout?: number;
    private lastHandledState: boolean | null = null;
    private overflowItems: OverflowItem[] = [];

    static get observedAttributes() {
      return ["orientation", "selector", "selectors", "threshold", "visible-on-load", "store-overflow-in-memory"];
    }

    connectedCallback() {
      this.classList.add("fluent-overflow");
      this.setupObservers();
      this.refresh();
      this.setAttribute("visible-on-load", "true");
      this.style.visibility = "";
    }

    disconnectedCallback() {
      this.cleanupObservers();
    }

    attributeChangedCallback(name: string, oldValue: string | null, newValue: string | null) {
      if (oldValue === newValue) {
        return;
      }

      if (name === "visible-on-load") {
        this.style.visibility = newValue === "false" ? "hidden" : "";
        return;
      }

      this.refresh();
    }

    refresh() {
      const result = refreshContainer(this, this.getIsHorizontal(), this.getQuerySelector(), this.getThreshold(), this.lastHandledState);
      this.lastHandledState = result.isHorizontal;

      if (this.getStoreOverflowInMemory()) {
        this.overflowItems = result.items;
      }

      if (result.overflowChanged) {
        if (!this.getStoreOverflowInMemory()) {
          this.overflowItems = result.items;
        }

        this.dispatchEvent(new CustomEvent("overflowchange", {
          detail: {
            items: result.items,
            overflowCount: result.items.filter(i => i.Overflow).length
          },
          bubbles: true,
          composed: true
        }));
      }
    }

    getOverflowItems(): OverflowItem[] {
      if (this.overflowItems.length > 0) {
        return [...this.overflowItems];
      }

      return getCurrentItems(this, this.getQuerySelector());
    }

    getOverflowCount(): number {
      return this.getOverflowItems().filter(i => i.Overflow).length;
    }

    private setupObservers() {
      this.cleanupObservers();

      if (typeof ResizeObserver !== "undefined") {
        this.resizeObserver = new ResizeObserver(() => {
          clearTimeout(this.resizeTimeout);
          this.resizeTimeout = window.setTimeout(() => this.refresh(), 100);
        });
        this.resizeObserver.observe(this);
      }

      this.mutationObserver = new MutationObserver(() => this.refresh());
      this.mutationObserver.observe(this, { childList: true, subtree: false });
    }

    private cleanupObservers() {
      this.resizeObserver?.disconnect();
      this.resizeObserver = undefined;
      this.mutationObserver?.disconnect();
      this.mutationObserver = undefined;
      clearTimeout(this.resizeTimeout);
      this.resizeTimeout = undefined;
    }

    private getIsHorizontal(): boolean {
      const orientation = this.getAttribute("orientation");
      return orientation !== "vertical";
    }

    private getThreshold(): number {
      const value = Number.parseFloat(this.getAttribute("threshold") ?? "25");
      return Number.isFinite(value) ? value : 25;
    }

    private getStoreOverflowInMemory(): boolean {
      const value = this.getAttribute("store-overflow-in-memory");
      return value === "true" || value === "";
    }

    private getQuerySelector(): string | null {
      return this.getAttribute("selector") ?? this.getAttribute("selectors");
    }
  }

  if (!customElements.get("fluent-overflow")) {
    customElements.define("fluent-overflow", FluentOverflow);
  }

  export function Initialize(dotNetHelper: DotNet.DotNetObject, id: string, isHorizontal: boolean, querySelector: string | null, threshold: number): void {
    const container = document.getElementById(id) as OverflowElementHost | null;
    if (!container) {
      return;
    }

    Dispose(id);

    const listener: EventListener = (event) => {
      const overflowEvent = event as CustomEvent<{ items: OverflowItem[] }>;
      const items = overflowEvent.detail?.items ?? [];
      dotNetHelper.invokeMethodAsync("OverflowRaisedAsync", items);
    };

    container.addEventListener("overflowchange", listener);

    if (container.tagName.toLowerCase() === "fluent-overflow" && typeof container.refresh === "function") {
      container.setAttribute("orientation", isHorizontal ? "horizontal" : "vertical");
      container.setAttribute("threshold", String(threshold));
      if (querySelector) {
        container.setAttribute("selector", querySelector);
      } else {
        container.removeAttribute("selector");
      }

      observerContexts.set(id, {
        resizeObserver: new ResizeObserver(() => { }),
        mutationObserver: new MutationObserver(() => { }),
        listener,
        lastHandledState: isHorizontal
      });

      container.refresh();
      return;
    }

    const context: ObserverContext = {
      resizeObserver: new ResizeObserver(() => { }),
      mutationObserver: new MutationObserver(() => { }),
      listener,
      lastHandledState: isHorizontal
    };

    context.mutationObserver = new MutationObserver(mutations => {
      for (const mutation of mutations) {
        if (mutation.type !== "childList") {
          continue;
        }

        const node = mutation.addedNodes.length > 0 ? mutation.addedNodes[0] : mutation.removedNodes[0];
        if (node?.nodeType !== Node.ELEMENT_NODE) {
          continue;
        }

        const selector = buildQuerySelector(querySelector);
        if (!(node as Element).matches(selector)) {
          continue;
        }

        Refresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
        break;
      }
    });

    context.resizeObserver = new ResizeObserver(() => {
      clearTimeout(context.resizeTimeout);
      context.resizeTimeout = window.setTimeout(() => {
        Refresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
      }, 100);
    });

    context.resizeObserver.observe(container);
    context.mutationObserver.observe(container, { childList: true, subtree: false });
    observerContexts.set(id, context);

    Refresh(dotNetHelper, id, isHorizontal, querySelector, threshold);
  }

  export function Refresh(dotNetHelper: DotNet.DotNetObject, id: string, isHorizontal: boolean, querySelector: string | null, threshold: number): void {
    const container = document.getElementById(id) as OverflowElementHost | null;
    if (!container) {
      return;
    }

    if (container.tagName.toLowerCase() === "fluent-overflow" && typeof container.refresh === "function") {
      container.refresh();
      if (typeof container.getOverflowItems === "function") {
        dotNetHelper.invokeMethodAsync("OverflowRaisedAsync", container.getOverflowItems());
      }
      return;
    }

    const context = observerContexts.get(id);
    const result = refreshContainer(container, isHorizontal, querySelector, threshold, context?.lastHandledState ?? null);
    if (context) {
      context.lastHandledState = isHorizontal;
    }

    if (result.overflowChanged) {
      dotNetHelper.invokeMethodAsync("OverflowRaisedAsync", result.items);
    }
  }

  export function Dispose(id: string): void {
    const context = observerContexts.get(id);
    if (!context) {
      return;
    }

    const container = document.getElementById(id);
    if (container && context.listener) {
      container.removeEventListener("overflowchange", context.listener);
    }

    context.resizeObserver.disconnect();
    context.mutationObserver.disconnect();
    if (context.resizeTimeout) {
      clearTimeout(context.resizeTimeout);
    }

    observerContexts.delete(id);
  }

  function refreshContainer(container: HTMLElement, isHorizontal: boolean, querySelector: string | null, threshold: number, lastHandledState: boolean | null): RefreshResult {
    const localQuerySelector = buildQuerySelector(querySelector);
    const allItems = Array.from(container.querySelectorAll<OverflowElement>(localQuerySelector));
    const items = allItems.filter(element => !element.hasAttribute("fixed"));
    const fixedItems = allItems.filter(element => element.hasAttribute("fixed") && element.getAttribute("fixed") !== "ellipsis");
    const ellipsisItems = allItems.filter(element => element.getAttribute("fixed") === "ellipsis");

    let ellipsisTotal = 0;
    let containerGap = parseFloat(window.getComputedStyle(container).gap);
    if (!containerGap) {
      containerGap = 0;
    }

    ellipsisItems.forEach((element, index) => {
      element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
      ellipsisTotal += element.overflowSize || 0;
      if (index > 0) {
        ellipsisTotal += containerGap;
      }
    });

    let itemsTotalSize = threshold > 0 ? 10 : 0;
    let containerMaxSize = isHorizontal ? container.offsetWidth : container.offsetHeight;
    let overflowChanged = false;
    containerMaxSize -= threshold;

    const availableSize = containerMaxSize - fixedItems.reduce((sum, element, index) => sum + (element.overflowSize || 0) + (index > 0 ? containerGap : 0), 0);

    if (ellipsisTotal > availableSize) {
      ellipsisItems.forEach(element => {
        element.style.flexShrink = "1";
      });
    } else {
      ellipsisItems.forEach(element => {
        element.style.flexShrink = "0";
      });
    }

    if (lastHandledState !== null && lastHandledState !== isHorizontal) {
      allItems.forEach(element => {
        element.removeAttribute("overflow");
        element.overflowSize = null;
      });
    }

    fixedItems.forEach(element => {
      element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
      element.overflowSize = (element.overflowSize || 0) + containerGap;
      itemsTotalSize += element.overflowSize;
    });

    items.forEach(element => {
      const isOverflow = element.hasAttribute("overflow");
      if (!isOverflow) {
        element.overflowSize = isHorizontal ? getElementWidth(element) : getElementHeight(element);
        element.overflowSize = (element.overflowSize || 0) + containerGap;
      }
      itemsTotalSize += element.overflowSize || 0;
      if (containerMaxSize > 0) {
        if (itemsTotalSize > containerMaxSize) {
          if (!isOverflow) {
            element.setAttribute("overflow", "");
            overflowChanged = true;
          }
        } else {
          if (isOverflow) {
            element.removeAttribute("overflow");
            overflowChanged = true;
          }
        }
      }
    });

    return {
      items: toOverflowItems(items),
      overflowChanged,
      isHorizontal
    };
  }

  function getCurrentItems(container: HTMLElement, querySelector: string | null): OverflowItem[] {
    const localQuerySelector = buildQuerySelector(querySelector);
    const items = Array.from(container.querySelectorAll<OverflowElement>(localQuerySelector))
      .filter(element => !element.hasAttribute("fixed"));
    return toOverflowItems(items);
  }

  function toOverflowItems(items: OverflowElement[]): OverflowItem[] {
    return items.map((element, index) => ({
      Id: element.id,
      Overflow: element.hasAttribute("overflow"),
      Text: element.innerText.trim(),
      Fixed: element.getAttribute("fixed"),
      Index: index
    }));
  }

  function buildQuerySelector(querySelector: string | null): string {
    if (!querySelector) {
      return ":scope > :not(.fluent-overflow-more)";
    }
    return `:scope > ${querySelector}`;
  }

  function getElementWidth(element: HTMLElement): number {
    const style = window.getComputedStyle(element);
    const width = element.offsetWidth;
    const margin = parseFloat(style.marginLeft) + parseFloat(style.marginRight);
    return width + margin;
  }

  function getElementHeight(element: HTMLElement): number {
    const style = window.getComputedStyle(element);
    const height = element.offsetHeight;
    const margin = parseFloat(style.marginTop) + parseFloat(style.marginBottom);
    return height + margin;
  }
}
