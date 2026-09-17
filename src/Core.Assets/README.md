# Microsoft.FluentUI.AspNetCore.Components.Assets

## Setup

The **Components.Scripts** project uses several NPM packages.
You should get them automatically the first time you compile this project from Visual Studio.

In the event of an NPM authentication problem (E401), you will probably need to run these command from the `Core.Assets` folder.

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

## Server Build Error

If the build fails with the `401 Unable to authenticate, your authentication token seems to be invalid.` error, 
the authentication token might not be the actual cause. The requested package has probably not yet been downloaded 
and cached by the `dotnet-public-npm` feed.

Clear both the **global** NPM cache and the **local** dependencies in `src\Core.Scripts`, 
then reinstall all packages and their dependencies. This forces a new package request, causing the 
`dotnet-public-npm` feed to retrieve and cache any missing packages. Without this procedure, 
the build server might not find a package in the feed and report the authentication error.

NPM Feed: https://dev.azure.com/dnceng/public/_artifacts/feed/dotnet-public-npm

```powershell
# Clean global cache: C:\.tools
npm cache clean --force
npm cache verify

# Clean local dependencies (src\Core.Scripts)
Remove-Item package-lock.json -Force
Remove-Item node_modules -Recurse -Force -ErrorAction SilentlyContinue

# Reinstall all packages
# To force the download to https://dev.azure.com/dnceng/public/_artifacts/feed/dotnet-public-npm
npm install
```