import { Menu, MenuList } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.Menu {

  /**
   * Initializes the menu by associating it with a trigger element and setting up the necessary styles for positioning.
   * @param id The id of the fluent-menu element to initialize.
   * @param triggerId The id of the trigger element that will open the menu when clicked.
   */
  export function Initialize(id: string, triggerId: string) {
    const trigger = document.getElementById(triggerId) as HTMLElement;

    if (trigger) {
      trigger.style["anchor-name" as any] = `--anchor-${triggerId}`;

      const menu = document.getElementById(id) as Menu;
      if (menu && menu.slottedMenuList.length) {
        menu.slottedMenuList[0].style["position-anchor" as any] = `--anchor-${triggerId}`;
        menu.slottedTriggersChanged(menu.slottedTriggers, [trigger]);
      }
    }
  }

  /**
   * Closes the menu with the specified id.
   * @param id 
   */
  export function CloseMenu(id: string) {
    const menu = document.getElementById(id) as Menu;
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
