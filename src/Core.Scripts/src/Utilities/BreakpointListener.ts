import { DotNet } from "../d-ts/Microsoft.JSInterop";

export namespace Microsoft.FluentUI.Blazor.Utilities.BreakpointListener {

  interface BreakpointItem {
    body: HTMLElement;
    listener: EventListener;
  }

  const breakpointItems = new Map<string, BreakpointItem>();

  /**
   * Reads the current viewport breakpoint published by the library's media-query tracking.
   * @param body The body element containing the data-media attribute.
   * @returns The current breakpoint name, such as xs, md, or xxl.
   * @throws If the data-media attribute has not been initialized.
   */
  function getBreakpoint(body: HTMLElement): string {
    const media = body.getAttribute('data-media');
    if (media === null) {
      throw new Error('The viewport breakpoint is unavailable. Ensure the Fluent UI Blazor script is loaded.');
    }

    return media;
  }

  /**
   * Subscribes a provider to viewport breakpoint changes and publishes any available initial value.
   * @param id A unique subscription identifier; it does not need to identify a DOM element.
   * @param dotNetHelper The .NET object exposing the OnBreakpointChangedAsync callback.
   * @returns A promise that resolves after registration and publication of any available initial value.
   * @throws If the identifier is already registered or the initial callback fails.
   */
  export async function initialize(id: string, dotNetHelper: DotNet.DotNetObject): Promise<void> {
    if (breakpointItems.has(id)) {
      throw new Error(`A viewport breakpoint observer with ID '${id}' is already registered.`);
    }

    const body = document.body;
    const listener: EventListener = () => {
      void dotNetHelper.invokeMethodAsync<void>('OnBreakpointChangedAsync', getBreakpoint(body))
                       .catch(error => console.error('Error publishing the viewport breakpoint:', error));
    };

    const newItem = { body, listener };
    breakpointItems.set(id, newItem);
    body.addEventListener('mediaChanged', listener);

    // The startup initializer may publish the first measurement after this provider mounts.
    const media = body.getAttribute('data-media');
    if (media === null) {
      return;
    }

    try {
      await dotNetHelper.invokeMethodAsync<void>('OnBreakpointChangedAsync', media);
    } 
    catch (error) {
      if (breakpointItems.get(id) === newItem) {
        dispose(id);
      }

      throw error;
    }
  }

  /**
   * Removes a provider's breakpoint listener. Repeated calls with the same identifier are safe.
   * @param id The subscription identifier supplied to initialize.
   */
  export function dispose(id: string): void {
    const item = breakpointItems.get(id);
    if (item) {
      item.body.removeEventListener('mediaChanged', item.listener);
      breakpointItems.delete(id);
    }
  }
}
