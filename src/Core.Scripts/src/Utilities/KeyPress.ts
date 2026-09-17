import { DotNet } from "../d-ts/Microsoft.JSInterop";
import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Utilities.KeyPress {

  export function addKeyPressEventListener(element: HTMLElement, dotnet: DotNet.DotNetObject, keyPress: IKeyPress[]): void {
    if (element == null || element == undefined) {
      return;
    }

    // PreventDefault
    element.addEventListener("keydown", (event: KeyboardEvent) => {

      const matchedKeyPress = getMatchedKeyPress(event, keyPress);

      if (matchedKeyPress && matchedKeyPress.preventDefault === true) {
        event.preventDefault();
      }

    });

    // ChangeAfterKeyPress
    element.addEventListener("keyup", (event: KeyboardEvent) => {

      const matchedKeyPress = getMatchedKeyPress(event, keyPress);

      if (matchedKeyPress) {

        let value = "";
        switch (true) {
          case element instanceof HTMLInputElement:
          case element instanceof HTMLTextAreaElement:
          case element instanceof FluentUIComponents.TextInput:
          case element instanceof FluentUIComponents.TextArea:
            value = element.value;
            break;

          default:
            value = "";
            break;
        }

        dotnet.invokeMethodAsync("ChangeAfterKeyPressHandlerAsync", value, matchedKeyPress);
      }
    });
  }

  function getMatchedKeyPress(event: KeyboardEvent, keyPress: IKeyPress[]): IKeyPress | undefined {
    const ev = event as any;
    const keyCode: number = ev.which || ev.keyCode || ev.charCode;

    return keyPress.find(kp =>
      keyCode === kp.key &&
      event.altKey == kp.altKey &&
      event.ctrlKey == kp.ctrlKey &&
      event.metaKey == kp.metaKey &&
      event.shiftKey == kp.shiftKey
    );
  }

  interface IKeyPress {
    key: number;
    ctrlKey: boolean;
    shiftKey: boolean;
    altKey: boolean;
    metaKey: boolean;
    preventDefault: boolean;
  }

}


