export namespace Microsoft.FluentUI.Blazor.KeyCode {

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

    if ((document as any).fluentKeyCodeEvents == null) {
      (document as any).fluentKeyCodeEvents = {};
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

      const handler = function (e: KeyboardEvent, netMethod: string) {
        const ev = e as any;
        const keyCode = ev.which || ev.keyCode || ev.charCode;

        if (stopRepeat && e.repeat) {
          return;
        }

        if (!!dotNetHelper && !!dotNetHelper.invokeMethodAsync) {

          const targetId = ev.currentTarget?.id ?? "";
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
      (document as any).fluentKeyCodeEvents[eventId] = { source: element, handlerKeydown, handlerKeyup };

      return eventId;
    }

    return "";
  }

  /**
   * Unregisters a key code event handler for the specified event ID.
   * @param eventId
   */
  export function UnregisterKeyCode(eventId: string) {

    if ((document as any).fluentKeyCodeEvents != null) {
      const keyEvent = (document as any).fluentKeyCodeEvents[eventId];
      const element = keyEvent.source;

      if (!!keyEvent.handlerKeydown) {
        element.removeEventListener("keydown", keyEvent.handlerKeydown);
      }

      if (!!keyEvent.handlerKeyup) {
        element.removeEventListener("keyup", keyEvent.handlerKeyup);
      }

      delete (document as any).fluentKeyCodeEvents[eventId];
    }
  }
}
