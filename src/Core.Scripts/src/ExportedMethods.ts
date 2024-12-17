import { Microsoft as LoggerFile } from './Utilities/Logger';
import { Microsoft as FluentDialogFile } from './Components/Dialog/FluentDialog';

export namespace Microsoft.FluentUI.Blazor.ExportedMethods {

  /**
    * Initializes the common methods to use with the Fluent UI Blazor library.
    */
  export function initialize() {

    // Create the Microsoft.FluentUI.Blazor namespace
    (window as any).Microsoft = (window as any).Microsoft || {};
    (window as any).Microsoft.FluentUI = (window as any).Microsoft.FluentUI || {};
    (window as any).Microsoft.FluentUI.Blazor = (window as any).Microsoft.FluentUI.Blazor || {};

    // Utilities methods (Logger)
    (window as any).Microsoft.FluentUI.Blazor.Utilities = (window as any).Microsoft.FluentUI.Blazor.Utilities || {};
    (window as any).Microsoft.FluentUI.Blazor.Utilities.Logger = LoggerFile.FluentUI.Blazor.Utilities.Logger;

    // Dialog methods
    (window as any).Microsoft.FluentUI.Blazor.Components = (window as any).Microsoft.FluentUI.Blazor.Components || {};
    (window as any).Microsoft.FluentUI.Blazor.Components.Dialog = FluentDialogFile.FluentUI.Blazor.Components.Dialog;

    // [^^^ Add your other exported methods before this line ^^^]
  }
}

