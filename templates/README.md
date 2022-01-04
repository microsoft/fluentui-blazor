## For Blazor Server
To create a project from the cli:
- `dotnet new fluentuiblazorserver -o {your project name}`

To create a package (after changing the template source):
- `nuget.exe pack {path to repo}\templates\FluentUIBlazorServerApp\Microsoft.Fast.Components.FluentUI.BlazorServer.Template.nuspec -OutputDirectory {output path}`

To install a template:
- `dotnet new -i .\Microsoft.Fast.Components.FluentUI.BlazorServer.Template.{version}.nupkg` (currently version = 1.0.0)

To uninstall a template:
- `dotnet new -u Microsoft.Fast.Components.FluentUI.BlazorServer.Template`

## For Blazor WebAssembly
To create a project from the cli:
-`dotnet new fluentuiblazorwebassembly -o {your project name}`

To create a package (after changing the template source):
- `nuget.exe pack {path to repo}\templates\FluentUIBlazorWebAssemblyApp\Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template.nuspec -OutputDirectory {output path}`

To install a template:
- `dotnet new -i .\Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template.{version}.nupkg` (currently version = 1.0.0)

To uninstall a template:
- `dotnet new -u Microsoft.Fast.Components.FluentUI.BlazorWebAssembly.Template`
