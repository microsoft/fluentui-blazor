export namespace Microsoft.FluentUI.Blazor.FluentUICustomEvents {

  /**
  * Initialize the FluentUI custom events
  *
  * 1. Add your register methods here
  * 2. Call your register methods in the `afterStarted` method in the `Startup.ts` file
  *    E.g. `FluentUICustomEvents.RadioGroup(blazor);`
  */

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

  // [^^^ Add your other custom events before this line ^^^]

}
