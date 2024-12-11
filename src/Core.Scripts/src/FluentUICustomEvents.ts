export namespace Microsoft.FluentUI.Blazor.FluentUICustomEvents {

  /**
  * Initialize the FluentUI custom events
  *
  * 1. Add your register methods here
  * 2. Call your register methods in the `afterStarted` method in the `Startup.ts` file
  *    E.g. `FluentUICustomEvents.RadioGroup(blazor);`
  */


  // TODO: Example of custom event for "not yet existing" RadioGroup component
  export function RadioGroup(blazor: Blazor) {

    blazor.registerCustomEventType('radiogroupclick', {
      browserEventName: 'click',
      createEventArgs: event => {
        if (event.target.readOnly || event.target.disabled) {
          return null;
        }
        return {
          value: event.target.value
        };
      }
    });

  }
}
