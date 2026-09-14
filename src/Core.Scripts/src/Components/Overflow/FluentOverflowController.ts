/**
 * Calculates and manages overflow independently of the custom element.
 */

import type {
  OverflowChangeDetail,
  OverflowControllerOptions,
  OverflowDirection,
  OverflowItem,
  OverflowOrientation,
} from './FluentOverflowTypes';

export namespace Microsoft.FluentUI.Blazor.Components.Overflow {
    
  /** Browser-facing overflow state event name. */
  export const overflowChangeEventName = 'overflowchange';

  /** Fluent-prefixed alias used by framework event bridges. */
  export const fluentOverflowChangeEventName = 'fluentoverflowchange';

  /** 
   * Applies overflow behavior to a host element without changing its child structure.
   */
  export class OverflowController {
    private resizeObserver?: ResizeObserver;
    private mutationObserver?: MutationObserver;
    private animationFrame = 0;
    private lastState = '';
    private connected = false;
    private initialLayoutCompleted = false;
    private restoreHostVisibility?: string;
    private managedItems: HTMLElement[] = [];
    private hiddenItems: HTMLElement[] = [];
    private observedReservedElement?: Element;
    private readonly measuredSizes = new WeakMap<Element, Partial<Record<OverflowOrientation, number>>>();
    private readonly observedItems = new Set<HTMLElement>();
    private readonly hiddenByController = new Set<HTMLElement>();
    private readonly expectedHiddenMutationCounts = new WeakMap<HTMLElement, number>();

    /**
     * Creates an overflow controller from host and layout adapters.
     */
    constructor(private readonly options: OverflowControllerOptions) { }

    /**
     * Starts observing the host and schedules its first measurement.
     */
    connect(): void {
      if (this.connected) {
        return;
      }
      this.connected = true;
      this.initialLayoutCompleted = false;
      this.syncInitialVisibility();
      const { host } = this.options;

      // Observe size changes on the host and layout container.
      this.resizeObserver = new ResizeObserver((entries) => {
        for (const entry of entries) {
          if (entry.target !== host && entry.target !== this.layoutContainer) {
            this.measuredSizes.delete(entry.target);
          }
        }
        this.requestRefresh();
      });

      // Observe mutations on the host and its subtree.
      this.mutationObserver = new MutationObserver((records) => {
        let shouldRefresh = false;
        for (const record of records) {
          shouldRefresh = this.shouldRefreshForMutation(record) || shouldRefresh;
        }
        if (shouldRefresh) {
          this.invalidateMeasurements();
          this.requestRefresh();
        }
      });

      this.resizeObserver.observe(host);
      if (this.layoutContainer !== host) {
        this.resizeObserver.observe(this.layoutContainer);
      }

      // Start observing mutations and schedule the first refresh.
      this.syncReservedElementObservation();

      const pinnedItemIdAttribute = this.options.pinnedItemIdAttribute;
      const attributeFilter = [
        'activeid',
        'behavior',
        'class',
        'hidden',
        'id',
        'orientation',
        'overflow-direction',
        'selector',
        'style',
        'threshold',
      ];

      // Ensure the pinned item ID attribute is included in the attribute filter if specified.
      if (pinnedItemIdAttribute && !attributeFilter.includes(pinnedItemIdAttribute)) {
        attributeFilter.push(pinnedItemIdAttribute);
      }

      // Begin observing mutations on the host element.
      this.mutationObserver.observe(host, {
        attributes: true,
        attributeFilter,
        childList: true,
        characterData: true,
        subtree: true,
      });

      // Schedule the first refresh to ensure the overflow state is up-to-date.
      this.requestRefresh();
    }

    /** 
     * Coalesces an overflow recalculation into the next animation frame.
     */
    requestRefresh(): void {
      if (!this.connected || !this.options.host.isConnected) {
        return;
      }
      this.syncInitialVisibility();
      if (this.animationFrame) {
        return;
      }

      this.animationFrame = requestAnimationFrame(() => {
        this.animationFrame = 0;
        if (this.options.host.isConnected) {
          this.refreshNow();
          this.completeInitialLayout();
        }
      });
    }

    /**
     * Recalculates overflow synchronously.
     */
    refresh(): void {
      cancelAnimationFrame(this.animationFrame);
      this.animationFrame = 0;
      if (this.connected && this.options.host.isConnected) {
        this.refreshNow();
        this.completeInitialLayout();
      }
    }

