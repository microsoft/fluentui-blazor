import { DotNet } from "../d-ts/Microsoft.JSInterop";
import * as FluentUIComponents from '@fluentui/web-components'

export namespace Microsoft.FluentUI.Blazor.Utilities.KeyPress {

  export function addKeyPressEventListener(element: HTMLElement, dotnet: DotNet.DotNetObject, keyPress: IKeyPress[]): void {
    if (element == null || element == undefined) {
      return;
    }

    element.addEventListener("keyup", (event: KeyboardEvent) => {
      const ev = event as any;
      const keyCode: number = ev.which || ev.keyCode || ev.charCode;

      const matchedKeyPress = keyPress.find(kp =>
        keyCode === kp.key &&
        event.altKey == kp.altKey &&
        event.ctrlKey == kp.ctrlKey &&
        event.metaKey == kp.metaKey &&
        event.shiftKey == kp.shiftKey
      );

      if (matchedKeyPress) {

        if (matchedKeyPress.preventDefault === true) {
          event.preventDefault();
        }

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

  interface IKeyPress {
    key: number;
    ctrlKey: boolean;
    shiftKey: boolean;
    altKey: boolean;
    metaKey: boolean;
    preventDefault: boolean;
  }

}


