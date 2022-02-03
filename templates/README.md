### For Blazor Server

#### Installation
Install the template by running the command:
```
dotnet new -i {path to package}\Microsoft.Fast.Templates.FluentUI.BlazorServer.{version}.nupkg
```
The current version is 1.0.0. After instalation the template will be available to create a new project from both the CLI as from Visual Studio 2022

#### Usage
After installing the template you can create new a project from either the CLI or by creating a new project in Visual studio 2022. 

For creating a project form the CLI run the following command (in the folder of your choice):
```
dotnet new fluentuiblazorserver -o {your project name}
``` 
In Visual Studio you can create a new project by selecting the FluentUI Blazor Server App template in the 'File->New->Project'-dialog.

#### Creating a template package
After changing the template source, a new package needs to be created. Remember to update the version number in the `.nuspec` file (see path below). To create the package run the following command:
```
nuget.exe pack {path to repo}\templates\FluentUIBlazorServerApp\Microsoft.Fast.Templates.FluentUI.BlazorServer.nuspec -OutputDirectory {output path}
```

#### Uninstalling the template
If you want to uninstall the template, both from the CLI and Visual Studio 2022,  run the following command:
```
dotnet new -u Microsoft.Fast.Templates.FluentUI.BlazorServer
```


### For Blazor WebAssembly

#### Installation
Install the template by running the command:
```
dotnet new -i {path to package}\Microsoft.Fast.Templates.FluentUI.BlazorWebAssembly.{version}.nupkg
```
The current version is 1.0.0. After instalation the template will be available to create a new project from both the CLI as from Visual Studio 2022

#### Usage
After installing the template you can create new a project from either the CLI or by creating a new project in Visual studio 2022. 

For creating a project form the CLI run the following command (in the folder of your choice):
```
dotnet new fluentuiBlazorWebAssembly -o {your project name}
``` 
In Visual Studio you can create a new project by selecting the FluentUI Blazor Server App template in the 'File->New->Project'-dialog.

#### Creating a template package
After changing the template source, a new package needs to be created. Remember to update the version number in the `.nuspec` file (see path below). To create the package run the following command:
```
nuget.exe pack {path to repo}\templates\FluentUIBlazorWebAssemblyApp\Microsoft.Fast.Templates.FluentUI.BlazorWebAssembly.nuspec -OutputDirectory {output path}
```

#### Uninstalling the template
If you want to uninstall the template, both from the CLI and Visual Studio 2022,  run the following command:
```
dotnet new -u Microsoft.Fast.Templates.FluentUI.BlazorWebAssembly
```
