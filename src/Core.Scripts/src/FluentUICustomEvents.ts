import { Microsoft as FluentDialogFile } from "./Components/Dialog/FluentDialog";
import { defineOnce } from './RegistrationState';

// Blazor Web Apps can invoke both Web and Server startup hooks. Keep registrations
// in shared global state because .NET 11 rejects registering the same custom event twice.
function registerCustomEventType(blazor: Blazor, eventName: string, options: EventTypeOptions) {
  defineOnce(`fluentui:custom-event:${eventName}`, () => {
    blazor.registerCustomEventType(eventName, options);
  });
}

export namespace Microsoft.FluentUI.Blazor.FluentUICustomEvents {

  /**
  * Initialize the FluentUI custom events
  *
  * 1. Add your register methods here
  * 2. Call your register methods in the `afterStarted` method in the `Startup.ts` file
  *    E.g. `FluentUICustomEvents.RadioGroup(blazor);`
  */

  export function Accordion(blazor: Blazor) {
    registerCustomEventType(blazor, 'accordionchange', {
      browserEventName: 'change',
      createEventArgs: event => {

        const isAccordion = event.target instanceof Element && event.target.tagName.toLowerCase() === 'fluent-accordion';
        if (!isAccordion) {
          return null;
        }

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

    registerCustomEventType(blazor, 'dialogbeforetoggle', {
      browserEventName: 'beforetoggle',
      createEventArgs: (event: any) => {
        FluentDialogFile.FluentUI.Blazor.Components.Dialog.DialogToggle_PreviousActiveElement(event.target.id, event.detail?.newState ?? event.newState);
        return {
          id: event.target.id,
          type: event.type,
          oldState: event.detail?.oldState ?? event.oldState,
          newState: event.detail?.newState ?? event.newState,
        };
      }
    });

    registerCustomEventType(blazor, 'dialogtoggle', {
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
    registerCustomEventType(blazor, 'menuitemchange', {
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
    registerCustomEventType(blazor, 'dropdownchange', {
      browserEventName: 'change',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          type: event.type,
          selectedOptions: event.target.selectedOptions?.map((item: any) => item.id).join(';'),
        };
      }
    });

    registerCustomEventType(blazor, 'listchange', {
      browserEventName: 'listboxchange',
      createEventArgs: (event: any)=> {
        return {
          id: event.srcElement?.id ?? event.detail?.id ?? event.id,
          type: event.type,
          selectedOptions: event.detail?.selectedOptions ?? '',
        };
      }
    });
  }

  export function Tabs(blazor: Blazor) {

    // Event when a tab is selected
    registerCustomEventType(blazor, 'tabchange', {
      browserEventName: 'change',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          activeid: event.detail?.id,
        };
      }
    });
  }

  export function RadioGroup(blazor: Blazor) {

    // Event when a radio element is changed
    registerCustomEventType(blazor, 'radiochange', {
      browserEventName: 'change',
      createEventArgs: (event: any) => {
        return {
          id: event.target.id,
          value: event.target.value,
        };
      }
    });
  }

  export function TreeView(blazor: Blazor) {

    // Event when an element is selected or deselected
    registerCustomEventType(blazor, 'treechanged', {
      browserEventName: 'change',
      createEventArgs: (event: EventType) => {
        return {
          id: event.target.id,
          selected: event.target.selected,
        };
      }
    });

    // Event when an element is expanded or collapsed
    registerCustomEventType(blazor, 'treetoggle', {
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

  export function TextInput(blazor: Blazor) {
    registerCustomEventType(blazor, 'textimmediate', {
      browserEventName: 'immediate',
      createEventArgs: (event: any)=> {
       return {
          id: event.target.id,
          type: event.type,
          value: event.detail?.value ?? event.value,
        };
      }
    });
  }

  export function Overflow(blazor: Blazor) {
    registerCustomEventType(blazor, 'overflowchange', {
      // .NET 11 requires a custom event name to differ from its browser event name.
      // FluentOverflow emits this internal event with the public overflowchange event.
      browserEventName: 'fluentoverflowchange',
      createEventArgs: (event: any) => {
        return {
          id: event.target?.id ?? '',
          items: event.detail?.items ?? [],
          overflowCount: event.detail?.overflowCount ?? 0,
          firstOverflowIndex: event.detail?.firstOverflowIndex ?? -1,
          orderedItemIds: event.detail?.orderedItemIds ?? [],
        };
      }
    });
  }

  // [^^^ Add your other custom events before this line ^^^]

}
