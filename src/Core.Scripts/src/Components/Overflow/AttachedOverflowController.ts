/** Represents an item returned in an overflow state payload. */
export interface OverflowItem {
  Id: string;
  Overflow: boolean;
  Text: string;
  Behavior?: string | null;
  Index?: number;
}

/** Represents the current overflow state of an attached host. */
export interface OverflowState {
  overflowItems: OverflowItem[];
  overflowCount: number;
  firstOverflowIndex: number;
  orderedItemIds: string[];
}

/** Extended overflow state with change detection and orientation metadata. */
export interface RefreshResult extends OverflowState {
  overflowChanged: boolean;
  isHorizontal: boolean;
  containerSize: number;
}

/** Extended HTMLElement interface to track cached overflow size measurements. */
export interface OverflowElement extends HTMLElement {
  overflowSize?: number | null;
}

/** Extended host interface for components that recalculate their managed children. */
export interface OverflowHostElement extends HTMLElement {
  tabsChanged?: () => void;
}

/** Calculates overflow state for an attached host. */
export type RefreshContainer = (
  container: HTMLElement,
  isHorizontal: boolean,
  querySelector: string | null,
  threshold: number,
  lastHandledState: boolean | null,
  containerGap: number,
  maxRenderedItems: number,
  pinnedItemId?: string | null
) => RefreshResult;

/** Applies the overflow algorithm to an existing element without adding a wrapper. */
export class AttachedOverflowController {
  private resizeObserver?: ResizeObserver;
  private mutationObserver?: MutationObserver;
  private resizeTimeout?: number;
  private mutationTimeout?: number;
  private refreshAnimationFrame?: number;
  private lastHandledState: boolean | null = null;
  private lastContainerSize = 0;
  private cachedContainerGap: number | null = null;
  private state: OverflowState = emptyOverflowState();
  private hiddenByController = new Set<OverflowElement>();
  private expectedHiddenMutations = new WeakMap<OverflowElement, boolean>();

  constructor(
    private readonly refreshContainer: RefreshContainer,
    private readonly host: OverflowHostElement,
    private querySelector: string,
    private threshold: number,
    private maxRenderedItems: number,
    private pinnedItemIdAttribute: string | null,
    private synchronizeHidden: boolean,
    private notifyHostItemsChanged: boolean
  ) {
  }

  connect() {
    this.setupObservers();
    this.scheduleRefresh();
  }

  update(
    querySelector: string,
    threshold: number,
    maxRenderedItems: number,
    pinnedItemIdAttribute: string | null,
    synchronizeHidden: boolean,
    notifyHostItemsChanged: boolean
  ) {
    this.querySelector = querySelector;
    this.threshold = threshold;
    this.maxRenderedItems = maxRenderedItems;
    this.pinnedItemIdAttribute = pinnedItemIdAttribute;
    this.synchronizeHidden = synchronizeHidden;
    this.notifyHostItemsChanged = notifyHostItemsChanged;
    this.lastContainerSize = 0;
    this.cachedContainerGap = null;
    this.scheduleRefresh();
  }

  disconnect() {
    this.cleanupObservers();

    for (const item of this.hiddenByController) {
      item.removeAttribute("overflow");
      item.overflowSize = null;
      item.hidden = false;
    }

    const managedItems = this.getManagedItems();
    for (const item of managedItems) {
      item.removeAttribute("overflow");
      item.overflowSize = null;
    }

    this.hiddenByController.clear();
    if (this.notifyHostItemsChanged) {
      this.host.tabsChanged?.();
    }
  }

  refresh() {
    this.flushScheduledRefresh();
  }

  getOverflowState(): OverflowState {
    if (this.refreshAnimationFrame !== undefined || this.resizeTimeout !== undefined || this.mutationTimeout !== undefined) {
      this.flushScheduledRefresh();
    }

    return {
      overflowItems: [...this.state.overflowItems],
      overflowCount: this.state.overflowCount,
      firstOverflowIndex: this.state.firstOverflowIndex,
      orderedItemIds: [...this.state.orderedItemIds]
    };
  }

  private scheduleRefresh() {
    if (!this.host.isConnected || this.refreshAnimationFrame !== undefined) {
      return;
    }

    this.refreshAnimationFrame = requestAnimationFrame(() => {
      this.refreshAnimationFrame = undefined;
      if (this.host.isConnected) {
        this.refreshNow();
      }
    });
  }

  private flushScheduledRefresh() {
    clearTimeout(this.resizeTimeout);
    this.resizeTimeout = undefined;
    clearTimeout(this.mutationTimeout);
    this.mutationTimeout = undefined;

    if (this.refreshAnimationFrame !== undefined) {
      cancelAnimationFrame(this.refreshAnimationFrame);
      this.refreshAnimationFrame = undefined;
    }

    if (this.host.isConnected) {
      this.refreshNow();
    }
  }

