# Microsoft.FluentUI.AspNetCore.Components.Scripts

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