    /**
     * Stops observation and restores visibility owned by this controller.
     */
    disconnect(): void {
      if (!this.connected) {
        return;
      }

      // Cleanup and disconnect observers.
      this.connected = false;
      cancelAnimationFrame(this.animationFrame);
      this.animationFrame = 0;
      this.resizeObserver?.disconnect();
      this.resizeObserver = undefined;
      this.mutationObserver?.disconnect();
      this.mutationObserver = undefined;

      const visibilityChanged = this.hiddenByController.size > 0;
      for (const item of this.hiddenByController) {
        item.hidden = false;
        this.measuredSizes.delete(item);
      }
      this.hiddenByController.clear();
      this.observedItems.clear();
      this.observedReservedElement = undefined;
      this.managedItems = [];
      this.hiddenItems = [];
      this.lastState = '';
      this.options.setOverflowActive?.(false);
      this.restoreInitialVisibility();
      this.initialLayoutCompleted = false;

      // Notify the host that the overflow state has changed.
      if (visibilityChanged) {
        this.options.onVisibilityChanged?.();
      }
    }

    /** 
     * Returns the currently managed items in DOM order.
     */
    getManagedItems(): readonly Element[] {
      return this.managedItems;
    }

    /**
     * Returns the items currently hidden by this controller.
     */
    getHiddenItems(): readonly Element[] {
      return this.hiddenItems;
    }

    /**
     * Converts the current hidden items to their public event representation.
     */
    getHiddenOverflowItems(maximum = Number.POSITIVE_INFINITY): OverflowItem[] {
      const limit = this.normalizeMaximum(maximum);
      const hidden = new Set(this.hiddenItems);
      return this.managedItems
        .flatMap((item, index) => hidden.has(item) ? [this.toOverflowItem(item, index)] : [])
        .slice(0, limit);
    }

    /** Synchronizes host visibility while waiting for the first layout. */
    private syncInitialVisibility(): void {
      if (this.initialLayoutCompleted) {
        return;
      }

      if (this.options.visibleOnLoad === false) {
        if (this.restoreHostVisibility === undefined) {
          this.restoreHostVisibility = this.options.host.style.visibility;
          this.options.host.style.visibility = 'hidden';
        }
        return;
      }

      this.restoreInitialVisibility();
    }

    /** Restores host visibility after the first completed layout. */
    private completeInitialLayout(): void {
      this.initialLayoutCompleted = true;
      this.restoreInitialVisibility();
    }

    /** Restores only the inline visibility value owned by this controller. */
    private restoreInitialVisibility(): void {
      if (this.restoreHostVisibility === undefined) {
        return;
      }

      if (this.options.host.style.visibility === 'hidden') {
        this.options.host.style.visibility = this.restoreHostVisibility;
      }
      this.restoreHostVisibility = undefined;
    }

    /**
     * Measures the current layout and applies its resulting overflow state.
     */
    private refreshNow(): void {
      const items = this.findManagedItems();
      const previouslyHidden = new Set(this.hiddenByController);
      const consumerHidden = new Set(items.filter((item) => item.hidden && !previouslyHidden.has(item)));
      for (const item of previouslyHidden) {
        this.setHidden(item, false);
      }
      this.options.setOverflowActive?.(false);
      this.syncReservedElementObservation();

      const hiddenItems = this.calculateOverflowedItems(items, consumerHidden);
      const visibilityChanged = this.applyVisibility(items, hiddenItems, consumerHidden, previouslyHidden);

      this.managedItems = items;
      this.hiddenItems = items.filter((item) => hiddenItems.has(item));
      this.syncObservedItems(items, new Set([...hiddenItems, ...consumerHidden]));
      this.options.setOverflowActive?.(this.hiddenItems.length > 0 || (this.options.preOverflowCount ?? 0) > 0);
      const detail = this.createChangeDetail();
      this.options.onLayoutChanged?.(detail, this.hiddenItems);
      if (visibilityChanged) {
        this.options.onVisibilityChanged?.();
      }
      this.emitChange(detail);
    }

