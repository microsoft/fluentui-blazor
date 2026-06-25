import { StartedMode } from "../../d-ts/StartedMode";

export namespace Microsoft.FluentUI.Blazor.Components.Overflow {
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

  interface OverflowState {
    overflowItems: OverflowItem[];
    overflowCount: number;
    firstOverflowIndex: number;
  }

  interface RefreshResult extends OverflowState {
    overflowChanged: boolean;
    isHorizontal: boolean;
  }

  class FluentOverflow extends HTMLElement {
    private resizeObserver?: ResizeObserver;
    private mutationObserver?: MutationObserver;
    private resizeTimeout?: number;
    private mutationTimeout?: number;
    private lastHandledState: boolean | null = null;
    private lastContainerSize = 0;
    private cachedContainerGap: number | null = null;
    private lastOverflowCount = 0;
    private lastFirstOverflowIndex = -1;
    private overflowItems: OverflowItem[] = [];

    static get observedAttributes() {
      return ["orientation", "selector", "selectors", "threshold", "visible-on-load", "store-overflow-in-memory", "max-rendered-items"];
    }

    connectedCallback() {
      const visibleOnLoad = this.getAttribute("visible-on-load") !== "false";

      this.cachedContainerGap = null;
      this.lastContainerSize = 0;
      this.lastOverflowCount = 0;
      this.lastFirstOverflowIndex = -1;
      this.classList.add("fluent-overflow");
      this.setupObservers();
      this.refresh();

      // Reveal after first measurement if the element was initially hidden on load.
      if (!visibleOnLoad) {
        this.style.visibility = "";
      }
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

      if (name === "orientation") {
        this.lastContainerSize = 0;
        this.cachedContainerGap = null;
      }

      this.refresh();
    }

    refresh() {
      const result = refreshContainer(
        this,
        this.getIsHorizontal(),
        this.getQuerySelector(),
        this.getThreshold(),
        this.lastHandledState,
        this.getContainerGap(),
        this.getMaxRenderedItems()
      );
      this.lastHandledState = result.isHorizontal;

      const payloadChanged = result.overflowChanged
        || this.lastOverflowCount !== result.overflowCount
        || this.lastFirstOverflowIndex !== result.firstOverflowIndex
        || !areOverflowItemsEqual(this.overflowItems, result.overflowItems);

      this.lastOverflowCount = result.overflowCount;
      this.lastFirstOverflowIndex = result.firstOverflowIndex;

      const storeInMemory = this.getStoreOverflowInMemory();
      if (storeInMemory || payloadChanged) {
        this.overflowItems = result.overflowItems;
      }

      if (payloadChanged) {
        this.dispatchEvent(new CustomEvent("overflowchange", {
          detail: {
            items: result.overflowItems,
            overflowCount: result.overflowCount,
            firstOverflowIndex: result.firstOverflowIndex
          },
          bubbles: true,
          composed: true
        }));
      }
    }

    getOverflowItems(): OverflowItem[] {
      if (this.overflowItems.length > 0 || this.lastOverflowCount === 0) {
        return [...this.overflowItems];
      }

      const state = getCurrentOverflowState(this, this.getQuerySelector(), this.getMaxRenderedItems());
      this.lastOverflowCount = state.overflowCount;
      this.lastFirstOverflowIndex = state.firstOverflowIndex;
      this.overflowItems = state.overflowItems;
      return [...state.overflowItems];
    }

    getOverflowCount(): number {
      if (this.overflowItems.length > 0 || this.lastOverflowCount > 0) {
        return this.lastOverflowCount;
      }

      const state = getCurrentOverflowState(this, this.getQuerySelector(), this.getMaxRenderedItems());
      this.lastOverflowCount = state.overflowCount;
      this.lastFirstOverflowIndex = state.firstOverflowIndex;
      this.overflowItems = state.overflowItems;
      return state.overflowCount;
    }

    getFirstOverflowIndex(): number {
      if (this.overflowItems.length > 0 || this.lastOverflowCount > 0) {
        return this.lastFirstOverflowIndex;
      }

      const state = getCurrentOverflowState(this, this.getQuerySelector(), this.getMaxRenderedItems());
      this.lastOverflowCount = state.overflowCount;
      this.lastFirstOverflowIndex = state.firstOverflowIndex;
      this.overflowItems = state.overflowItems;
      return state.firstOverflowIndex;
    }

