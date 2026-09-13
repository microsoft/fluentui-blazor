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

  /** Creates a controller that applies overflow behavior directly to an existing host. */
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

  /** Starts observing the host and schedules its initial overflow measurement. */
  connect() {
    this.setupObservers();
    this.scheduleRefresh();
  }

  /** Updates runtime options and invalidates measurements affected by those options. */
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

  /** Stops observation and restores items whose visibility was managed by this controller. */
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

  /** Immediately recalculates overflow after canceling any scheduled refresh work. */
  refresh() {
    this.flushScheduledRefresh();
  }

  /** Returns a defensive snapshot of the latest overflow state. */
  getOverflowState(): OverflowState {
    // Imperative reads must include any DOM changes already queued for measurement.
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

  /** Coalesces automatic refresh requests into one measurement per animation frame. */
  private scheduleRefresh() {
    // Observer callbacks often arrive in the same DOM update; one pending frame is enough
    // to measure the final layout produced by the entire burst.
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

  /** Cancels deferred refresh paths and synchronously measures the connected host. */
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

  /** Measures overflow, synchronizes item visibility, and publishes meaningful changes. */
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
    // Keep the size read performed by the algorithm so ResizeObserver's follow-up
    // notification does not trigger an identical second measurement.
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

      // Keep the browser-facing event while providing a distinct name for Blazor's
      // custom-event bridge, which cannot share a native event name in .NET 11.
      this.host.dispatchEvent(new CustomEvent("overflowchange", eventInit));
      this.host.dispatchEvent(new CustomEvent("fluentoverflowchange", eventInit));
    }
  }

  /** Observes size and relevant DOM changes that can invalidate overflow calculations. */
  private setupObservers() {
    if (typeof ResizeObserver !== "undefined") {
      this.resizeObserver = new ResizeObserver(() => {
        // A pending frame will consume the latest dimensions regardless of its trigger.
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
            // Ignore the observer echo produced by synchronizeManagedVisibility.
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

  /** Disconnects observers and cancels all pending refresh work. */
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

  /** Mirrors overflow attributes to hidden state and reports whether visibility changed. */
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

    // Detached or no-longer-managed items must not remain owned by this controller.
    for (const item of this.hiddenByController) {
      if (!managedItemSet.has(item)) {
        this.hiddenByController.delete(item);
      }
    }

    return changed;
  }

  /** Finds direct children selected for overflow management. */
  private getManagedItems(): OverflowElement[] {
    return Array.from(this.host.querySelectorAll<OverflowElement>(buildQuerySelector(this.querySelector)));
  }

  /** Resolves the pinned item identifier from the configured host attribute. */
  private getPinnedItemId(): string | null {
    return this.pinnedItemIdAttribute
      ? this.host.getAttribute(this.pinnedItemIdAttribute)
      : null;
  }

  /** Reads and caches the host gap used by the overflow measurement algorithm. */
  private getContainerGap(): number {
    if (this.cachedContainerGap === null) {
      const gap = Number.parseFloat(window.getComputedStyle(this.host).gap);
      this.cachedContainerGap = Number.isFinite(gap) ? gap : 0;
    }

    return this.cachedContainerGap;
  }
}

/** Creates the initial state used before the first host measurement. */
function emptyOverflowState(): OverflowState {
  return {
    overflowItems: [],
    overflowCount: 0,
    firstOverflowIndex: -1,
    orderedItemIds: []
  };
}

/** Compares the item fields included in overflow change payloads. */
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

/** Compares two ordered identifier collections without allocating intermediate arrays. */
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

/** Restricts the configured selector to direct children of the attached host. */
function buildQuerySelector(querySelector: string | null): string {
  if (!querySelector) {
    return ":scope > :not(.fluent-overflow-more)";
  }

  return `:scope > ${querySelector}`;
}