    /**
     * Calculates which managed items must be hidden to fit the available space.
     */
    private calculateOverflowedItems(items: HTMLElement[], consumerHidden: ReadonlySet<HTMLElement>): Set<HTMLElement> {
      const vertical = this.orientation === 'vertical';
      const availableSize = Math.max(
        0,
        (vertical ? this.layoutContainer.clientHeight : this.layoutContainer.clientWidth) - this.normalizedThreshold,
      );
      const gap = this.getGap(vertical);
      const layoutChildren = this.layoutChildren.filter((item) => !consumerHidden.has(item));
      const sizes = new Map(layoutChildren.map((item) => [item, this.getOuterSize(item, vertical)]));
      const pinnedItemIdAttribute = this.options.pinnedItemIdAttribute ?? null;
      const pinnedItemId = pinnedItemIdAttribute
        ? this.options.host.getAttribute(pinnedItemIdAttribute)
        : null;
      const ellipsisItems = items.filter((item) => item.getAttribute('behavior') === 'ellipsis');

      // Determine which items can be hidden to make room for overflow.
      const overflowableItems = items.filter((item) => {
        const behavior = item.getAttribute('behavior');
        return behavior !== 'fixed'
          && behavior !== 'ellipsis'
          && item.id !== pinnedItemId
          && !consumerHidden.has(item);
      });

      const naturalSize = layoutChildren.reduce((total, item) => total + (sizes.get(item) ?? 0), 0)
        + gap * Math.max(0, layoutChildren.length - 1);

      const shrinkableSize = vertical
        ? 0
        : ellipsisItems.reduce((total, item) => total + (sizes.get(item) ?? 0), 0);

      const hiddenItems = new Set<HTMLElement>();
      
      let visibleSize = naturalSize - shrinkableSize;

      // If the visible size exceeds the available size, determine which items to hide.
      if (visibleSize > availableSize || (this.options.preOverflowCount ?? 0) > 0) {
        this.options.setOverflowActive?.(true);
        const reservedElement = this.options.reservedElement;
        const reservedSize = reservedElement ? this.getOuterSize(reservedElement, vertical, false) + gap : 0;
        const itemBudget = Math.max(0, availableSize - reservedSize);
        const orderedCandidates = this.overflowDirection === 'end'
          ? [...overflowableItems].reverse()
          : overflowableItems;

        for (const item of orderedCandidates) {
          if (visibleSize <= itemBudget) {
            break;
          }
          hiddenItems.add(item);
          visibleSize -= (sizes.get(item) ?? 0) + gap;
        }
      }

      return hiddenItems;
    }

    /**
     * Applies controller-owned visibility and reports whether it changed.
     */
    private applyVisibility(
      items: HTMLElement[],
      hiddenItems: ReadonlySet<HTMLElement>,
      consumerHidden: ReadonlySet<HTMLElement>,
      previouslyHidden: ReadonlySet<HTMLElement>,
    ): boolean {
      let visibilityChanged = false;
      for (const item of items) {
        if (consumerHidden.has(item)) {
          continue;
        }
        const shouldHide = hiddenItems.has(item);
        if (shouldHide !== previouslyHidden.has(item)) {
          visibilityChanged = true;
        }
        this.setHidden(item, shouldHide);
      }
      return visibilityChanged;
    }

    /**
     * Returns the element that constrains the available layout space.
     */
    private get layoutContainer(): HTMLElement {
      return this.options.layoutContainer ?? this.options.host;
    }

    /**
     * Returns all direct children that participate in layout measurement.
     */
    private get layoutChildren(): HTMLElement[] {
      return (this.options.layoutChildren
        ?? Array.from(this.options.host.children)) as HTMLElement[];
    }

    /**
     * Returns the threshold normalized to a non-negative finite value.
     */
    private get normalizedThreshold(): number {
      const threshold = this.options.threshold;
      return Number.isFinite(threshold) ? Math.max(0, threshold) : 0;
    }

    /**
     * Returns the logical side from which items should overflow.
     */
    private get overflowDirection(): OverflowDirection {
      return this.options.overflowDirection === 'start' ? 'start' : 'end';
    }

    /** Returns the axis used for layout and size calculations. */
    private get orientation(): OverflowOrientation {
      return this.options.orientation
        ?? (this.options.host.getAttribute('orientation') === 'vertical' ? 'vertical' : 'horizontal');
    }

    /**
     * Finds layout children that match the configured selector.
     */
    private findManagedItems(): HTMLElement[] {
      const children = this.layoutChildren;
      const querySelector = this.options.querySelector;
      if (!querySelector) {
        return children;
      }

      try {
        return children.filter((item) => item.matches(querySelector));
      } catch {
        return [];
      }
    }

    /** 
     * Updates an item's controller-owned hidden state.
     */
    private setHidden(item: HTMLElement, hidden: boolean): void {
      if (item.hidden !== hidden) {
        const expectedMutationCount = this.expectedHiddenMutationCounts.get(item) ?? 0;
        this.expectedHiddenMutationCounts.set(item, expectedMutationCount + 1);
        item.hidden = hidden;
      }

      if (hidden) {
        this.hiddenByController.add(item);
      } else {
        this.hiddenByController.delete(item);
      }
    }

    /**
     * Synchronizes resize observation with the currently visible managed items.
     */
    private syncObservedItems(items: HTMLElement[], hidden: Set<HTMLElement>): void {
      if (!this.resizeObserver) {
        return;
      }

      const nextItems = new Set(items);
      for (const item of this.hiddenByController) {
        if (!nextItems.has(item)) {
          item.hidden = false;
          this.measuredSizes.delete(item);
          this.hiddenByController.delete(item);
        }
      }
      for (const item of this.observedItems) {
        if (!nextItems.has(item) || hidden.has(item)) {
          this.resizeObserver.unobserve(item);
          this.observedItems.delete(item);
        }
        if (!nextItems.has(item) && this.hiddenByController.delete(item)) {
          item.hidden = false;
        }
      }
      for (const item of items) {
        if (!hidden.has(item) && !this.observedItems.has(item)) {
          this.resizeObserver.observe(item);
          this.observedItems.add(item);
        }
      }
    }