    private setupObservers() {
      this.cleanupObservers();

      if (typeof ResizeObserver !== "undefined") {
        this.resizeObserver = new ResizeObserver(() => {
          clearTimeout(this.resizeTimeout);
          this.resizeTimeout = window.setTimeout(() => {
            const isHorizontal = this.getIsHorizontal();
            const currentSize = isHorizontal ? this.offsetWidth : this.offsetHeight;
            if (currentSize === this.lastContainerSize) {
              return;
            }

            this.lastContainerSize = currentSize;
            this.cachedContainerGap = null;
            this.refresh();
          }, 16);
        });
        this.resizeObserver.observe(this);
      }

      this.mutationObserver = new MutationObserver((mutations) => {
        let shouldRefresh = false;

        for (const mutation of mutations) {
          if (mutation.type === "childList") {
            shouldRefresh = true;
            this.lastContainerSize = 0;
            continue;
          }

          if (mutation.type === "attributes") {
            const target = mutation.target as OverflowElement;
            if (mutation.attributeName === "class" || mutation.attributeName === "style" || mutation.attributeName === "fixed" || mutation.attributeName === "hidden") {
              target.overflowSize = null;
            }
            shouldRefresh = true;
          }
        }

        if (!shouldRefresh) {
          return;
        }

        clearTimeout(this.mutationTimeout);
        this.mutationTimeout = window.setTimeout(() => this.refresh(), 16);
      });
      this.mutationObserver.observe(this, {
        childList: true,
        subtree: false,
        attributes: true,
        attributeFilter: ["id", "fixed", "class", "style", "hidden"]
      });
    }

    private cleanupObservers() {
      this.resizeObserver?.disconnect();
      this.resizeObserver = undefined;
      this.mutationObserver?.disconnect();
      this.mutationObserver = undefined;
      clearTimeout(this.resizeTimeout);
      this.resizeTimeout = undefined;
      clearTimeout(this.mutationTimeout);
      this.mutationTimeout = undefined;
    }

    private getIsHorizontal(): boolean {
      return this.getAttribute("orientation") !== "vertical";
    }

    private getThreshold(): number {
      const value = Number.parseFloat(this.getAttribute("threshold") ?? "25");
      return Number.isFinite(value) ? value : 25;
    }

    private getStoreOverflowInMemory(): boolean {
      const value = this.getAttribute("store-overflow-in-memory");
      return value === "true" || value === "";
    }

    private getMaxRenderedItems(): number {
      const value = Number.parseInt(this.getAttribute("max-rendered-items") ?? "25", 10);
      if (!Number.isFinite(value)) {
        return 25;
      }

      return value;
    }

    private getQuerySelector(): string | null {
      return this.getAttribute("selector") ?? this.getAttribute("selectors");
    }

    private getContainerGap(): number {
      if (this.cachedContainerGap === null) {
        const gap = Number.parseFloat(window.getComputedStyle(this).gap);
        this.cachedContainerGap = Number.isFinite(gap) ? gap : 0;
      }

      return this.cachedContainerGap;
    }
  }

