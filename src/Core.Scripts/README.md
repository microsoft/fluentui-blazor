# Microsoft.FluentUI.AspNetCore.Components.Scripts

## Setup

The **Components.Scripts** project uses several NPM packages.
You should get them automatically the first time you compile this project from Visual Studio.

In the event of an NPM authentication problem (E401), you will probably need to run these command from the `Core.Scripts` folder.

1. Install the **vsts-npm-auth** command.
   ```bash
   npm install -g vsts-npm-auth --registry https://registry.npmjs.com --always-auth false
   ```
2. Execute this command to get an authentication token.
   ```bash
   vsts-npm-auth -config .npmrc -force
   ```
3. Download and install NPM packages manually.
   ```bash
   npm install
   ```

## Development

  The `ìndex.ts` file is the entry point for the Fluent UI Blazor library.
  It re-exports the `beforeStart` and `afterStarted` methods from the `Startup` file,
  which are used to define and initialize the Fluent UI components and styles.

  -  To create a new **custom component**, you can add a new file in the `Components` folder,
     and register it in the `afterStarted` method in the `Startup` file.

  -  To create a new **custom event**, you can add a new function in the `FluentUICustomEvents` file,
     and register it in the `afterStarted` method in the `Startup` file.

  -  All **FluentUI WebComponents** are defined and initialized in the `FluentUIWebComponents` file.
