import { Microsoft as FluentTreeView } from './Components/TreeView/FluentTreeView';

export namespace Microsoft.FluentUI.Blazor.FluentUICustomEvents {

  /**
  * Initialize the FluentUI custom events
  *
  * 1. Add your register methods here
  * 2. Call your register methods in the `afterStarted` method in the `Startup.ts` file
  *    E.g. `FluentUICustomEvents.RadioGroup(blazor);`
  */

  export function Accordion(blazor: Blazor) {
    blazor.registerCustomEventType('accordionchange', {
      browserEventName: 'change',
      createEventArgs: event => {
        const item: any = event.target.accordionItems[event.target.activeItemIndex];
        const header = item?.querySelector(`[slot="heading"]`)?.innerText ?? null;
        return {
          id: item?.id ?? "",
          expanded: item?._expanded ?? null,
          headerText: header
        };
      }
    });
  }

  export function DialogToggle(blazor: Blazor) {

    blazor.registerCustomEventType('dialogbeforetoggle', {
      browserEventName: 'beforetoggle',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          type: event.type,
          oldState: event.detail?.oldState ?? event.oldState,
          newState: event.detail?.newState ?? event.newState,
        };
      }
    });

    blazor.registerCustomEventType('dialogtoggle', {
      browserEventName: 'toggle',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          type: event.type,
          oldState: event.detail?.oldState ?? event.oldState,
          newState: event.detail?.newState ?? event.newState,
        };
      }
    });
  }

  export function MenuItem(blazor: Blazor) {
    blazor.registerCustomEventType('menuitemchange', {
      browserEventName: 'change',
      createEventArgs: event => {
        return {
          id: event.target.id,
          text: event.target.innerText,
          checked: event.target._role == 'menuitem' ? null : event.target._checked,
        };
      }
    });
  }

  export function DropdownList(blazor: Blazor) {

    // Event when an element is selected or deselected
    // from the dropdown list: listbox, select, combobox, ...
    blazor.registerCustomEventType('dropdownchange', {
      browserEventName: 'change',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          type: event.type,
          selectedOptions: event.target.selectedOptions.map((item: any) => item.id).join(';'),
        };
      }
    });
  }
  export function Tabs(blazor: Blazor) {

    // Event when a tab is selected
    blazor.registerCustomEventType('tabchange', {
      browserEventName: 'change',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          activeid: event.detail?.id,
        };
      }
    });
  }

  export function TreeView(blazor: Blazor) {

    // Event when an element is selected or deselected
    blazor.registerCustomEventType('treechanged', {
      browserEventName: 'change',
      createEventArgs: (event: EventType) => {
        return {
          id: event.target.id,
          selected: event.target.selected,
        };
      }
    });

    // Event when an element is expanded or collapsed
    blazor.registerCustomEventType('treetoggle', {
      browserEventName: 'toggle',
      createEventArgs: (event: any) => {
        console.log("toggle", event);
        return {
          id: event.target.id,
          type: event.type,
          oldState: event.detail?.oldState ?? event.oldState,
          newState: event.detail?.newState ?? event.newState,
        };
      }
    });

    /**
     * Toggle the expand/collapse tree item.
     * And selecte the item.
     */
    ((window as any).Blazor as any).__toggleTreeItem = FluentTreeView.FluentUI.Blazor.Components.TreeView.ToggleItem
  }

  // [^^^ Add your other custom events before this line ^^^]

}
