/**
 * Exposes the imperative API for attaching overflow behavior to existing elements.
 */

import { Microsoft as OverflowControllerFile } from './overflow-controller.js';
import { Microsoft as OverflowFile } from './overflow.js';

export namespace Microsoft.FluentUI.Blazor.Components.Overflow {

  import OverflowController = OverflowControllerFile.FluentUI.Blazor.Components.Overflow.OverflowController;
  import Overflow = OverflowFile.FluentUI.Blazor.Components.Overflow.Overflow;

  const attachedControllers = new Map<string, OverflowController>();

  /**
   * Attaches overflow behavior to an existing element without wrapping its children.
   * Replaces any controller previously registered for the same identifier.
   *
   * @param id - Identifier of the host element. Missing elements are ignored.
   * @param querySelector - Selector applied only to direct children of the host.
   * @param threshold - Additional space, in pixels, reserved before items overflow.
   * @param maxRenderedItems - Maximum payload size. Non-positive values are unlimited.
   * @param pinnedItemIdAttribute - Name of the host attribute containing the item ID to keep visible.
   */
  export function initializeOverflow(
    id: string,
    querySelector: string,
    threshold: number,
    maxRenderedItems: number,
    pinnedItemIdAttribute: string | null,
  ): void {

    // Clean any previously attached controller for this element.
    attachedControllers.get(id)?.disconnect();
    attachedControllers.delete(id);

    // Element corresponding to the provided ID.
    const element = document.getElementById(id);
    if (!element) {
      return;
    }

    // Configure it directly.
    if (element instanceof Overflow) {
      element.selector = querySelector;
      element.threshold = threshold;
      element.maxOverflowItems = maxRenderedItems <= 0
        ? Number.POSITIVE_INFINITY
        : maxRenderedItems;
      element.refresh();
      return;
    }

    // Otherwise, attach a new controller.
    const controller = new OverflowController({
      host: element,
      querySelector,
      threshold,
      maxRenderedItems,
      pinnedItemIdAttribute,
    });
    attachedControllers.set(id, controller);
    controller.connect();
  }

  /** Recalculates overflow synchronously for a custom element or attached host. */
  export function refreshOverflow(id: string): void {
    const element = document.getElementById(id);
    if (element instanceof Overflow) {
      element.refresh();
      return;
    }

    attachedControllers.get(id)?.refresh();
  }

  /** Returns the identifiers of managed items in DOM order. */
  export function getOverflowItemIds(id: string): string[] {
    const element = document.getElementById(id);
    if (element instanceof Overflow) {
      element.refresh();
      return element.getItemIds();
    }

    const controller = attachedControllers.get(id);
    controller?.refresh();
    return controller?.getManagedItems().map((item) => item.id) ?? [];
  }

  /** Releases overflow management and restores visibility changed by the controller. */
  export function disposeOverflow(id: string): void {
    attachedControllers.get(id)?.disconnect();
    attachedControllers.delete(id);

    const element = document.getElementById(id);
    if (element instanceof Overflow) {
      element.dispose();
    }
  }
}
