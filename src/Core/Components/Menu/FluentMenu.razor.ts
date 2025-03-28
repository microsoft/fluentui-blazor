export namespace Microsoft.FluentUI.Blazor.Menu {
  export function Initialize(id: string, triggerId: string) {
    const trigger = document.getElementById(triggerId);

    if (trigger) {
      trigger.style["anchor-name" as any] = `--anchor-${triggerId}`;

      const menu = document.getElementById(id) as any;
      if (menu && menu.slottedMenuList.length) {
        menu.slottedTriggers.push(trigger);
        menu.slottedMenuList[0].style["position-anchor" as any] = `--anchor-${triggerId}`;
        menu.setComponent();
      }
    }
  }

  export function CloseMenu(id: string) {
    const menu = document.getElementById(id) as any;
    if (menu) {
      menu.closeMenu();
    }
   }

  export function OpenMenu(id: string) {
    const menu = document.getElementById(id) as any;
    if (menu) {
      menu.openMenu();
    }
  }
}