  function refreshContainer(
    container: HTMLElement,
    isHorizontal: boolean,
    querySelector: string | null,
    threshold: number,
    lastHandledState: boolean | null,
    containerGap: number,
    maxRenderedItems: number
  ): RefreshResult {
    const localQuerySelector = buildQuerySelector(querySelector);
    const allItems = Array.from(container.querySelectorAll<OverflowElement>(localQuerySelector));
    const directChildren = Array.from(container.children) as OverflowElement[];
    const managedItemSet = new Set(allItems);
    const managedItems: OverflowElement[] = [];
    const fixedItems: OverflowElement[] = [];
    const ellipsisItems: OverflowElement[] = [];
    const unmanagedItems: OverflowElement[] = [];

    for (const element of directChildren) {
      if (!managedItemSet.has(element)) {
        unmanagedItems.push(element);
      }
    }

    for (const element of allItems) {
      const fixedMode = element.getAttribute("fixed");
      if (fixedMode === "ellipsis") {
        ellipsisItems.push(element);
        continue;
      }

      if (fixedMode !== null) {
        fixedItems.push(element);
        continue;
      }

      managedItems.push(element);
    }

    const orientationChanged = lastHandledState !== null && lastHandledState !== isHorizontal;
    if (orientationChanged) {
      for (const element of allItems) {
        element.removeAttribute("overflow");
        element.overflowSize = null;
      }
    }

    let itemsTotalSize = threshold > 0 ? 10 : 0;
    let containerMaxSize = isHorizontal ? container.offsetWidth : container.offsetHeight;
    containerMaxSize -= threshold;

    let unmanagedTotal = 0;
    for (let index = 0; index < unmanagedItems.length; index++) {
      const size = ensureMeasuredSize(unmanagedItems[index], isHorizontal);
      unmanagedTotal += size + containerGap;
      itemsTotalSize += size + containerGap;
    }

    let ellipsisTotal = 0;
    for (let index = 0; index < ellipsisItems.length; index++) {
      const element = ellipsisItems[index];
      ellipsisTotal += ensureMeasuredSize(element, isHorizontal);
      if (index > 0) {
        ellipsisTotal += containerGap;
      }
    }

    let fixedTotal = 0;
    for (let index = 0; index < fixedItems.length; index++) {
      const size = ensureMeasuredSize(fixedItems[index], isHorizontal);
      fixedTotal += size + (index > 0 ? containerGap : 0);
      itemsTotalSize += size + containerGap;
    }

    const availableSize = containerMaxSize - fixedTotal - unmanagedTotal;
    const desiredFlexShrink = ellipsisTotal > availableSize ? "1" : "0";

    // When ellipsis items keep their natural width, reserve that space before classifying managed items.
    if (desiredFlexShrink !== "1") {
      for (let index = 0; index < ellipsisItems.length; index++) {
        const size = ensureMeasuredSize(ellipsisItems[index], isHorizontal);
        itemsTotalSize += size + containerGap;
      }
    }

    const desiredOverflowStates: boolean[] = [];
    let overflowCount = 0;
    let firstOverflowIndex = -1;
    const forceManagedOverflow = itemsTotalSize > containerMaxSize;

    for (let index = 0; index < managedItems.length; index++) {
      const element = managedItems[index];
      const size = ensureMeasuredSize(element, isHorizontal);
      itemsTotalSize += size + containerGap;

      // When fixed ellipsis items already need shrinking to fit, keep managed items in overflow.
      let shouldOverflow = forceManagedOverflow || desiredFlexShrink === "1";
      if (!shouldOverflow && containerMaxSize > 0) {
        shouldOverflow = itemsTotalSize > containerMaxSize;
      }

      desiredOverflowStates.push(shouldOverflow);
      if (shouldOverflow) {
        overflowCount++;
        if (firstOverflowIndex < 0) {
          firstOverflowIndex = index;
        }
      }

    }

    let overflowChanged = false;

    for (const element of ellipsisItems) {
      if (element.style.flexShrink !== desiredFlexShrink) {
        element.style.flexShrink = desiredFlexShrink;
      }
    }

    for (let index = 0; index < managedItems.length; index++) {
      const element = managedItems[index];
      const shouldOverflow = desiredOverflowStates[index];
      const isOverflow = element.hasAttribute("overflow");

      if (shouldOverflow && !isOverflow) {
        element.setAttribute("overflow", "");
        overflowChanged = true;
      } else if (!shouldOverflow && isOverflow) {
        element.removeAttribute("overflow");
        overflowChanged = true;
      }
    }

    return {
      overflowItems: buildOverflowItems(managedItems, desiredOverflowStates, maxRenderedItems),
      overflowCount,
      firstOverflowIndex,
      overflowChanged,
      isHorizontal
    };
  }

