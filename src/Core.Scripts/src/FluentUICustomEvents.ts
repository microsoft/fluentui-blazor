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


  // [^^^ Add your other custom events before this line ^^^]

}
