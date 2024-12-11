/*

  This file is the entry point for the Fluent UI Blazor library.
  It re-exports the `beforeStart` and `afterStarted` methods from the `Startup` file,
  which are used to define and initialize the Fluent UI components and styles.

  -  To create a new custom component, you can add a new file in the `Components` folder,
     and register it in the `afterStarted` method in the `Startup` file.

  -  To create a new custom event, you can add a new function in the `FluentUICustomEvents` file,
     and register it in the `afterStarted` method in the `Startup` file.

  -  All FluentUI WebComponents are defined and initialized in the `FluentUIWebComponents` file.
*/

import { Microsoft as StartupFile } from './Startup';
import Startup = StartupFile.FluentUI.Blazor.Startup;

// Re-export the beforeStart and afterStarted methods
export const beforeWebStart = Startup.beforeWebStart;
export const beforeServerStart = Startup.beforeServerStart;
export const beforeWebAssemblyStart = Startup.beforeWebAssemblyStart;

export const afterWebStarted = Startup.afterWebStarted;
export const afterServerStarted = Startup.afterServerStarted;
export const afterWebAssemblyStarted = Startup.afterWebAssemblyStarted;

