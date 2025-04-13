export namespace Microsoft.FluentUI.Blazor.FluentUICustomEvents {

  /**
  * Initialize the FluentUI custom events
  *
  * 1. Add your register methods here
  * 2. Call your register methods in the `afterStarted` method in the `Startup.ts` file
  *    E.g. `FluentUICustomEvents.RadioGroup(blazor);`
  */

  export function Tree(blazor: Blazor) {
    blazor.registerCustomEventType('selectedchange', {
      browserEventName: 'change',
      createEventArgs: event => {
        if (event.target!.localName == 'fluent-tree-item') {
          return {
            affectedId: event.detail.attributes['id'].value,
            selected: event.detail._selected,
            expanded: event.detail._expanded
          }
        }

        return null;
      }
    });

    blazor.registerCustomEventType('expandedchange', {
      browserEventName: 'expanded-change',
      createEventArgs: event => {
        return {
          affectedId: event.detail.attributes['id'].value,
          selected: event.detail._selected,
          expanded: event.detail._expanded
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

  // [^^^ Add your other custom events before this line ^^^]

}
