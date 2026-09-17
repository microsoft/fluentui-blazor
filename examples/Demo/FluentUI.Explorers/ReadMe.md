# FluentUI Blazor Assets Explorer

This is a simple Blazor site app that allows you to explore the assets (Icons and Emojis)
of the Fluent UI Blazor library.

## Create the Publish files

1. Before compiling, update the **csproj** file to use the latest NuGet packages in
   `PackageReference`.

2. Restore the NuGet packages
   ```bash
   dotnet restore
   ```

3. Publish the project in Release mode
   ```bash
   # Note: Update net9.0 to match NetVersion in Directory.Build.props (net8.0, net9.0, or net10.0)
   dotnet publish "FluentUI.Explorers.csproj" -c Release -o ./bin/Publish -f net9.0
   ```

4. Compress the resulting folder into a ZIP file.

   ```bash
   Compress-Archive ./bin/Publish/* ./bin/FluentUI-Explorers.zip
   ```

## Deploy to Azure

1. Open Azure Portal and navigate to the Web App **fluentui-explorer** (Slot `v5`)
   Use the Development **Tools / Advanced Tools** to drop the Zip file
   in the `site/wwwroot` folder.

   https://fluentui-explorer-v5.scm.azurewebsites.net/DebugConsole

2. Check the deployment: https://fluentui-explorer-v5.azurewebsites.net
