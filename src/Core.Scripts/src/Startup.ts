import { Microsoft as LoggerFile } from './Utilities/Logger';
import { Microsoft as FluentUIComponentsFile } from './FluentUIWebComponents';
import { Microsoft as FluentPageScriptFile } from './Components/PageScript/FluentPageScript';
import { Microsoft as FluentUIStylesFile } from './FluentUIStyles';
import { Microsoft as FluentUICustomEventsFile } from './FluentUICustomEvents';
import { StartedMode } from './d-ts/StartedMode';

export namespace Microsoft.FluentUI.Blazor.Startup {

  // Alias
  import Logger = LoggerFile.FluentUI.Blazor.Utilities.Logger;
  import FluentUIComponents = FluentUIComponentsFile.FluentUI.Blazor.FluentUIWebComponents;
  import FluentPageScript = FluentPageScriptFile.FluentUI.Blazor.Components.PageScript;
  import FluentUIStyles = FluentUIStylesFile.FluentUI.Blazor.FluentUIStyles;
  import FluentUICustomEvents = FluentUICustomEventsFile.FluentUI.Blazor.FluentUICustomEvents;

  var beforeStartCalled = false;
  var afterStartedCalled = false;

  /**
   * Function to run before the Blazor application starts. This function is called only once.
   */
  export function beforeStart(options: WebStartOptions, mode: StartedMode) {
    if (beforeStartCalled) return;
    Logger.debug(`beforeStart mode "${mode}"`, options);

    // Define Fluent UI components
    FluentUIComponents.defineComponents();

    // Finishing
    beforeStartCalled = true;
  }

  /**
   * Function to run after the Blazor application starts. This function is called only once.
   */
  export function afterStarted(blazor: Blazor, mode: StartedMode) {
    if (afterStartedCalled) return;
    Logger.debug(`afterStarted mode "${mode}"`, blazor);

    // Update the default FluentUI Blazor styles to the document
    FluentUIStyles.applyStyles();

    // Initialize Fluent UI theme
    FluentUIComponents.initializeTheme(blazor);

    // Initialize all custom components
    FluentPageScript.registerComponent(blazor, mode);
    // [^^^ Add your other custom components before this line ^^^]

    // Register all custom events
    FluentUICustomEvents.RadioGroup(blazor);
    // [^^^ Add your other custom events before this line ^^^]

    // Finishing
    afterStartedCalled = true;
  }

  /**
   * Blazor methods: don't use these directly, use the beforeStart or afterStart methods instead.
   */
  export const beforeWebStart = (options: WebStartOptions) => beforeStart(options, StartedMode.Web);
  export const beforeWebAssemblyStart = (options: WebStartOptions) => beforeStart(options, StartedMode.Wasm);
  export const beforeServerStart = (options: WebStartOptions) => beforeStart(options, StartedMode.Server);
  export const afterWebStarted = (blazor: Blazor) => afterStarted(blazor, StartedMode.Web);
  export const afterWebAssemblyStarted = (blazor: Blazor) => afterStarted(blazor, StartedMode.Wasm);
  export const afterServerStarted = (blazor: Blazor) => afterStarted(blazor, StartedMode.Server);
}
