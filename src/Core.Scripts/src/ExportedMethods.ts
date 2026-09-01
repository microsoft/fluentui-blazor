import { Microsoft as LoggerFile } from './Utilities/Logger';
import { Microsoft as AttributesFile } from './Utilities/Attributes';
import { Microsoft as KeyPressFile } from './Utilities/KeyPress';
import { Microsoft as FluentDialogFile } from './Components/Dialog/FluentDialog';
import { Microsoft as FluentSortableListFile } from './Components/SortableList/FluentSortableList';
import { Microsoft as FluentTabsFile } from './Components/Tabs/FluentTabs';
import { Microsoft as FluentMultiSplitterFile } from './Components/Splitter/FluentMultiSplitter';
import { Microsoft as FluentLayoutFile } from './Components/Layout/FluentLayout';
import { Microsoft as FluentTextMaskedFile } from './Components/TextInput/TextMasked';
import { Microsoft as FluentTextInput } from './Components/TextInput/TextInput';
import { Microsoft as FluentOverlayFile } from './Components/Overlay/FluentOverlay';
import { Microsoft as FluentListBoxContainerFile } from './Components/List/ListBoxContainer';
import { Microsoft as FluentAutocompleteFile } from './Components/List/FluentAutocomplete';
import { Microsoft as FluentSelectFile } from './Components/List/FluentSelect';
import { Microsoft as FluentMenuFile } from './Components/Menu/FluentMenu';
import { Microsoft as FluentColorPickerFile } from './Components/ColorPicker/FluentColorPicker';
import { Microsoft as FluentKeyCodeFile } from './Components/KeyCode/FluentKeyCode';
import { Microsoft as FluentOverflowFile } from './Components/Overflow/FluentOverflow';

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
    (window as any).Microsoft.FluentUI.Blazor.Components.SortableList = FluentSortableListFile.FluentUI.Blazor.Components.SortableList;
    (window as any).Microsoft.FluentUI.Blazor.Components.Tabs = FluentTabsFile.FluentUI.Blazor.Components.Tabs;
    (window as any).Microsoft.FluentUI.Blazor.Components.MultiSplitter = FluentMultiSplitterFile.FluentUI.Blazor.Components.MultiSplitter;
    (window as any).Microsoft.FluentUI.Blazor.Components.Layout = FluentLayoutFile.FluentUI.Blazor.Components.Layout;
    (window as any).Microsoft.FluentUI.Blazor.Components.TextMasked = FluentTextMaskedFile.FluentUI.Blazor.Components.TextMasked;
    (window as any).Microsoft.FluentUI.Blazor.Components.TextInput = FluentTextInput.FluentUI.Blazor.Components.TextInput;
    (window as any).Microsoft.FluentUI.Blazor.Components.Overlay = FluentOverlayFile.FluentUI.Blazor.Components.Overlay;
    (window as any).Microsoft.FluentUI.Blazor.Components.ListBoxContainer = FluentListBoxContainerFile.FluentUI.Blazor.Components.ListBoxContainer;
    (window as any).Microsoft.FluentUI.Blazor.Components.Autocomplete = FluentAutocompleteFile.FluentUI.Blazor.Components.Autocomplete;
    (window as any).Microsoft.FluentUI.Blazor.Components.Select = FluentSelectFile.FluentUI.Blazor.Components.Select;
    (window as any).Microsoft.FluentUI.Blazor.Components.Menu = FluentMenuFile.FluentUI.Blazor.Components.Menu;
    (window as any).Microsoft.FluentUI.Blazor.Components.ColorPicker = FluentColorPickerFile.FluentUI.Blazor.Components.ColorPicker;
    (window as any).Microsoft.FluentUI.Blazor.Components.KeyCode = FluentKeyCodeFile.FluentUI.Blazor.Components.KeyCode;
    (window as any).Microsoft.FluentUI.Blazor.Components.Overflow = FluentOverflowFile.FluentUI.Blazor.Components.Overflow;

    // [^^^ Add your other exported methods before this line ^^^]
  }
}

