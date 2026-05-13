import { Menu, MenuList } from "@fluentui/web-components";

export namespace Microsoft.FluentUI.Blazor.Components.Menu {
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

  export function CloseMenu(id: string) {
    const menu = document.getElementById(id) as Menu;
    if (menu) {
      menu.closeMenu();
    }
  }

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
