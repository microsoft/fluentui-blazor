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

The component can be customized by adapting the `ChildContent`.  
The areas that need to be associated with the opening of the file selection dialog
must be included in a `label for` that references the component `Id`: E.g. `<label for=“my-file-uploader”>browse...</label>`.

{{ InputFileDefault }}

## Manual upload

By using the parameter `DragDropZoneVisible=“false”`, the component is completely invisible. In this case, use the parameter `AnchorId` to
specify the component that will trigger the opening of the file selection dialog.

{{ InputFileByCode }}

## Mode = InputFileMode.Buffer

By default, the component has a parameter `Mode=“InputFileMode.SaveToTemporaryFolder”`.  
Change this parameter to `InputFileMode.Buffer` to control the reception of files directly into memory.
When the `OnCompleted` event is triggered, the entire file content is present in the `file.Buffer` property.

{{ InputFileBufferMode }}

## Mode = InputFileMode.Stream

If you want to transfer very large files and do not want to save your files locally (in a temporary folder, for example),
you can retrieve the file stream from the `OnFileUploaded` event and the `file.Stream` property.

{{ InputFileStream }}

## Disabled Component

Thanks to the `Disabled` parameter, the component becomes inaccessible to the user from the graphical interface.

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

- The `OnFileCountExceeded` event was replaced by the `OnFileError` event.
