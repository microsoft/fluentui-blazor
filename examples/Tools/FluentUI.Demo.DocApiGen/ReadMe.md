# DocApiGen

If API documentation is not included in your project,
you can generate it using this project `FluentUI.Demo.DocApiGen`.

Simply run the project `FluentUI.Demo.DocApiGen` to generate the file.

From Visual Studio

1. Right-click on the project `FluentUI.Demo.DocApiGen`.
1. Select the menu `Debug` > `Start without debugging`.
1. Re-run the project `FluentUI.Demo` to see the changes.

From command line

1. Open a command line in the folder `FluentUI.Demo.DocApiGen`.
1. Run the command
   ```
   dotnet run --xml "./Microsoft.FluentUI.AspNetCore.Components.xml" --dll "../../../src/Core/bin/Debug/net9.0/Microsoft.FluentUI.AspNetCore.Components.dll" --output "../../../examples/Demo/FluentUI.Demo.Client/wwwroot/api-comments.json" --format json
   ```
