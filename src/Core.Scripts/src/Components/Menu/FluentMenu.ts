import { Menu, MenuItem, MenuList } from "@fluentui/web-components";
import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Components.Menu {
  const menuAbortControllers = new Map<string, AbortController>();
  const menuListsWithMouseOutWorkaround = new WeakSet<MenuList>();
  const targetAnchorRestorers = new WeakMap<MenuList, () => void>();

  // Closes an open submenu when the pointer leaves, even if its parent menu item retains focus.
  function handleMenuListMouseOut(event: MouseEvent) {
    const relatedTarget = event.relatedTarget;

    for (const target of event.composedPath()) {
      if (!(target instanceof MenuItem)) {
        continue;
      }

      const submenu = target.submenu;
      if (!submenu?.matches(":popover-open")) {
        continue;
      }

      if (relatedTarget instanceof Node && target.contains(relatedTarget)) {
        continue;
      }

      if (submenu.contains(document.activeElement)) {
        continue;
      }

      submenu.togglePopover(false);
    }
  }

  /**
   * Initializes the menu by associating it with a trigger element and setting up the necessary styles for positioning.
   * @param id The id of the fluent-menu element to initialize.
   * @param triggerId The id of the trigger element that will open the menu when clicked.
   * @param openMenu Whether to open the menu after initialization.
   * @param dotNetHelper The .NET component notified when the menu's open state changes.
   */
  export function Initialize(id: string, triggerId: string, openMenu: boolean, dotNetHelper?: DotNet.DotNetObject) {
    // Blazor may invoke initialization before the menu or its generated menu list is rendered, so retry for a few animation frames.
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

      if (!menuListsWithMouseOutWorkaround.has(menuList)) {
        menuList.addEventListener("mouseout", handleMenuListMouseOut);
        menuListsWithMouseOutWorkaround.add(menuList);
      }

      menuAbortControllers.get(id)?.abort();
      if (dotNetHelper) {
        const abortController = new AbortController();
        menuAbortControllers.set(id, abortController);
        menuList.addEventListener("toggle", (event: ToggleEvent) => {
          void dotNetHelper.invokeMethodAsync("FluentMenu.OpenedChangedAsync", event.newState === "open");
        }, { signal: abortController.signal });
      }

      if (openMenu) {
        menu.openMenu();
      }
    };

    initWithRetry();
  }

  /**
   * Removes event listeners associated with the specified menu.
   * @param id The id of the fluent-menu element to dispose.
   */
  export function Dispose(id: string) {
    menuAbortControllers.get(id)?.abort();
    menuAbortControllers.delete(id);
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
   * @param targetId The id of the target element to anchor the menu to. If null, the menu will open relative to its trigger.
   * @param targetOffsetLeft The inline offset from the target element. Default is 0.
   * @param targetOffsetTop The block offset from the target element, applied above the target when the menu flips. Default is 0.
   */
  export function OpenMenu(id: string, targetId: string | null = null, targetOffsetLeft: number = 0, targetOffsetTop: number = 0) {
    const menu = document.getElementById(id) as Menu | null;
    const target = targetId ? document.getElementById(targetId) : null;
    const menuList = menu?.querySelector("fluent-menu-list") as MenuList | null;

    // Position the popover relative to the target element
    if (target && menuList) {
      anchorMenuListToTarget(menuList, target, `--anchor-${targetId}`, targetOffsetLeft, targetOffsetTop);
    }

    menu?.openMenu();
  }

  /**
   * Anchors the menu list to the target so the fluent-menu position fallbacks keep it within the viewport.
   * The original anchoring is restored when the menu closes.
   * @param menuList The fluent-menu-list element to position.
   * @param target The element the menu list is anchored to.
   * @param anchorName The CSS anchor name (dashed ident) shared by the target and the menu list.
   * @param offsetLeft The inline offset, in pixels, from the target.
   * @param offsetTop The block offset, in pixels, from the target (applied upward when the menu flips).
   */
  function anchorMenuListToTarget(menuList: MenuList, target: HTMLElement, anchorName: string, offsetLeft: number, offsetTop: number) {
    // Undo a previous target anchoring if the menu is reopened on another target before closing.
    targetAnchorRestorers.get(menuList)?.();

    const restoreMenuListStyle = saveInlineStyle(menuList, ["position-anchor", "margin-block-start", "margin-inline-start"]);
    const restoreTargetStyle = saveInlineStyle(target, ["anchor-name"]);

    // Override the fluent-menu trigger anchor; its `position-try-fallbacks` then flip the menu around the target.
    target.style.setProperty("anchor-name", anchorName);
    menuList.style.setProperty("position-anchor", anchorName);

    // Margins (not top/left) keep the offsets compatible with the native flip, which mirrors them.
    menuList.style.setProperty("margin-block-start", `${offsetTop}px`);
    menuList.style.setProperty("margin-inline-start", `${offsetLeft}px`);

    // Restores the trigger anchoring so subsequent openings use the default position.
    const restore = () => {
      menuList.removeEventListener("toggle", restoreOnClose);
      targetAnchorRestorers.delete(menuList);
      restoreMenuListStyle();
      restoreTargetStyle();
    };
    const restoreOnClose = (event: ToggleEvent) => {
      if (event.newState === "closed") {
        restore();
      }
    };

    menuList.addEventListener("toggle", restoreOnClose);
    targetAnchorRestorers.set(menuList, restore);
  }

  /**
   * Saves the inline values of the specified style properties and returns a function restoring them.
   */
  function saveInlineStyle(element: HTMLElement, properties: string[]): () => void {
    const savedValues = properties.map(property => [property, element.style.getPropertyValue(property)] as const);

    return () => savedValues.forEach(([property, value]) => {
      if (value) {
        element.style.setProperty(property, value);
      } else {
        element.style.removeProperty(property);
      }
    });
  }
}
