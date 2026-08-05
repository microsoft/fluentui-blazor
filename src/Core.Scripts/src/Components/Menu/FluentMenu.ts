import { Menu, MenuList } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.Menu {

  /**
   * Initializes the menu by associating it with a trigger element and setting up the necessary styles for positioning.
   * @param id The id of the fluent-menu element to initialize.
   * @param triggerId The id of the trigger element that will open the menu when clicked.
   * @param openMenu Whether to open the menu after initialization.
   */
  export function Initialize(id: string, triggerId: string, openMenu: boolean) {
    const initWithRetry = (attempt: number = 0) => {
      const trigger = document.getElementById(triggerId) as HTMLElement | null;
      const menu = document.getElementById(id) as Menu | null;
      if (!trigger || !menu) {
        if (attempt < 10) {
          requestAnimationFrame(() => initWithRetry(attempt + 1));
        }
        return;
      }

      trigger.style["anchor-name" as any] = `--anchor-${triggerId}`;

      if (trigger.getAttribute("role") === null) {
        trigger.setAttribute("role", "button");
      }

      // Keep trigger wiring explicit for hosted surfaces (drawer/dialog/shadow-heavy layouts).
      menu.setAttribute("trigger", triggerId);

      const menuList = menu.slottedMenuList?.[0] as MenuList | undefined;
      if (!menuList) {
        if (attempt < 10) {
          requestAnimationFrame(() => initWithRetry(attempt + 1));
        }
        return;
      }

      menuList.style["position-anchor" as any] = `--anchor-${triggerId}`;
      menu.slottedTriggersChanged(menu.slottedTriggers ?? [], [trigger]);

      if (openMenu) {
        menu.openMenu();
      }
    };

    initWithRetry();
  }

  /**
   * Closes the menu with the specified id.
   * @param id 
   */
  export function CloseMenu(id: string) {
    const menu = document.getElementById(id) as Menu | null;
    if (menu) {
      menu.closeMenu();
    }
  }

  /**
   * Opens the menu with the specified id, optionally positioning it relative to a target element with specified offsets.
   * @param id The id of the fluent-menu element to open.
   * @param targetId The id of the target element to position the menu relative to. If null, the menu will open in its default position.
   * @param targetOffsetLeft The left offset from the target element to open the menu. Default is 0.
   */
  export function OpenMenu(id: string, targetId: string | null = null, targetOffsetLeft: number = 0, targetOffsetTop: number = 0) {
    const menu = document.getElementById(id) as Menu;

    if (targetId) {
      const target = document.getElementById(targetId);
      const menuList = menu.querySelector("fluent-menu-list") as MenuList;

      // Position the popover relative to the target element
      if (target && menuList) {
        const rect = target.getBoundingClientRect();
        menuList.style.position = "fixed";
        menuList.style.margin = "0";
        menuList.style.top = `${rect.bottom + targetOffsetTop}px`;
        menuList.style.left = `${rect.left + targetOffsetLeft}px`;
      }
    }

    if (menu) {
      menu.openMenu();
    }
  }
}
