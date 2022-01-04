## For Blazor Server
First install the template by running the command `dotnet new -i {path to package}\Microsoft.Fast.Components.FluentUI.BlazorServer.Template.{version}.nupkg` (currently version = 1.0.0)

Then either create a project from the cli by running the command `dotnet new fluentuiblazorserver -o {your project name}` or create a new project in Visual Studio 2022 by selecting the FluentUI Blazor Server App template

To create a package (after changing the template source):
- `nuget.exe pack {path to repo}\templates\FluentUIBlazorServerApp\Microsoft.Fast.Components.FluentUI.BlazorServer.Template.nuspec -OutputDirectory {output path}`

To uninstall the template:
- `dotnet new -u Microsoft.Fast.Components.FluentUI.BlazorServer.Template`

## For Blazor WebAssembly
First install the template by running the command `dotnet new -i .\Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template.{version}.nupkg` (currently version = 1.0.0) 

Then either create a project from the cli by executing the command `dotnet new fluentuiblazorwebassembly -o {your project name}` or create a new project in Visual Studio 2022 by selecting the FluentUI Blazor WebAssembly App template

To create a package (after changing the template source):
- `nuget.exe pack {path to repo}\templates\FluentUIBlazorWebAssemblyApp\Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template.nuspec -OutputDirectory {output path}`

To uninstall the template:
- `dotnet new -u Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template`
