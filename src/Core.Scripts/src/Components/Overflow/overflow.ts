/**
 * Defines the main `Overflow` class and the Web Component behavior.
 *
 * `<fluent-overflow>` displays as many direct children as available space allows.
 *
 * @fires overflowchange - Reports the current overflow state.
 * @fires fluentoverflowchange - Fluent-prefixed alias of `overflowchange` with the same detail.
 * @fires fluentoverflowtoggle - Reports when the slotted Fluent tooltip opens or closes.
 *
 * @slot - Items managed by the overflow controller.
 * @slot trigger - Overflow trigger. A `[target="count"]` descendant receives the hidden count.
 * @slot menu - Overflow menu. A `[target="content"]` descendant receives the hidden item labels.
 */

import { template } from './overflow.template';
import { Microsoft as OverflowControllerFile } from './overflow-controller';
import { StartedMode } from '../../d-ts/StartedMode';
import { tagName } from './overflow.types';
import type { OverflowDirection, OverflowOrientation, OverflowToggleDetail } from './overflow.types';

export namespace Microsoft.FluentUI.Blazor.Components.Overflow {

  import OverflowController = OverflowControllerFile.FluentUI.Blazor.Components.Overflow.OverflowController;

  export function registerComponent(blazor: Blazor, mode: StartedMode): void {
    if (!customElements.get(tagName)) {
      customElements.define(tagName, Overflow);
    }
  }

  const customizationSlots = new Set(['trigger', 'menu']);

  /** Custom element that presents overflow state calculated by `OverflowController`. */
  export class Overflow extends HTMLElement {
    static readonly observedAttributes = [
      'max-overflow-items',
      'pre-overflow-count',
      'overflow-direction',
      'orientation',
      'selector',
      'threshold',
      'visible-on-load',
    ];

    private readonly container: HTMLElement;
    private readonly itemsContainer: HTMLElement;
    private readonly triggerSlot: HTMLSlotElement;
    private readonly menuSlot: HTMLSlotElement;
    private controller?: OverflowController;
    private lastPresentedCount = '';

    /** Creates the shadow DOM and caches its layout elements. */
    constructor() {
      super();
      const shadowRoot = this.attachShadow({ mode: 'open' });
      shadowRoot.append(template.content.cloneNode(true));

      this.container = shadowRoot.querySelector<HTMLElement>('[part="container"]')!;
      this.itemsContainer = shadowRoot.querySelector<HTMLElement>('[part="items"]')!;
      this.triggerSlot = shadowRoot.querySelector<HTMLSlotElement>('slot[name="trigger"]')!;
      this.menuSlot = shadowRoot.querySelector<HTMLSlotElement>('slot[name="menu"]')!;
    }

    /** Logical side from which items are hidden when space runs out. */
    get overflowDirection(): OverflowDirection {
      return this.getAttribute('overflow-direction') === 'start' ? 'start' : 'end';
    }

    /** Updates the logical side from which items are hidden. */
    set overflowDirection(value: OverflowDirection) {
      this.setAttribute('overflow-direction', value === 'start' ? 'start' : 'end');
    }

    /** Axis used to arrange and measure managed items. */
    get orientation(): OverflowOrientation {
      return this.getAttribute('orientation') === 'vertical' ? 'vertical' : 'horizontal';
    }

    /** Updates the axis used to arrange and measure managed items. */
    set orientation(value: OverflowOrientation) {
      this.setAttribute('orientation', value === 'vertical' ? 'vertical' : 'horizontal');
    }

    /** Maximum number of items exposed in event and tooltip payloads. */
    get maxOverflowItems(): number {
      if (!this.hasAttribute('max-overflow-items')) {
        return Number.POSITIVE_INFINITY;
      }

      const value = Number(this.getAttribute('max-overflow-items'));
      return Number.isFinite(value) && value > 0
        ? Math.floor(value)
        : Number.POSITIVE_INFINITY;
    }

    /** Number of items displayed before overflow begins. */
    get preOverflowCount(): number {
      const value = Number(this.getAttribute('pre-overflow-count'));
      return Number.isFinite(value) && value > 0 ? Math.floor(value) : 0;
    }

