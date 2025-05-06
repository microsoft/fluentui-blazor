import { Microsoft as LoggerFile } from './Utilities/Logger';
import { Microsoft as AttributesFile } from './Utilities/Attributes';
import { Microsoft as KeyPressFile } from './Utilities/KeyPress';
import { Microsoft as FluentDialogFile } from './Components/Dialog/FluentDialog';
import { Microsoft as FluentTabsFile } from './Components/Tabs/FluentTabs';
import { Microsoft as FluentMultiSplitterFile } from './Components/Splitter/FluentMultiSplitter';
import { Microsoft as FluentLayoutFile } from './Components/Layout/FluentLayout';

export namespace Microsoft.FluentUI.Blazor.ExportedMethods {

  /**
    * Initializes the common methods to use with the Fluent UI Blazor library.
    */
  export function initialize() {

    // Create the Microsoft.FluentUI.Blazor namespace
    (window as any).Microsoft = (window as any).Microsoft || {};
    (window as any).Microsoft.FluentUI = (window as any).Microsoft.FluentUI || {};
    (window as any).Microsoft.FluentUI.Blazor = (window as any).Microsoft.FluentUI.Blazor || {};

    // Utilities methods
    (window as any).Microsoft.FluentUI.Blazor.Utilities = (window as any).Microsoft.FluentUI.Blazor.Utilities || {};
    (window as any).Microsoft.FluentUI.Blazor.Utilities.Logger = LoggerFile.FluentUI.Blazor.Utilities.Logger;
    (window as any).Microsoft.FluentUI.Blazor.Utilities.Attributes = AttributesFile.FluentUI.Blazor.Utilities.Attributes;
    (window as any).Microsoft.FluentUI.Blazor.Utilities.KeyPress = KeyPressFile.FluentUI.Blazor.Utilities.KeyPress;

    // Components methods
    (window as any).Microsoft.FluentUI.Blazor.Components = (window as any).Microsoft.FluentUI.Blazor.Components || {};
    (window as any).Microsoft.FluentUI.Blazor.Components.Dialog = FluentDialogFile.FluentUI.Blazor.Components.Dialog;
    (window as any).Microsoft.FluentUI.Blazor.Components.Tabs = FluentTabsFile.FluentUI.Blazor.Components.Tabs;
    (window as any).Microsoft.FluentUI.Blazor.Components.MultiSplitter = FluentMultiSplitterFile.FluentUI.Blazor.Components.MultiSplitter;
    (window as any).Microsoft.FluentUI.Blazor.Components.Layout = FluentLayoutFile.FluentUI.Blazor.Components.Layout;

    // [^^^ Add your other exported methods before this line ^^^]
  }
}

