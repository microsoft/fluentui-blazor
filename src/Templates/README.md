> **IMPORTANT!!**
> Just as with the standard Blazor Web App template, Blazor will use SSR by default. If you want to have interactive components, make sure you add a rendermode to the app, page or component.!</p>

#### Installation
Install the templates by running the command:
```
dotnet new install {path to package}\Microsoft.FluentUI.AspNetCore.Templates.{version}.nupkg
```
The current version can be found on the [NuGet page](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates/). 

#### Usage
After installing the templates you can create new a project from either the CLI or by creating a new project in Visual studio 2022. 

For creating a Fluent Blazor Web App project from the CLI 
```
dotnet new fluentblazor -o {your project name}
``` 
For creating a Fluent Blazor WebAssembly Standalone App project from the CLI:
```
dotnet new fluentblazorwasm -o {your project name}
``` 
In Visual Studio you can create a new project by selecting either the FluentUI Blazor Server App template or the FluentUI Blazor WebAssembly template in the 'File->New->Project'-dialog.

#### Uninstalling the templates
If you want to uninstall the templates, both from the CLI and Visual Studio 2022,  run the following command:
```
dotnet new uninstall Microsoft.FluentUI.AspNetCore.Templates
```