    /** Updates the maximum number of overflow items included in payloads. */
    set maxOverflowItems(value: number) {
      if (Number.isFinite(value)) {
        this.setAttribute('max-overflow-items', String(Math.max(0, Math.floor(value))));
      } else {
        this.removeAttribute('max-overflow-items');
      }
    }

    /** Additional space, in pixels, reserved before overflow begins. */
    get threshold(): number {
      const value = Number(this.getAttribute('threshold'));
      return Number.isFinite(value) && value > 0 ? value : 0;
    }

    /** Updates the space reserved before overflow begins. */
    set threshold(value: number) {
      const normalizedValue = Number.isFinite(value) ? Math.max(0, value) : 0;
      this.setAttribute('threshold', String(normalizedValue));
    }

    /** CSS selector used to choose managed direct children. */
    get selector(): string {
      return this.getAttribute('selector') ?? '';
    }

    /** Updates the selector used to choose managed children. */
    set selector(value: string) {
      if (value) {
        this.setAttribute('selector', value);
      } else {
        this.removeAttribute('selector');
      }
    }

    /** Whether the component is visible before its first layout completes. */
    get visibleOnLoad(): boolean {
      return this.getAttribute('visible-on-load') !== 'false';
    }

    /** Controls whether the component is visible before its first layout completes. */
    set visibleOnLoad(value: boolean) {
      this.setAttribute('visible-on-load', String(value));
    }

    /** Starts overflow management when the element enters the document. */
    connectedCallback(): void {
      this.stopOverflowManagement();
      this.setDefaultAttributes();
      this.triggerSlot.addEventListener('slotchange', this.handleCustomizationSlotChange);
      this.menuSlot.addEventListener('slotchange', this.handleCustomizationSlotChange);
      this.addEventListener('toggle', this.handleTooltipToggle, true);

      this.controller?.disconnect();
      this.controller = this.createController();
      this.controller.connect();
    }

    /** Stops overflow management when the element leaves the document. */
    disconnectedCallback(): void {
      this.stopOverflowManagement();
    }

    /** Requests a new layout after an observed attribute changes. */
    attributeChangedCallback(): void {
      this.controller?.requestRefresh();
    }

    /** Recalculates overflow synchronously. */
    refresh(): void {
      this.controller?.refresh();
    }

    /** Returns the identifiers of managed items in DOM order. */
    getItemIds(): string[] {
      return this.controller?.getManagedItems().map((item) => item.id) ?? [];
    }

    /** Stops overflow management and restores visibility owned by the controller. */
    dispose(): void {
      this.stopOverflowManagement();
      this.triggerSlot.hidden = true;
      this.lastPresentedCount = '';
    }

    /** Applies default accessibility and layout attributes. */
    private setDefaultAttributes(): void {
      if (!this.hasAttribute('role')) {
        this.setAttribute('role', 'group');
      }
      if (!this.hasAttribute('overflow-direction')) {
        this.setAttribute('overflow-direction', 'end');
      }
      if (!this.hasAttribute('orientation')) {
        this.setAttribute('orientation', 'horizontal');
      }
    }

    /** Creates the controller adapters used by this custom element. */
    private createController(): OverflowController {
      const overflow = this;
      return new OverflowController({
        host: overflow,
        get querySelector() { return overflow.selector; },
        get threshold() { return overflow.threshold; },
        get maxRenderedItems() { return overflow.maxOverflowItems; },
        get preOverflowCount() { return overflow.preOverflowCount; },
        get visibleOnLoad() { return overflow.visibleOnLoad; },
        get orientation() { return overflow.orientation; },
        get overflowDirection() { return overflow.overflowDirection; },
        get layoutContainer() { return overflow.container; },
        get gapContainer() { return overflow.itemsContainer; },
        get layoutChildren() {
          return Array.from(overflow.children)
            .filter((item): item is HTMLElement => item instanceof HTMLElement
              && !customizationSlots.has(item.getAttribute('slot') ?? ''));
        },
        get reservedElement() { return overflow.getTriggerElement(); },
        setOverflowActive: (active) => {
          overflow.triggerSlot.hidden = !active;
        },
        onLayoutChanged: () => overflow.updateOverflowPresentation(),
      });
    }

