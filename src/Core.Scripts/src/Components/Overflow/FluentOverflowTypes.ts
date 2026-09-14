/**
 * Defines the public types and custom element tag name.
 */

/** Tag name for the overflow element. */
export const tagName = 'fluent-overflow' as const;

/** Logical side from which items are hidden when space runs out. */
export type OverflowDirection = 'start' | 'end';

/** Axis used to arrange and measure managed items. */
export type OverflowOrientation = 'horizontal' | 'vertical';

/** Public representation of an item managed by an overflow controller. */
export interface OverflowItem {
  id: string;                               // Element identifier
  text: string;                             // Trimmed text content
  index: number;                            // Position in the ordered item collection
}

/** State carried by the `overflowchange` and `fluentoverflowchange` events. */
export interface OverflowChangeDetail {
  items: OverflowItem[];                    // Overflowed items, limited by `maxRenderedItems`
  overflowCount: number;                    // Total number of overflowed items before payload limiting.
}

/** Detail emitted when the slotted overflow presentation opens or closes. */
export interface OverflowToggleDetail {
  totalItemsCount: number;                  // Total number of items managed by the overflow controller.
  totalHiddenCount: number;                 // Total number of items currently hidden.
  opened: boolean;                          // Whether the overflow presentation is currently open.
  items: OverflowItem[];                    // Items currently managed by the overflow presentation.
}

/** Configuration adapters used to apply overflow behavior to any host element. */
export interface OverflowControllerOptions {
  readonly preOverflowCount?: number;                                                                   // Items omitted from the DOM but counted as overflow; reserves trigger space even when all rendered items fit. Defaults to zero.
  readonly host: HTMLElement;                                                                           // Element that owns the managed direct children and dispatches overflow events.
  readonly querySelector: string;                                                                       // Selector applied to candidate direct children.
  readonly threshold: number;                                                                           // Number of pixels reserved before overflow begins.
  readonly maxRenderedItems: number;                                                                    // Maximum number of overflow items included in event payloads.
  readonly visibleOnLoad?: boolean;                                                                     // Whether the host is visible before the first completed layout. Defaults to true.
  readonly pinnedItemIdAttribute?: string | null;                                                       // Host attribute whose value identifies an item that must remain visible.
  readonly orientation?: OverflowOrientation;                                                           // Current layout orientation. Defaults to the host `orientation` attribute.
  readonly overflowDirection?: OverflowDirection;                                                       // Logical side from which candidates overflow. Defaults to `end`.
  readonly layoutContainer?: HTMLElement;                                                               // Element whose available dimensions constrain the layout.
  readonly gapContainer?: HTMLElement;                                                                  // Element whose computed row or column gap applies to the layout.
  readonly layoutChildren?: HTMLElement[];                                                              // Direct children participating in total-size calculations.
  readonly reservedElement?: Element;                                                                   // Optional trigger or other element whose size is reserved during overflow.
  readonly setOverflowActive?: (active: boolean) => void;                                               // Updates presentation that must be active only while at least one item overflows.
  readonly onLayoutChanged?: (detail: OverflowChangeDetail, hiddenItems: readonly Element[]) => void;   // Receives every completed layout, including layouts whose event state is unchanged.
  readonly onVisibilityChanged?: () => void;                                                            // Receives a notification when the controller changes managed-item visibility.
}
