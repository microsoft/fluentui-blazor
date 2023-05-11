## Configuring the `AddFluentUIComponents()` service collection extension
 
You get run-time checks when using the `<FluentIcon>` and `<FluentEmoji>` components on the referenced assets actually being available. 
When running a Blazor Server application with `WindowsAuthentication` enabled, some extra configuration is needed.
To make this work, the `AddFluentUIComponents()` service collection extension needs some configuration.
 
Services and configuration classed have been added to the library for this. You do not need to specify the configuration in code yourself. 
A source generator has been added that reads the settings from the project file and adds the necessary code at compile time. That way,
settings made in the project file and the source code are always kept in sync.
 
The lines that need to be added to the `Program.cs` file are:

```csharp
builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = {see remark below};
    options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
    options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
});

*If you're running your application on Blazor Server, make sure a default HttpClient is available by adding the following:*

```csharp
builder.Services.AddHttpClient();
```
*before adding the `AddFluentUIComponents()` code*

**The `options.HostingModel` setting is used to determine the type of project you are building. Choose `BlazorHostingModel.WebAssembly` OR `BlazorHostingModel.Server` depending on your type of project**

## Scripts 
The hart of this library is formed by the Fluent UI Web Components and the accompanying `web-components.min.js` file. From version 2.3 onwards, the 
script is included in the library itself and no longer needs to be added to your `index.html` or `_Layout.cshtml`. In fact, doing this might lead to 
unpredictable results. 

> **If you are upgrading from an earlier version please remove the script from your `index.html` or `_Layout.cshtml` file.**

The script is added to the application automatically. This way we can safeguard that the you are always using the best matching script version.