    /** Disconnects the controller and removes presentation listeners. */
    private stopOverflowManagement(): void {
      this.controller?.disconnect();
      this.controller = undefined;
      this.triggerSlot.removeEventListener('slotchange', this.handleCustomizationSlotChange);
      this.menuSlot.removeEventListener('slotchange', this.handleCustomizationSlotChange);
      this.removeEventListener('toggle', this.handleTooltipToggle, true);
    }

    /** Returns the first element assigned to the trigger slot. */
    private getTriggerElement(): Element | undefined {
      return this.triggerSlot.assignedElements()[0];
    }

    /** Synchronizes trigger and menu content with the hidden items. */
    private updateOverflowPresentation(): void {
      const hiddenItems = this.controller?.getHiddenItems() ?? [];
      const totalCount = hiddenItems.length + this.preOverflowCount;
      const count = String(totalCount);
      const itemTexts = hiddenItems
        .slice(0, this.maxOverflowItems)
        .map((item) => (item.textContent ?? '').trim());
      this.triggerSlot.hidden = totalCount === 0;

      const trigger = this.getTriggerElement();
      if (trigger) {
        this.setAttributeIfChanged(trigger, 'aria-label', `${count} hidden items`);
        this.updateTargetContent(trigger, 'count', count);
      }

      const menu = this.menuSlot.assignedElements()[0];
      if (menu) {
        const contentTarget = menu.getAttribute('target') === 'content'
          ? menu
          : menu.querySelector('[target="content"]');
        if (contentTarget) {
          this.updateMenuContent(contentTarget, itemTexts);
        }
      }

      if (this.lastPresentedCount && this.lastPresentedCount !== count) {
        this.controller?.requestRefresh();
      }
      this.lastPresentedCount = count;
    }

    /** Updates text in the descendant matching a named target. */
    private updateTargetContent(element: Element, targetName: string, value: string): void {
      const target = element.getAttribute('target') === targetName
        ? element
        : element.querySelector(`[target="${targetName}"]`);
      if (target && target.textContent !== value) {
        target.textContent = value;
      }
    }

    /** Rebuilds the menu only when its item labels have changed. */
    private updateMenuContent(container: Element, itemTexts: string[]): void {
      const currentTexts = Array.from(container.children).map((item) => item.textContent ?? '');
      const unchanged = currentTexts.length === itemTexts.length
        && currentTexts.every((text, index) => text === itemTexts[index]);
      if (unchanged) {
        return;
      }

      const entries = itemTexts.map((text) => {
        const entry = document.createElement('div');
        entry.textContent = text;
        return entry;
      });
      container.replaceChildren(...entries);
    }

    /** Sets an attribute only when its value differs. */
    private setAttributeIfChanged(element: Element, name: string, value: string): void {
      if (element.getAttribute(name) !== value) {
        element.setAttribute(name, value);
      }
    }

    /** Refreshes the presentation when a customization slot changes. */
    private readonly handleCustomizationSlotChange = (): void => {
      this.updateOverflowPresentation();
      this.controller?.requestRefresh();
    };

    /** Emits overflow details when the slotted tooltip opens or closes. */
    private readonly handleTooltipToggle = (event: Event): void => {
      const tooltip = this.menuSlot.assignedElements().find((element) => element.localName === 'fluent-tooltip');
      if (event.target !== tooltip) {
        return;
      }

      const toggleEvent = event as ToggleEvent & { detail?: { newState?: string } };
      const newState = toggleEvent.newState ?? toggleEvent.detail?.newState;
      if (newState !== 'open' && newState !== 'closed') {
        return;
      }

      const hiddenItems = this.controller?.getHiddenItems() ?? [];
      const detail: OverflowToggleDetail = {
        opened: newState === 'open',
        totalItemsCount: this.controller?.getManagedItems().length ?? 0,
        totalHiddenCount: hiddenItems.length,
        items: this.controller?.getHiddenOverflowItems(this.maxOverflowItems) ?? [],
      };
      this.dispatchEvent(new CustomEvent<OverflowToggleDetail>('fluentoverflowtoggle', {
        bubbles: true,
        composed: true,
        detail,
      }));
    };
  }
}