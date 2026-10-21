import type { Menu } from "@fluentui/web-components";

/**
 * Keeps a fluent-menu closed while its trigger is disabled.
 * The menu opens from its trigger (click, Enter/Space keydown, hover, context menu) without checking whether
 * the trigger is disabled. A `disabled-focusable` trigger stays focusable and still receives these events.
 * Every way of opening shows the menu list as a popover, so its cancelable `beforetoggle` event is where
 * the menu is kept closed while its trigger is disabled.
 */
export namespace Microsoft.FluentUI.Blazor.Components.Menu {

  /**
   * Keeps the menu closed while its trigger is disabled.
   * The listener is on the menu itself, so it goes away with the menu, and adding it again does nothing.
   * `beforetoggle` does not bubble, but the capture phase still reaches the menu from its menu list.
   * @param menu The fluent-menu element.
   */
  export function keepClosedWhileTriggerDisabled(menu: Menu): void {
    menu.addEventListener('beforetoggle', cancelOpeningFromDisabledTrigger, { capture: true });
  }

  /**
   * Cancels the opening of the menu's own menu list (not of a submenu) while the menu's trigger is disabled.
   * Closing is never cancelled, so a menu that was open when its trigger got disabled can still be closed.
   * @param e The `beforetoggle` event.
   */
  function cancelOpeningFromDisabledTrigger(e: Event): void {
    const menu = e.currentTarget as Menu;
    const target = e.target as Element;
    if ((e as ToggleEvent).newState !== 'open' || target.parentElement !== menu) {
      return;
    }

    if (getTrigger(menu)?.matches('[disabled], [disabled-focusable]')) {
      e.preventDefault();
    }
  }

  /**
   * Gets the menu's trigger: the element in its `trigger` slot or,
   * for a menu opened by an element elsewhere (`FluentMenu.Trigger`), the element whose id is in its `trigger` attribute.
   * @param menu The fluent-menu element.
   */
  function getTrigger(menu: Menu): Element | null {
    const triggerId = menu.getAttribute('trigger');
    return triggerId
      ? document.getElementById(triggerId)
      : menu.querySelector(':scope > [slot="trigger"]');
  }
}
