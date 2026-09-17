export namespace Microsoft.FluentUI.Blazor.Components.KeyCode {

  /**
   * Registers a key code event handler for the specified element.
   * This function integrates with the FluentKeyCode Blazor component to handle key events such as KeyDown and KeyUp.
   *
   * @param globalDocument - Indicates whether the event handler should be applied globally to the document or to a specific element.
   * @param eventNames - A semicolon-separated list of event names to listen for (e.g., "KeyDown;KeyUp").
   * @param id - The ID of the target element. Ignored if `elementRef` is provided.
   * @param elementRef - A reference to the target DOM element. If null, the element is resolved using the `id`.
   * @param onlyCodes - A list of key codes to exclusively allow for event handling.
   * @param excludeCodes - A list of key codes to ignore during event handling.
   * @param stopPropagation - If true, prevents further propagation of the event in the DOM.
   * @param preventDefault - If true, prevents the default action associated with the event.
   * @param preventDefaultOnly - A list of key codes for which the default action should be prevented.
   * @param dotNetHelper - A .NET object reference used to invoke Blazor methods asynchronously.
   * @param preventMultipleKeydown - If true, prevents multiple KeyDown events from being fired consecutively.
   * @param stopRepeat - If true, prevents handling of repeated key events when a key is held down.
   * @returns A unique event ID that can be used to unregister the event handler later.
  */
  export function RegisterKeyCode(
    globalDocument: boolean,
    eventNames: string,
    id: string,
    elementRef: any,
    onlyCodes: any,
    excludeCodes: any,
    stopPropagation: boolean,
    preventDefault: boolean,
    preventDefaultOnly: any,
    dotNetHelper: any,
    preventMultipleKeydown: boolean,
    stopRepeat: boolean) {

    const element = globalDocument
      ? document
      : elementRef == null ? document.getElementById(id) : elementRef;

    if ((document as any)._fluentKeyCodeEvents == null) {
      (document as any)._fluentKeyCodeEvents = {};
    }

    if (!!element) {

      const eventId = Math.random().toString(36).slice(2);
      let fired = false;

      const handlerKeydown = function (e: KeyboardEvent) {
        if (!fired || !preventMultipleKeydown) {
          fired = true;
          return handler(e, "OnKeyDownRaisedAsync");
        }
      }

      const handlerKeyup = function (e: KeyboardEvent) {
        fired = false;
        return handler(e, "OnKeyUpRaisedAsync");
      }

      const handler = function (event: KeyboardEvent, netMethod: string) {
        const e = new SafeKeyboardEvent(event);
        const keyCode = e.keyCode;

        if (stopRepeat && e.repeat) {
          return;
        }

        if (!!dotNetHelper && !!dotNetHelper.invokeMethodAsync) {
          const targetId = e.targetId;
          const isPreventDefault = preventDefault || (preventDefaultOnly.length > 0 && preventDefaultOnly.includes(keyCode));
          const isStopPropagation = stopPropagation;

          // Exclude
          if (excludeCodes.length > 0 && excludeCodes.includes(keyCode)) {
            if (isPreventDefault) {
              e.preventDefault();
            }
            if (isStopPropagation) {
              e.stopPropagation();
            }
            return;
          }

          // All or Include only
          if (onlyCodes.length == 0 || (onlyCodes.length > 0 && onlyCodes.includes(keyCode))) {
            if (isPreventDefault) {
              e.preventDefault();
            }
            if (isStopPropagation) {
              e.stopPropagation();
            }
            dotNetHelper.invokeMethodAsync(netMethod, keyCode, e.key, e.ctrlKey, e.shiftKey, e.altKey, e.metaKey, e.location, targetId, e.repeat);
            return;
          }
        }
      };

      if (preventMultipleKeydown || (!!eventNames && eventNames.includes("KeyDown"))) {
        element.addEventListener('keydown', handlerKeydown)
      }
      if (preventMultipleKeydown || (!!eventNames && eventNames.includes("KeyUp"))) {
        element.addEventListener('keyup', handlerKeyup)
      }
      (document as any)._fluentKeyCodeEvents[eventId] = { source: element, handlerKeydown, handlerKeyup };

      return eventId;
    }

    return "";
  }

  /**
   * Unregisters a key code event handler for the specified event ID.
   * @param eventId
   */
    export function UnregisterKeyCode(eventId: string) {
      const events = (document as any)._fluentKeyCodeEvents;
      const keyEvent = events?.[eventId];

      if (!keyEvent) {
          return;
      }

      const element = keyEvent.source;

      if (element && keyEvent.handlerKeydown) {
          element.removeEventListener("keydown", keyEvent.handlerKeydown);
      }

      if (element && keyEvent.handlerKeyup) {
          element.removeEventListener("keyup", keyEvent.handlerKeyup);
      }

      delete events[eventId];
    }

  /**
   * A safe, read-only snapshot of the fields needed from a keyboard event, guaranteed
   * to have valid types even when the source event is missing properties (e.g. a
   * synthetic or malformed event dispatched instead of a real KeyboardEvent).
   */
  class SafeKeyboardEvent {
    private readonly source: KeyboardEvent;

    constructor(event: KeyboardEvent) {
      const ev = event as any;
      this.source = event;
      this.keyCode = SafeKeyboardEvent.toSafeInt(ev.which ?? ev.keyCode ?? ev.charCode);
      this.key = SafeKeyboardEvent.toSafeString(ev.key);
      this.ctrlKey = SafeKeyboardEvent.toSafeBool(ev.ctrlKey);
      this.shiftKey = SafeKeyboardEvent.toSafeBool(ev.shiftKey);
      this.altKey = SafeKeyboardEvent.toSafeBool(ev.altKey);
      this.metaKey = SafeKeyboardEvent.toSafeBool(ev.metaKey);
      this.location = SafeKeyboardEvent.toSafeInt(ev.location);
      this.repeat = SafeKeyboardEvent.toSafeBool(ev.repeat);
      this.targetId = SafeKeyboardEvent.toSafeString(ev.currentTarget?.id);
    }

    readonly keyCode: number;
    readonly key: string;
    readonly ctrlKey: boolean;
    readonly shiftKey: boolean;
    readonly altKey: boolean;
    readonly metaKey: boolean;
    readonly location: number;
    readonly repeat: boolean;
    readonly targetId: string;

    preventDefault(): void {
      this.source.preventDefault();
    }

    stopPropagation(): void {
      this.source.stopPropagation();
    }

    private static toSafeString(value: unknown): string {
      return typeof value === "string" ? value : "";
    }

    private static toSafeInt(value: unknown): number {
      const result = Number(value);
      return Number.isInteger(result) ? result : 0;
    }

    private static toSafeBool(value: unknown): boolean {
      return typeof value === "boolean" ? value : false;
    }
  }
}