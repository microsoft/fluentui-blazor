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
