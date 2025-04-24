---
title: InputFile
route: /InputFile
---

# InputFile

The `FluentInputFile` wraps the native Blazor [InputFile](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads) component
and extends it with drag/drop zone support. See the examples below for some different ways on how to use this component.

**Customization:** You can localize this component by customizing the content of `ChildContent`,
and also the content of the progress area via the `ProgressTemplate` attribute.  
The default progress area displays <i>Loading</i>, <i>Completed</i> or <i>Canceled</i> labels via
the embedded [Localization](https://fluentui-blazor-v5.azurewebsites.net/localization) service.

## Example



{{ InputFileDefault }}

## Manual upload

{{ InputFileByCode }}

## Mode = InputFileMode.Buffer

{{ InputFileBufferMode }}

## Mode = InputFileMode.Stream

{{ InputFileStream }}

## Disabled Component

{{ InputFileDisabled }}

## Known Issues

Starting with .NET 6, the `InputFile` component does not work in Server-Side Blazor applications using Autofac IoC containers.
This issue is being tracked here: [aspnetcore#38842](https://github.com/dotnet/aspnetcore/issues/38842).  

Enable the HubOptions [DisableImplicitFromServicesParameters](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.SignalR.HubOptions.DisableImplicitFromServicesParameters)
in program/startup to workaround this issue.

```csharp
builder.Services
       .AddServerSideBlazor()
       .AddHubOptions(opt =>
       {
           opt.DisableImplicitFromServicesParameters = true;
       });
```


## API FluentInputFile

{{ API Type=FluentInputFile }}

## Migrating to v5

No changes
