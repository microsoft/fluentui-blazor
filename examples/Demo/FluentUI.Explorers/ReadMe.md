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
   dotnet publish "FluentUI.Explorers.csproj" -c Release -o ./bin/Publish -f net9.0
   ```

## Update the Web.Config

To allow an external site to embed your site in an iframe,
you need to add a [Content-Security-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/frame-ancestors) item
the following to the generated ´web.config` file:

```xml
<configuration>
  ...
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <remove name="X-Frame-Options" />
        <add name="Content-Security-Policy" value="frame-ancestors 'self' https://localhost:7026 https://fluentui-blazor.net https://www.fluentui-blazor.net https://fluentui-blazor-v5.azurewebsites.net" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
  ...
</configuration>
```

## Deploy in Azure

1. Compress the resulting folder into a ZIP file.

   ```bash
   Compress-Archive ./bin/Publish/* ./bin/FluentUI-Explorers.zip
   ```

2. Open Azure Portal and navigate to the Web App **fluentui-explorer** (Slot `v5`)

3. Use the Development **Tools / Advanced Tools** to drop the Zip file in the root folder.

4. Check the deployment: https://fluentui-explorer-v5.azurewebsites.net