    /**
     * Observes the current reserved element for size changes.
     */
    private syncReservedElementObservation(): void {
      const reservedElement = this.options.reservedElement;
      if (!this.resizeObserver || reservedElement === this.observedReservedElement) {
        return;
      }

      if (this.observedReservedElement) {
        this.resizeObserver.unobserve(this.observedReservedElement);
      }
      if (reservedElement) {
        this.resizeObserver.observe(reservedElement);
      }
      this.observedReservedElement = reservedElement;
    }

    /**
     * Ignores expected hidden mutations and refreshes for external changes.
     */
    private shouldRefreshForMutation(record: MutationRecord): boolean {
      if (record.type !== 'attributes' || record.attributeName !== 'hidden') {
        return true;
      }

      const item = record.target as HTMLElement;
      const expectedMutationCount = this.expectedHiddenMutationCounts.get(item) ?? 0;
      if (expectedMutationCount > 0) {
        if (expectedMutationCount === 1) {
          this.expectedHiddenMutationCounts.delete(item);
        } else {
          this.expectedHiddenMutationCounts.set(item, expectedMutationCount - 1);
        }
        return false;
      }
      return true;
    }

    /**
     * Clears cached measurements for all current layout children.
     */
    private invalidateMeasurements(): void {
      for (const item of this.layoutChildren) {
        this.measuredSizes.delete(item);
      }
    }

    /**
     * Reads the row or column gap used by the active orientation.
     */
    private getGap(vertical: boolean): number {
      const styles = getComputedStyle(this.options.gapContainer ?? this.options.host);
      const gap = Number.parseFloat(vertical ? styles.rowGap : styles.columnGap);
      return Number.isFinite(gap) ? gap : 0;
    }

    /**
     * Measures an item's rendered size, intrinsic size, and margins.
     */
    private getOuterSize(item: Element, vertical: boolean, cache = true): number {
      const orientation = vertical ? 'vertical' : 'horizontal';
      const cachedSize = this.measuredSizes.get(item)?.[orientation];
      if (cache && cachedSize !== undefined) {
        return cachedSize;
      }

      const rect = item.getBoundingClientRect();
      const styles = getComputedStyle(item);
      const renderedSize = vertical ? rect.height : rect.width;
      const intrinsicSize = vertical ? item.scrollHeight : item.scrollWidth;
      const margins = vertical
        ? Number.parseFloat(styles.marginBlockStart) + Number.parseFloat(styles.marginBlockEnd)
        : Number.parseFloat(styles.marginInlineStart) + Number.parseFloat(styles.marginInlineEnd);
      const size = Math.max(renderedSize, intrinsicSize)
        + (Number.isFinite(margins) ? margins : 0);
      if (cache) {
        const measuredSizes = this.measuredSizes.get(item) ?? {};
        measuredSizes[orientation] = size;
        this.measuredSizes.set(item, measuredSizes);
      }
      return size;
    }

    /**
     * Builds the public event detail from the current overflow state.
     */
    private createChangeDetail(): OverflowChangeDetail {
      const overflowItems = this.getHiddenOverflowItems(this.options.maxRenderedItems);
      return {
        items: overflowItems,
        overflowCount: this.hiddenItems.length + (this.options.preOverflowCount ?? 0),
      };
    }

    /**
     * Converts a managed element to its public overflow representation.
     */
    private toOverflowItem(item: Element, index: number): OverflowItem {
      return {
        id: item.id,
        text: (item.textContent ?? '').trim(),
        index,
      };
    }

    /**
     * Dispatches change events when the serialized overflow state changes.
     */
    private emitChange(detail: OverflowChangeDetail): void {
      const state = JSON.stringify(detail);
      if (state === this.lastState) {
        return;
      }

      this.lastState = state;
      const eventInit: CustomEventInit<OverflowChangeDetail> = {
        bubbles: true,
        composed: true,
        detail,
      };
      this.options.host.dispatchEvent(new CustomEvent<OverflowChangeDetail>(overflowChangeEventName, eventInit));
      this.options.host.dispatchEvent(new CustomEvent<OverflowChangeDetail>(fluentOverflowChangeEventName, eventInit));
    }

    /**
     * Normalizes a payload limit to a positive integer or infinity.
     */
    private normalizeMaximum(value: number): number {
      if (!Number.isFinite(value) || value <= 0) {
        return Number.POSITIVE_INFINITY;
      }
      return Math.floor(value);
    }
  }
}