  function getCurrentOverflowState(container: HTMLElement, querySelector: string | null, maxRenderedItems: number): OverflowState {
    const localQuerySelector = buildQuerySelector(querySelector);
    const managedItems = Array.from(container.querySelectorAll<OverflowElement>(localQuerySelector))
      .filter(element => !element.hasAttribute("fixed"));

    const overflowStates = managedItems.map(element => element.hasAttribute("overflow"));
    let overflowCount = 0;
    let firstOverflowIndex = -1;

    for (let index = 0; index < overflowStates.length; index++) {
      if (!overflowStates[index]) {
        continue;
      }

      overflowCount++;
      if (firstOverflowIndex < 0) {
        firstOverflowIndex = index;
      }
    }

    return {
      overflowItems: buildOverflowItems(managedItems, overflowStates, maxRenderedItems),
      overflowCount,
      firstOverflowIndex
    };
  }

  function buildOverflowItems(items: OverflowElement[], overflowStates: boolean[], maxRenderedItems: number): OverflowItem[] {
    const overflowItems: OverflowItem[] = [];
    const unlimited = maxRenderedItems <= 0;

    for (let index = 0; index < items.length; index++) {
      if (!overflowStates[index]) {
        continue;
      }

      overflowItems.push(toOverflowItem(items[index], index));
      if (!unlimited && overflowItems.length >= maxRenderedItems) {
        break;
      }
    }

    return overflowItems;
  }

  function toOverflowItem(element: OverflowElement, index: number): OverflowItem {
    return {
      Id: element.id,
      Overflow: true,
      Text: (element.textContent ?? "").trim(),
      Fixed: element.getAttribute("fixed"),
      Index: index
    };
  }

  function areOverflowItemsEqual(left: OverflowItem[], right: OverflowItem[]): boolean {
    if (left.length !== right.length) {
      return false;
    }

    for (let index = 0; index < left.length; index++) {
      const leftItem = left[index];
      const rightItem = right[index];
      if (leftItem.Id !== rightItem.Id || leftItem.Text !== rightItem.Text || leftItem.Index !== rightItem.Index) {
        return false;
      }
    }

    return true;
  }

  function buildQuerySelector(querySelector: string | null): string {
    if (!querySelector) {
      return ":scope > :not(.fluent-overflow-more)";
    }

    return `:scope > ${querySelector}`;
  }

  function ensureMeasuredSize(element: OverflowElement, isHorizontal: boolean): number {
    if (element.overflowSize === null || element.overflowSize === undefined) {
      const isEllipsisFixed = element.getAttribute("fixed") === "ellipsis";
      element.overflowSize = isHorizontal
        ? getElementWidth(element, isEllipsisFixed)
        : getElementHeight(element, isEllipsisFixed);
    }

    return element.overflowSize;
  }

  function getElementWidth(element: HTMLElement, useIntrinsicSize = false): number {
    const style = window.getComputedStyle(element);
    const width = useIntrinsicSize ? element.scrollWidth : element.offsetWidth;
    const margin = Number.parseFloat(style.marginLeft) + Number.parseFloat(style.marginRight);
    return width + margin;
  }

  function getElementHeight(element: HTMLElement, useIntrinsicSize = false): number {
    const style = window.getComputedStyle(element);
    const height = useIntrinsicSize ? element.scrollHeight : element.offsetHeight;
    const margin = Number.parseFloat(style.marginTop) + Number.parseFloat(style.marginBottom);
    return height + margin;
  }

  export const registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    if (typeof customElements !== "undefined" && !customElements.get("fluent-overflow")) {
      customElements.define("fluent-overflow", FluentOverflow);
    }
  };

  export function Refresh(id: string): void {
    const element = document.getElementById(id) as FluentOverflow | null;
    element?.refresh();
  }

  export function GetOverflowItems(id: string): OverflowItem[] {
    const element = document.getElementById(id) as FluentOverflow | null;
    return element?.getOverflowItems() ?? [];
  }

  export function GetOverflowCount(id: string): number {
    const element = document.getElementById(id) as FluentOverflow | null;
    return element?.getOverflowCount() ?? 0;
  }

  export function GetFirstOverflowIndex(id: string): number {
    const element = document.getElementById(id) as FluentOverflow | null;
    return element?.getFirstOverflowIndex() ?? -1;
  }
}
