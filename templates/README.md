#### Installation
Install the templates by running the command:
```
dotnet new install {path to package}\Microsoft.Fast.Templates.FluentUI.{version}.nupkg
```
The current version can be found on the [NuGet page](https://www.nuget.org/packages/Microsoft.Fast.Templates.FluentUI/). After instalation the templates will be available to create a new project from both the CLI as from Visual Studio 2022

#### Usage
After installing the templates you can create new a project from either the CLI or by creating a new project in Visual studio 2022. 

For a Blazor Server project:
```
dotnet new fluentuiblazorserver -o {your project name}
``` 
For a Blazor WebAssembly project:
```
dotnet new fluentuiBlazorWebAssembly -o {your project name}
``` 
In Visual Studio you can create a new project by selecting either the FluentUI Blazor Server App template or the FluentUI Blazor WebAssembly template in the 'File->New->Project'-dialog.

#### Uninstalling the templates
If you want to uninstall the templates, both from the CLI and Visual Studio 2022,  run the following command:
```
dotnet new uninstall Microsoft.Fast.Templates.FluentUI
```