  private refreshNow() {
    const isHorizontal = this.host.getAttribute("orientation") !== "vertical";
    const result = this.refreshContainer(
      this.host,
      isHorizontal,
      this.querySelector,
      this.threshold,
      this.lastHandledState,
      this.getContainerGap(),
      this.maxRenderedItems,
      this.getPinnedItemId()
    );
    this.lastHandledState = result.isHorizontal;
    this.lastContainerSize = result.containerSize;

    const visibilityChanged = this.synchronizeManagedVisibility();
    const payloadChanged = result.overflowChanged
      || this.state.overflowCount !== result.overflowCount
      || this.state.firstOverflowIndex !== result.firstOverflowIndex
      || !areStringArraysEqual(this.state.orderedItemIds, result.orderedItemIds)
      || !areOverflowItemsEqual(this.state.overflowItems, result.overflowItems);

    this.state = result;

    if (visibilityChanged && this.notifyHostItemsChanged) {
      this.host.tabsChanged?.();
    }

    if (payloadChanged) {
      const eventInit = {
        detail: {
          items: result.overflowItems,
          overflowCount: result.overflowCount,
          firstOverflowIndex: result.firstOverflowIndex,
          orderedItemIds: result.orderedItemIds
        },
        bubbles: true,
        composed: true
      };

      this.host.dispatchEvent(new CustomEvent("overflowchange", eventInit));
      this.host.dispatchEvent(new CustomEvent("fluentoverflowchange", eventInit));
    }
  }

  private setupObservers() {
    if (typeof ResizeObserver !== "undefined") {
      this.resizeObserver = new ResizeObserver(() => {
        if (this.refreshAnimationFrame !== undefined) {
          return;
        }

        clearTimeout(this.resizeTimeout);
        this.resizeTimeout = window.setTimeout(() => {
          this.resizeTimeout = undefined;
          const isHorizontal = this.host.getAttribute("orientation") !== "vertical";
          const currentSize = isHorizontal ? this.host.offsetWidth : this.host.offsetHeight;
          this.cachedContainerGap = null;
          if (currentSize === this.lastContainerSize) {
            return;
          }

          this.lastContainerSize = currentSize;
          this.scheduleRefresh();
        }, 16);
      });
      this.resizeObserver.observe(this.host);
    }

    this.mutationObserver = new MutationObserver((mutations) => {
      let shouldRefresh = false;

      for (const mutation of mutations) {
        if (mutation.type === "childList") {
          if (mutation.target === this.host) {
            this.lastContainerSize = 0;
            shouldRefresh = true;
          }
          continue;
        }

        const target = mutation.target as OverflowElement;
        if (target === this.host) {
          if (mutation.attributeName === "orientation") {
            this.lastContainerSize = 0;
            this.cachedContainerGap = null;
          }
          shouldRefresh = true;
          continue;
        }

        if (target.parentElement !== this.host) {
          continue;
        }

        if (mutation.attributeName === "hidden") {
          const expectedHidden = this.expectedHiddenMutations.get(target);
          if (expectedHidden === target.hidden) {
            this.expectedHiddenMutations.delete(target);
            continue;
          }
        }

        if (mutation.attributeName === "class"
          || mutation.attributeName === "style"
          || mutation.attributeName === "behavior"
          || mutation.attributeName === "hidden") {
          target.overflowSize = null;
        }
        shouldRefresh = true;
      }

      if (!shouldRefresh) {
        return;
      }

      clearTimeout(this.mutationTimeout);
      this.mutationTimeout = undefined;
      if (this.refreshAnimationFrame !== undefined) {
        return;
      }

      this.mutationTimeout = window.setTimeout(() => {
        this.mutationTimeout = undefined;
        this.scheduleRefresh();
      }, 16);
    });
    this.mutationObserver.observe(this.host, {
      childList: true,
      subtree: true,
      attributes: true,
      attributeFilter: ["id", "behavior", "class", "style", "hidden", "activeid", "orientation"]
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
    if (this.refreshAnimationFrame !== undefined) {
      cancelAnimationFrame(this.refreshAnimationFrame);
      this.refreshAnimationFrame = undefined;
    }
  }

  private synchronizeManagedVisibility(): boolean {
    if (!this.synchronizeHidden) {
      return false;
    }

    let changed = false;
    const managedItems = this.getManagedItems();
    const managedItemSet = new Set(managedItems);

    for (const item of managedItems) {
      if (item.hasAttribute("overflow")) {
        if (!item.hidden) {
          this.expectedHiddenMutations.set(item, true);
          item.hidden = true;
          this.hiddenByController.add(item);
          changed = true;
        }
      } else if (this.hiddenByController.delete(item)) {
        this.expectedHiddenMutations.set(item, false);
        item.hidden = false;
        changed = true;
      }
    }

    for (const item of this.hiddenByController) {
      if (!managedItemSet.has(item)) {
        this.hiddenByController.delete(item);
      }
    }

    return changed;
  }

  private getManagedItems(): OverflowElement[] {
    return Array.from(this.host.querySelectorAll<OverflowElement>(buildQuerySelector(this.querySelector)));
  }

  private getPinnedItemId(): string | null {
    return this.pinnedItemIdAttribute
      ? this.host.getAttribute(this.pinnedItemIdAttribute)
      : null;
  }

  private getContainerGap(): number {
    if (this.cachedContainerGap === null) {
      const gap = Number.parseFloat(window.getComputedStyle(this.host).gap);
      this.cachedContainerGap = Number.isFinite(gap) ? gap : 0;
    }

    return this.cachedContainerGap;
  }
}

function emptyOverflowState(): OverflowState {
  return {
    overflowItems: [],
    overflowCount: 0,
    firstOverflowIndex: -1,
    orderedItemIds: []
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

function areStringArraysEqual(left: string[], right: string[]): boolean {
  if (left.length !== right.length) {
    return false;
  }

  for (let index = 0; index < left.length; index++) {
    if (left[index] !== right[index]) {
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